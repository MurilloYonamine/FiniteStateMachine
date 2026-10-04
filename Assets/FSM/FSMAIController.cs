using UnityEngine;
using UnityEngine.AI;

public class FSMAIController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float _visionDistance = 10f;
    [SerializeField] private float _visionAngle = 45f;
    [SerializeField] private float _attackDistance = 7f;

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public AudioSource AudioSource => _audioSource;
    public Transform Player => _player;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(direction, transform.forward);

        if (direction.magnitude < _visionDistance && angle < _visionAngle)
        {
            return true;
        }
        return false;
    }

    public bool IsPlayerBehind()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(direction, transform.forward);

        if (direction.magnitude < 2f && angle > _visionAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0;
        if (direction.magnitude < _attackDistance)
        {
            return true;
        }
        return false;
    }
}
