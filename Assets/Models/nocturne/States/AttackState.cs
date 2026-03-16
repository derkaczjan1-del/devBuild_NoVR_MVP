using UnityEngine;

public class AttackState : BaseState
{
    //AttackState gdyby kiedyœ dodaæ animacje, zmieniæ zasadê zabijania gracza itp.
    public override void Enter()
    {
        Debug.Log("Attacking player");
    }

    public override void Perform()
    {
        float distance = enemy.DistanceToPlayer();

        if (distance > enemy.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.chaseState);
        }
    }

    public override void Exit()
    {
        enemy.Agent.isStopped = false;
    }
}
