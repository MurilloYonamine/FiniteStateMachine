using UnityEngine;
using UnityEngine.AI;
public class Patrol : StateFSM
{
    int currentIndex = -1;
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        state = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDistance = Mathf.Infinity;
        for(int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            float distance = Vector3.Distance(npc.transform.position, GameEnvironment.Singleton.Checkpoints[i].transform.position);

            if (distance < lastDistance)
            {
                currentIndex = i - 1;
                lastDistance = distance;
            }
        }


        Debug.Log("Entrando no estado de Patrulha");
        //currentIndex = 0;
        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {

        if (agent.remainingDistance < 1.0f)
        {
            currentIndex =
                currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1 ?
                0 : currentIndex + 1;

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if (IsPlayerBehind())
        {
            nextState = new Runaway(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        Debug.Log("Saindo do estado de Patrulha");
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}
