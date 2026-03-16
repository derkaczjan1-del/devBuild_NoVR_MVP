using UnityEngine;

public class InvestigateNoiseState : BaseState
{
    public override void Enter()
    {
        //idzie w kierunku ostatniej znanej pozycji gracza
        enemy.Agent.SetDestination(enemy.LastKnownPlayerPosition);
    }

    public override void Perform()
    {
        //jeœli przeciwnik zauwa¿a gracza
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(stateMachine.chaseState);
            return;
        }

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.5f)
        {
            stateMachine.ChangeState(stateMachine.suspicionState);
        }
    }
    public override void Exit()
    {

    }
}
