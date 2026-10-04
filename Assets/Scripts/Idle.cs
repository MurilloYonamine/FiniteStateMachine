using UnityEngine;
using UnityEngine.AI;
public class Idle : StateFSM
{
    float timeToPatrol = 1.0f;
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        state = STATE.IDLE;
    }

    public override void Enter()
    {
        Debug.Log("Entrando no estado IDLE");
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {

        if(timeToPatrol > 0) timeToPatrol -= Time.deltaTime;

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        else if(Random.Range(0, 1000) < 10 && timeToPatrol <= 0)
        {
            nextState = new Patrol(npc, agent, anim, player);

            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        Debug.Log("Saindo do estado IDLE");
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
