// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour, IPauseable
{
    [SerializeField] private PhaseData[] _phases;
    [SerializeField] private PhaseSpawner _spawner;
    [SerializeField] private ResultScreen _results;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActionAsset _uiActions;

    private PauseManager _pauseManager;
    private InputActionMap _uiMap;
    private int _phaseIndex;
    private int _remainingNpcs;
    private float _remainingTime;
    private bool _playing;
    private bool _paused;

    public static GameManager Instance { get; private set; }
    public bool IsPlaying => _playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _pauseManager = PauseManager.Instance;
        _uiMap = _uiActions.FindActionMap("UI", true);
    }

    private void OnEnable() => _pauseManager.Register(this);
    private void OnDisable() => _pauseManager.Unregister(this);

    private void Update()
    {
        if (!_playing || _paused) return;
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
            ShowTime();
            Lose();
            return;
        }
        ShowTime();
    }

    public void EnterIntro()
    {
        _pauseManager.enabled = false;
        _timerText.gameObject.SetActive(false);
        SetGameplayInput(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        _pauseManager.enabled = true;
        _timerText.gameObject.SetActive(true);
        StartPhase();
    }

    private void StartPhase()
    {
        _results.Hide();
        for (int i = 0; i < _spawner.Npcs.Count; i++)
        {
            _spawner.Npcs[i].DeathFinished -= OnNpcDeathFinished;
        }

        PhaseData phase = _phases[_phaseIndex];
        if (!_spawner.Spawn(phase.npcCount)) return;

        _remainingNpcs = _spawner.Npcs.Count;
        for (int i = 0; i < _spawner.Npcs.Count; i++)
        {
            _spawner.Npcs[i].DeathFinished += OnNpcDeathFinished;
        }

        _remainingTime = phase.duration;
        ShowTime();
        SetGameplayInput(true);
        _playing = true;
    }

    private void OnNpcDeathFinished(FSMAIController npc)
    {
        if (!_playing) return;
        _remainingNpcs--;

        if (_remainingNpcs > 0) return;
        _playing = false;
        _pauseManager.enabled = false;
        SetGameplayInput(false);
        _results.ShowVictory(_phaseIndex + 1 < _phases.Length);
    }

    private void Lose()
    {
        _playing = false;
        _pauseManager.enabled = false;
        SetGameplayInput(false);
        _results.ShowDefeat(_spawner.Npcs);
    }

    public void NextPhase()
    {
        if (_phaseIndex + 1 >= _phases.Length) return;

        _phaseIndex++;
        _pauseManager.enabled = true;
        StartPhase();
    }

    public void RetryPhase()
    {
        _pauseManager.enabled = true;
        StartPhase();
    }

    public void Quit() => Application.Quit();
    public void OnPause() => _paused = true;
    public void OnResume() => _paused = false;

    private void SetGameplayInput(bool enabled)
    {
        _pauseManager.SetPlayerGameplayActive(enabled);
        if (enabled)
        {
            _uiMap.Disable();
            _playerInput.currentActionMap.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            _playerInput.currentActionMap.Disable();
            _uiMap.Enable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ShowTime()
    {
        int seconds = Mathf.CeilToInt(_remainingTime);
        _timerText.text = $"{seconds / 60:00}:{seconds % 60:00}";
    }
}
