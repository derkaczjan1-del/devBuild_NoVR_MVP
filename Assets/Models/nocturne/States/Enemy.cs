using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get=>agent; }
    [SerializeField] private string currentState;
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] Canvas deadCanvas;
    public EnemyPath path;
    GameObject player;
    bool playerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindWithTag("Player");
        deadCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceToPlayer() < chaseDistance)
        {
            Chase();
                    
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            agent.isStopped = true;
            playerDead = true;
            deadCanvas.gameObject.SetActive(true);
        }
    }

    private void Chase()
    {
        agent.destination = player.transform.position;
    }

    private float DistanceToPlayer()
    {
        
        return Vector3.Distance(player.transform.position, transform.position);
    }
    
    public bool IsPlayerDead()
    {
        return playerDead;
    }
}
