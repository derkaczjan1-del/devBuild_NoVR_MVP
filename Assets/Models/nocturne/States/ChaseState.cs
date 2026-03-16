using UnityEngine;

public class ChaseState : BaseState
{
    public override void Enter()
    {
        Debug.Log("Entering Chase State");
        enemy.Agent.isStopped = false;
    }

    public override void Perform()
    {
        if (enemy == null) return;

        //je¿eli gracz jest w kryjówce, przeciwnik nie bêdzie go goni³
        if (enemy.Player.GetComponent<PlayerInteract>().IsInHideout()) return;

        enemy.isChasing = true;

        float distance = enemy.DistanceToPlayer();

        if (distance <= enemy.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.attackState);
            return;
        }

        if (distance > enemy.ChaseDistance)
        {
            stateMachine.ChangeState(stateMachine.suspicionState);
            return;
        }

        Vector3 playerPos = enemy.Player.transform.position;

        if (enemy.CanSeePlayer())
        {
            enemy.Agent.SetDestination(playerPos);
        }
    }

    public override void Exit()
    {
        Debug.Log("Leaving Chase State");
        enemy.isChasing = false;
    }
}