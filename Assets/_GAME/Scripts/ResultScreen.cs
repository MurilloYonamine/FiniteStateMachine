// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _defeatPanel;
    [SerializeField] private Button _nextPhaseButton;
    [SerializeField] private Button _victoryQuitButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _defeatQuitButton;
    [SerializeField] private float _cameraDuration = 3f;

    private readonly List<CinemachineCamera> _defeatCameras = new List<CinemachineCamera>();
    private int _cameraIndex;
    private float _cameraTimer;

    private void Awake()
    {
        _victoryPanel.SetActive(false);
        _defeatPanel.SetActive(false);
    }

    private void OnEnable()
    {
        _nextPhaseButton.onClick.AddListener(NextPhase);
        _victoryQuitButton.onClick.AddListener(Quit);
        _retryButton.onClick.AddListener(RetryPhase);
        _defeatQuitButton.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        _nextPhaseButton.onClick.RemoveListener(NextPhase);
        _victoryQuitButton.onClick.RemoveListener(Quit);
        _retryButton.onClick.RemoveListener(RetryPhase);
        _defeatQuitButton.onClick.RemoveListener(Quit);
    }

    private void NextPhase() => GameManager.Instance.NextPhase();
    private void RetryPhase() => GameManager.Instance.RetryPhase();
    private void Quit() => GameManager.Instance.Quit();

    private void Update()
    {
        if (_defeatCameras.Count < 2) return;
        _cameraTimer += Time.deltaTime;

        if (_cameraTimer < _cameraDuration) return;
        _cameraTimer = 0f;

        ShowCamera((_cameraIndex + 1) % _defeatCameras.Count);
    }

    public void ShowVictory(bool hasNextPhase)
    {
        _nextPhaseButton.interactable = hasNextPhase;
        _victoryPanel.SetActive(true);
    }

    public void ShowDefeat(List<FSMAIController> npcs)
    {
        _defeatCameras.Clear();
        _cameraIndex = 0;
        _cameraTimer = 0f;
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i].IsDead) continue;
            npcs[i].Celebrate();
            CinemachineCamera camera = npcs[i].GetComponentInChildren<CinemachineCamera>(true);
            camera.Priority = 100;
            _defeatCameras.Add(camera);
        }

        _defeatPanel.SetActive(true);
        if (_defeatCameras.Count > 0)
            ShowCamera(0);
    }

    public void Hide()
    {
        for (int i = 0; i < _defeatCameras.Count; i++)
            _defeatCameras[i].enabled = false;
        _defeatCameras.Clear();
        _victoryPanel.SetActive(false);
        _defeatPanel.SetActive(false);
    }

    private void ShowCamera(int index)
    {
        _defeatCameras[_cameraIndex].enabled = false;
        _cameraIndex = index;
        _defeatCameras[_cameraIndex].enabled = true;
    }
}
