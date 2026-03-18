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

        enemy.isChasing = true;

        float distance = enemy.DistanceToPlayer();

        if (distance <= enemy.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.attackState);
            return;
        }

        //jeœli widzi gracza - update pozycji
        if (enemy.CanSeePlayer())
        {
            enemy.LastKnownPlayerPosition = enemy.Player.transform.position;
        }

        //AWSZE idŸ do ostatniej znanej pozycji
        enemy.Agent.SetDestination(enemy.LastKnownPlayerPosition);
    }

    public override void Exit()
    {
        Debug.Log("Leaving Chase State");
        enemy.isChasing = false;
    }
}