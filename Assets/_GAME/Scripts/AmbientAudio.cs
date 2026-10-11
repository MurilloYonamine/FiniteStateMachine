// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;

public class AmbientAudio : MonoBehaviour, IPauseable
{
    [SerializeField] private AudioClip _clip;
    [SerializeField, Range(0f, 1f)] private float _volume = 0.35f;

    private AudioSource _source;
    private PauseManager _pauseManager;
    private bool _initialized;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _source.clip = _clip;
        _source.volume = _volume;
        _source.loop = true;
        _source.playOnAwake = false;
        _source.spatialBlend = 0f;
    }

    private void Start()
    {
        _source.Play();
        _pauseManager = PauseManager.Instance;
        _pauseManager.Register(this);
        _initialized = true;
    }

    private void OnEnable()
    {
        if (!_initialized) return;
        _source.Play();
        _pauseManager.Register(this);
    }

    private void OnDisable()
    {
        if (!_initialized) return;
        _pauseManager.Unregister(this);
        _source.Stop();
    }

    public void OnPause() => _source.Pause();
    public void OnResume() => _source.UnPause();
}
