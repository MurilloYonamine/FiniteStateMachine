// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;
using UnityEngine.InputSystem;

public class NPCFinder : MonoBehaviour, IPauseable
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private InputActionReference _findAction;
    [SerializeField] private float _maxDistance = 50f;
    [SerializeField] private AudioClip _foundClip;

    private PauseManager _pauseManager;
    private AudioSource _foundAudio;
    private bool _paused;
    private bool _initialized;

    private void Awake()
    {
        _foundAudio = gameObject.AddComponent<AudioSource>();
        _foundAudio.playOnAwake = false;
        _foundAudio.spatialBlend = 0f;
    }

    private void Start()
    {
        _pauseManager = PauseManager.Instance;
        _pauseManager.Register(this);
        _initialized = true;
    }

    private void OnEnable()
    {
        if (_initialized) _pauseManager.Register(this);
        _findAction.action.Enable();
    }

    private void OnDisable()
    {
        _findAction.action.Disable();
        if (_initialized) _pauseManager.Unregister(this);
    }

    private void Update()
    {
        if (_paused || !GameManager.Instance.IsPlaying || !_findAction.action.WasPressedThisFrame()) return;

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (!Physics.Raycast(ray, out RaycastHit hit, _maxDistance)) return;
        
        FSMAIController npc = hit.collider.GetComponentInParent<FSMAIController>();
        if (npc != null && !npc.IsDead)
        {
            npc.Die();
            _foundAudio.PlayOneShot(_foundClip);
        }
    }

    public void OnPause()
    {
        _paused = true;
        _foundAudio.Pause();
    }

    public void OnResume()
    {
        _paused = false;
        _foundAudio.UnPause();
    }
}
