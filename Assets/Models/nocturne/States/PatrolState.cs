using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    //track which waypoint currently targeting
    public int waypointIndex;
    public override void Enter()
    {
        Debug.Log("Patrolling");
        enemy.Agent.isStopped = false;
        enemy.WasRecentlySuspicious = false;
    }

    public override void Perform()
    {
        //szansa, ¿e sprawdzi co jest za nim
        enemy.CheckBehind();
        //wykonuje obrót
        enemy.HandleLookAround();

        //je¿eli zobaczy gracza, a ten nie jest w kryjówce
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(stateMachine.chaseState);
        }
        
        //system patrolowania, przechodzenie miêdzy waypointami
        PatrolCycle();
    }

    public override void Exit()
    {
        
    }

    public void PatrolCycle()
    {
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            //losowa szansa, ¿e przeciwnik bêdzie siê rozgl¹da³ dooko³a
            enemy.TryLookAround();
            

            if (waypointIndex < enemy.path.waypoints.Count-1)
            {
                waypointIndex++;
            }
            else
            {
                waypointIndex = 0;
            }
            enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
        }
    }
}
