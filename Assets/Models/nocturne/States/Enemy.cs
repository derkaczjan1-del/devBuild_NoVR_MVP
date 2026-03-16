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

    [Header("Movement")]
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float angularSpeed = 120f;
    [SerializeField] float accelerationSpeed = 20f;

    [Header("States")]
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 3f;
    [SerializeField] float backCheckDistance = 2.5f;
    [SerializeField] float attackRange = 1f;
    
    [Header("Eyesight")]
    [SerializeField] float viewDistance = 10f;
    [SerializeField] float viewAngle = 90f;
    [SerializeField] float centralVisionAngle = 60f;
    [SerializeField] float peripheralVisionAngle = 140f;
    [SerializeField] LayerMask obstacleMask;

    [Header("Awareness")]
    [SerializeField] float lookAroundChance = 0.15f;
    [SerializeField] float lookAroundDuration = 2f;

    [Header("Others")]
    [SerializeField] Canvas deadCanvas;
    [SerializeField] private string currentState;

    float backCheckTimer;
    float lookTimer;
    bool lookingAround;
    Quaternion lookTargetRotation;

    public EnemyPath path;
    GameObject player;
    Vector3 noisePosition;
    Vector3 lastKnownPlayerPosition;
    public GameObject Player => player;
    public float SuspicionTime => suspicionTime;
    public float AttackRange => attackRange;
    public float ChaseDistance => chaseDistance;
    public bool WasRecentlySuspicious { get; set; }
    public bool isChasing = false;

    bool playerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindWithTag("Player");
        deadCanvas.gameObject.SetActive(false);
        //ustawienie poruszania siê przeciwnika
        agent.speed = patrolSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = accelerationSpeed;
        agent.updateRotation = false;

    }

    // Update is called once per frame
    void Update()
    {
        //kontroluje gdzie patrzy przeciwnik
        HandleRotation();
        //kontroluje prêdkoœæ obrotu przeciwnika, spowalniaj¹c go podczas ostrych zakrêtów
        HandleTurningSpeed();
        currentState = stateMachine.activeState.GetType().Name;
    }

    //Przeciwnik zabija gracza, gdy ten wejdzie w jego trigger 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            agent.isStopped = true;
            playerDead = true;
            deadCanvas.gameObject.SetActive(true);
        }
    }

    //Metoda obliczaj¹ca odleg³oœæ miêdzy przeciwnikiem a graczem
    public float DistanceToPlayer()
    {
        
        return Vector3.Distance(player.transform.position, transform.position);
    }
    
    public bool IsPlayerDead()
    {
        return playerDead;
    }

    //Zapamiêtanie ostatniej pozycji gracza
    public Vector3 LastKnownPlayerPosition
    {
        get => lastKnownPlayerPosition;
        set => lastKnownPlayerPosition = value;
    }

    //Metoda sprawdzaj¹ca, czy przeciwnik widzi gracza
    public bool CanSeePlayer()
    {
        //je¿eli gracz jest w kryjówce, przeciwnik go nie widzi
        if (player.GetComponent<PlayerInteract>().IsInHideout()) return false;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < centralVisionAngle / 2)
        {
            if (!Physics.Raycast(transform.position + Vector3.up * 1.6f, directionToPlayer, distance, obstacleMask))
            {
                LastKnownPlayerPosition = player.transform.position;
                return true;
            }
        }

        if (angle < peripheralVisionAngle / 2)
        {
            stateMachine.ChangeState(stateMachine.suspicionState);
        }

        return false;
    }

    //Metoda wywo³ywana, gdy przeciwnik us³yszy ha³as, np. krok gracza
    public void HearNoise(Vector3 position)
    {
        noisePosition = position;
        LastKnownPlayerPosition = position;

        stateMachine.ChangeState(stateMachine.investigateNoiseState);
    }

    //Metoda próbuj¹ca sprawiæ, by przeciwnik rozejrza³ siê dooko³a, wiêksza szansa kiedy jest "podejrzliwy" - suspicionState
    public void TryLookAround()
    {
        if (lookingAround) return;

        float chance = lookAroundChance;

        if (WasRecentlySuspicious)
        {
            chance += 0.2f;
        }

        if (Random.value < chance)
        {
            lookingAround = true;
            lookTimer = lookAroundDuration;

            float randomAngle = Random.Range(-160f, 160f);
            lookTargetRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomAngle, 0);
        }
    
    }

    //Metoda pwooduj¹ca siê rozgl¹danie przeciwnika
    public void HandleLookAround()
    {
        if (!lookingAround) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookTargetRotation,
            Time.deltaTime * 2f
        );

        lookTimer -= Time.deltaTime;

        if (lookTimer <= 0f)
        {
            lookingAround = false;
        }
    }

    //eliminowanie exploitu polegaj¹cego na podchodzeniu przeciwnika od ty³u, jeœli gracz jest zbyt blisko
    public void CheckBehind()
    {
        Vector3 toPlayer = player.transform.position - transform.position;

        float distance = toPlayer.magnitude;

        if (distance > backCheckDistance)
        {
            backCheckTimer = 0;
            return;
        }

        float angle = Vector3.Angle(transform.forward, toPlayer);

        if (angle > 120f)
        {
            backCheckTimer += Time.deltaTime;

            if (backCheckTimer > 1.0f)
            {
                LastKnownPlayerPosition = player.transform.position;
                stateMachine.ChangeState(stateMachine.suspicionState);
            }
        }
    }

    void HandleTurningSpeed()
    {
        if (agent.velocity.magnitude < 0.1f) return;

        Vector3 direction = agent.desiredVelocity.normalized;

        float angle = Vector3.Angle(transform.forward, direction);

        float turnSlowdown = Mathf.Clamp01(angle / 90f);

        agent.speed = Mathf.Lerp(3.5f, 2.0f, turnSlowdown);
    }

    void HandleRotation()
    {
        if (agent.desiredVelocity.sqrMagnitude < 0.1f) return;

        Quaternion targetRotation =
            Quaternion.LookRotation(agent.desiredVelocity);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 5f
        );
    }

    //Metoda zwracaj¹ca punkt na drodze NavMeshAgenta, co pozwala na p³ynniejsze zakrêty przeciwnika
    public Vector3 GetSmoothedTarget()
    {
        if (agent.path == null) return agent.destination;

        var corners = agent.path.corners;

        if (corners.Length > 1)
        {
            return corners[1]; // drugi waypoint = p³ynniejszy zakrêt
        }

        return agent.destination;
    }

    //wizualizacja zasiêgu widzenia przeciwnika w edytorze Unity
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + left * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + right * viewDistance);
    }


}
