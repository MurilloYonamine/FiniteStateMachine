// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerFlashlight : MonoBehaviour, IPauseable
{
    [SerializeField] private float _intensity = 5f;
    [SerializeField] private float _range = 25f;
    [SerializeField] private float _spotAngle = 65f;
    [SerializeField] private bool _startsOn = true;

    private Light _light;
    private InputAction _toggleAction;
    private PauseManager _pauseManager;
    private bool _initialized;
    private bool _paused;

    private void Awake()
    {
        _light = gameObject.AddComponent<Light>();
        _light.type = LightType.Spot;
        _light.intensity = _intensity;
        _light.range = _range;
        _light.spotAngle = _spotAngle;
        _light.shadows = LightShadows.Soft;
        _light.enabled = _startsOn;
        _toggleAction = new InputAction("Flashlight", InputActionType.Button, "<Keyboard>/f");
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
        _toggleAction.Enable();
    }

    private void OnDisable()
    {
        _toggleAction.Disable();
        if (_initialized) _pauseManager.Unregister(this);
    }

    private void OnDestroy() => _toggleAction.Dispose();

    private void Update()
    {
        if (_paused || !GameManager.Instance.IsPlaying) return;
        if (_toggleAction.WasPressedThisFrame()) _light.enabled = !_light.enabled;
    }

    public void OnPause() => _paused = true;
    public void OnResume() => _paused = false;
}
