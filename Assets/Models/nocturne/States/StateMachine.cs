using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public BaseState activeState;
    public PatrolState patrolState;
    public ChaseState chaseState;
    public SuspicionState suspicionState;
    public AttackState attackState;
    public InvestigateNoiseState investigateNoiseState;
    public void Initialise()
    {
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        suspicionState = new SuspicionState();
        attackState = new AttackState();
        investigateNoiseState = new InvestigateNoiseState();

        ChangeState(patrolState);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        //check activeState != null
        if(activeState != null)
        {
            //cleanup on activeState
            activeState.Exit();
        }
        //change to new state
        activeState = newState;

        if (activeState != null)
        {
            //setup new state
            activeState.stateMachine = this;    
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }

    public bool IsInState(BaseState state)
    {
        return activeState == state;
    }
}
