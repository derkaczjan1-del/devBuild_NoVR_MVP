using UnityEngine;

public class SuspicionState : BaseState
{
    float timer;

    public override void Enter()
    {
        Debug.Log("Suspicious, looking around");
        timer = 0;
        enemy.Agent.SetDestination(enemy.LastKnownPlayerPosition);

        //zmienna zwiêkszaj¹ca szansa, ¿e przeciwnik bêdzie siê rozgl¹da³ dooko³a
        enemy.WasRecentlySuspicious = true;
    }

    public override void Perform()
    {
        timer += Time.deltaTime;
        Vector3 directionToPlayer = enemy.LastKnownPlayerPosition - enemy.transform.position;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation,targetRotation,Time.deltaTime * 2f);


        if (timer >= enemy.SuspicionTime)
        {
            stateMachine.ChangeState(stateMachine.patrolState);
        }
    }

    public override void Exit()
    {
        enemy.Agent.isStopped = false;
    }
}