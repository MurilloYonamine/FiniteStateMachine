// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActionAsset _uiActions;
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private Behaviour[] _playerBehaviours = new Behaviour[0];

    private readonly List<IPauseable> _pauseables = new List<IPauseable>();
    private InputActionMap _uiActionMap;
    private CursorLockMode _previousCursorLockMode;
    private bool _previousCursorVisibility;
    private bool _isOpen;
    public static PauseManager Instance { get; private set; }
    public bool IsPaused => _isOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _uiActionMap = _uiActions.FindActionMap("UI", true);
        _pauseUI.SetActive(false);
    }

    public void Register(IPauseable pauseable)
    {
        _pauseables.Add(pauseable);
        if (_isOpen) pauseable.OnPause();
    }

    public void Unregister(IPauseable pauseable)
    {
        _pauseables.Remove(pauseable);
    }

    public void SetPlayerGameplayActive(bool active)
    {
        for (int i = 0; i < _playerBehaviours.Length; i++)
        {
            _playerBehaviours[i].enabled = active;
        }
    }

    private void OnEnable()
    {
        _pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_isOpen) SetOpen(false);
        _pauseAction.action.Disable();
    }

    private void Update()
    {
        if (_pauseAction.action.WasPressedThisFrame())
            SetOpen(!_isOpen);
    }

    public void SetOpen(bool isOpen)
    {
        if (_isOpen == isOpen)
            return;

        _isOpen = isOpen;
        SetPlayerGameplayActive(!isOpen);
        for (int i = 0; i < _pauseables.Count; i++)
        {
            if (isOpen) _pauseables[i].OnPause();
            else _pauseables[i].OnResume();
        }

        if (isOpen)
        {
            _previousCursorLockMode = Cursor.lockState;
            _previousCursorVisibility = Cursor.visible;
            _playerInput.currentActionMap.Disable();
            _uiActionMap.Enable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            _uiActionMap.Disable();
            _playerInput.currentActionMap.Enable();
            Cursor.lockState = _previousCursorLockMode;
            Cursor.visible = _previousCursorVisibility;
        }

        _pauseUI.SetActive(isOpen);
    }
}
