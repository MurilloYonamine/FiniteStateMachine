using UnityEngine;
using UnityEngine.AI;

public class StateFSM
{
    public enum STATE { IDLE, PATROL, PURSUE, ATTACK, DEAD, RUNAWAY };
    public enum EVENT { ENTER, UPDATE, EXIT  };
    public STATE state;
    public EVENT stage;

    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    public StateFSM nextState;
    protected NavMeshAgent agent;

    float visionDistance = 10.0f;
    float visionAngle = 30.0f;
    float attackDistance = 7.0f;

    public StateFSM(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        anim = _anim;
        player = _player;
        agent = _agent;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public StateFSM Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;

        float angle = Vector3.Angle(direction, npc.transform.forward);

        if(direction.magnitude < visionDistance && angle < visionAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if(direction.magnitude < attackDistance)
        {
            return true;
        }
        return false;
    }

    public bool IsPlayerBehind()
    {
        Vector3 direction = npc.transform.position - player.position;

        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < 2f && angle < visionAngle)
        {
            return true;
        }
        return false;
    }
}
