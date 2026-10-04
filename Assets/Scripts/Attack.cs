using UnityEngine;
using UnityEngine.AI;

public class Attack : StateFSM
{
    float rotationSpeed = 2.0f;
    AudioSource shoot;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base (_npc, _agent, _anim, _player)
    {
        state = STATE.ATTACK;
        shoot = npc.GetComponent<AudioSource>();
        agent.isStopped = true;
    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting");
        shoot.Play();
        base.Enter();
    }

    public override void Update()
    {

        Vector3 direction = player.position - npc.transform.position;

        direction.y = 0;

        Quaternion newRotation = Quaternion.LookRotation(direction);

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        if (!CanSeePlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        shoot.Stop();
        base.Exit();
    }
}
