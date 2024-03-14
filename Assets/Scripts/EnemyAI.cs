using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyAI : MonoBehaviour
{
    [Header("REFERENCES")]
    
    [SerializeField]
    private NavMeshAgent agent;
    
    [SerializeField]
    private Animator animator;
    
    private Transform player;
    
    private PlayerStat playerStat;

    [SerializeField] 
    private AudioSource audioSource;
    
    [Header("ENEMY STAT")]
    
    [SerializeField]
    private float maxHealth = 50;
    
    [SerializeField]
    private float currentHealth;
    
    [SerializeField]
    private float detectionRange;
    
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float attackDelay;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float chaseSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float damage;

    [Header("Wandering Parameters")] 
    [SerializeField]
    private float wanderingWaitTimeMin;
    [SerializeField]
    private float wanderingWaitTimeMax;

    [SerializeField] 
    private float wanderingDistanceMin;

    [SerializeField] 
    private float wanderingDistanceMax;
    
    private bool hasDestination;
    private bool isAttacking;
    public bool isDead;

    private void Awake()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform;
        playerStat = playerTransform.GetComponent<PlayerStat>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= detectionRange && !playerStat.isDead)
        {
            agent.speed = chaseSpeed;

            Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            if (!isAttacking)
            {
                if (Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    StartCoroutine(AttackPlayer());
                }
                else
                {
                    if (agent.enabled)
                    {
                        agent.SetDestination(player.position);
                    }
                    
                }
            }
        }
        else
        {
            agent.speed = walkSpeed;
            if (agent.remainingDistance < 0.75f && !hasDestination)
            {
                StartCoroutine(GetNewDestination());
            }
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;
        
        animator.SetTrigger("Attack");
        audioSource.Play();
        playerStat.TakeDamage(damage);
        yield return new WaitForSeconds(attackDelay);
        if (agent.enabled)
        {
            agent.isStopped = false;
        }
        
        isAttacking = false;
    }
    private IEnumerator GetNewDestination()
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));

        Vector3 nextDestination = transform.position;

        nextDestination += Random.Range(wanderingDistanceMin,wanderingDistanceMax) * 
                           new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            
        }
        hasDestination = false;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            agent.enabled = false;
            enabled = false;
            animator.SetTrigger("Die");


        }
        else
        {
            animator.SetTrigger("GetHit");
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    
    
}
