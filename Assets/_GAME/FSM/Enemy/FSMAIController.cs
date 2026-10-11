// Author: Murillo Gomes Yonamine
// Date: 04/10/2026

using System;
using UnityEngine;
using UnityEngine.AI;

public class FSMAIController : MonoBehaviour, IPauseable
{
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int DanceHash = Animator.StringToHash("Dance");

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _visionDistance = 10f;
    [SerializeField] private float _visionAngle = 45f;
    [SerializeField] private float _attackDistance = 7f;
    [SerializeField] private float _relocateDistance = 12f;
    [SerializeField] private float _scanAngle = 35f;
    [SerializeField] private float _scanSpeed = 1f;
    [SerializeField] private AudioClip _footstepOne;
    [SerializeField] private AudioClip _footstepTwo;
    [SerializeField] private AudioClip _deathClip;

    private PauseManager _pauseManager;
    private AudioSource _footstepAudio;
    private Camera _playerCamera;
    private Transform _hidePoint;
    private Transform _previousHidePoint;
    private bool _isDead;
    private bool _deathFinished;
    private bool _paused;
    private bool _nextFootstepIsSecond;
    private float _footstepTimer;
    private NavMeshPath _path;

    public event Action<FSMAIController> DeathFinished;
    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public AudioSource AudioSource => _audioSource;
    public Transform Player => _player;
    public Transform HidePoint => _hidePoint;
    public float RelocateDistance => _relocateDistance;
    public float ScanAngle => _scanAngle;
    public float ScanSpeed => _scanSpeed;
    public bool IsDead => _isDead;
    public bool IsPaused => _paused;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _footstepAudio = gameObject.AddComponent<AudioSource>();
        _pauseManager = PauseManager.Instance;
        _path = new NavMeshPath();
    }

    private void OnEnable() => _pauseManager.Register(this);
    private void OnDisable() => _pauseManager.Unregister(this);

    private void Update()
    {
        if (_paused || _isDead || !_agent.enabled || _agent.isStopped ||
            _agent.velocity.sqrMagnitude < 0.25f)
        {
            _footstepTimer = 0f;
            return;
        }

        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer > 0f) return;

        _footstepAudio.PlayOneShot(_nextFootstepIsSecond ? _footstepTwo : _footstepOne);
        _nextFootstepIsSecond = !_nextFootstepIsSecond;
        _footstepTimer = _agent.speed > 3f ? 0.3f : 0.45f;
    }

    public void Initialize(Camera playerCamera, Transform hidePoint)
    {
        _playerCamera = playerCamera;
        _player = playerCamera.transform.root;
        _hidePoint = hidePoint;
    }

    public bool IsObserved()
    {
        Vector3 toNpc = transform.position + Vector3.up * 1.5f - _playerCamera.transform.position;
        if (toNpc.magnitude > _visionDistance ||
            Vector3.Angle(_playerCamera.transform.forward, toNpc) > _visionAngle)
            return false;

        if (Physics.Raycast(_playerCamera.transform.position, toNpc.normalized, out RaycastHit hit, toNpc.magnitude))
            return hit.transform.IsChildOf(transform);
        return true;
    }

    public bool EyesMeet()
    {
        Vector3 toPlayer = _playerCamera.transform.position - transform.position;
        toPlayer.y = 0f;
        return IsObserved() && Vector3.Angle(transform.forward, toPlayer) < _visionAngle;
    }

    public bool PlayerIsFar() => Vector3.Distance(_player.position, transform.position) > _relocateDistance;

    public bool CanSeePlayer() => IsObserved();
    public bool IsPlayerBehind() => EyesMeet();
    public bool CanAttackPlayer() => Vector3.Distance(_player.position, transform.position) < _attackDistance;

    public Transform FindNextHidePoint()
    {
        Transform choice = null;
        Transform fallback = null;
        int options = 0;
        var points = GameEnvironment.Singleton.SafeLocations;
        for (int i = 0; i < points.Count; i++)
        {
            Transform point = points[i].transform;
            if (point == _hidePoint ||
                !NavMesh.CalculatePath(transform.position, point.position, NavMesh.AllAreas, _path) ||
                _path.status != NavMeshPathStatus.PathComplete)
                continue;

            if (point == _previousHidePoint)
            {
                fallback = point;
                continue;
            }

            options++;
            if (UnityEngine.Random.Range(0, options) == 0)
                choice = point;
        }
        return choice != null ? choice : fallback;
    }

    public bool GoTo(Transform hidePoint, float speed)
    {
        if (hidePoint == null) return false;
        _agent.speed = speed;
        _agent.isStopped = false;
        if (!_agent.SetDestination(hidePoint.position)) return false;
        _previousHidePoint = _hidePoint;
        _hidePoint = hidePoint;
        return true;
    }

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;
        _footstepAudio.Stop();
        _audioSource.PlayOneShot(_deathClip);
        _animator.SetTrigger(DieHash);
    }

    public void FinishDeath()
    {
        if (_deathFinished) return;
        _deathFinished = true;
        DeathFinished?.Invoke(this);
    }

    public void Celebrate()
    {
        if (_isDead) return;
        _agent.isStopped = true;
        _agent.ResetPath();
        _audioSource.Stop();
        _footstepAudio.Stop();
        _animator.SetTrigger(DanceHash);
    }

    public void OnPause()
    {
        _paused = true;
        _animator.speed = 0f;
        _footstepAudio.Pause();
        if (_agent.enabled) _agent.isStopped = true;
    }

    public void OnResume()
    {
        _paused = false;
        _animator.speed = 1f;
        _footstepAudio.UnPause();
        if (_agent.enabled) _agent.isStopped = false;
    }
}
