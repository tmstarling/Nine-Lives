using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class enemyAI: MonoBehaviour, IDamage, IOpen
{
    [SerializeField] Renderer enemyRenderer;
    [SerializeField] Animator enemyAnimator;

    [SerializeField] int maxHealth;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float attackCooldown;
    [SerializeField] int viewAngle;
    [SerializeField] float attackRange;

    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] Transform visionPoint;
    [SerializeField] float turnSpeed;
    [SerializeField] float patrolRadius;
    [SerializeField] float waitTime = 2f;

    Color originalColor;
    float lastAttackTime;
    float patrolWaitTimer;
    float playerAngle;
    float originalStoppingDistance;
    bool playerDetected;
    Vector3 directionToPlayer;
    Vector3 homePosition;
    int currentHealth;

    enum EnemyState { Patrolling, Chasing, Attacking, Searching }
    EnemyState currentState = EnemyState.Patrolling;

    public bool IsOpen => throw new System.NotImplementedException();

    void Start()
    {
        InitializeEnemy();
    }

    void Update()
    {
        UpdateAnimations();
        HandleEnemyBehavior();
    }

    void InitializeEnemy()
    {
        originalColor = enemyRenderer.material.color;
        currentHealth = maxHealth;
        homePosition = transform.position;
        originalStoppingDistance = navAgent.stoppingDistance;

        if (gamemanager.instance != null)
        {
            gamemanager.instance.updateGameGoal(1);
        }
    }

    void UpdateAnimations()
    {
        if (enemyAnimator != null)
        {
            float speed = navAgent.velocity.magnitude / navAgent.speed;
            enemyAnimator.SetFloat("Speed", speed);
            enemyAnimator.SetBool("IsAttacking", currentState == EnemyState.Attacking);
        }
    }

    void HandleEnemyBehavior()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                PatrolBehavior();
                break;
            case EnemyState.Chasing:
                ChaseBehavior();
                break;
            case EnemyState.Attacking:
                AttackBehavior();
                break;
            case EnemyState.Searching:
                SearchBehavior();
                break;
        }

        
        if (playerDetected && CanSeePlayer())
        {
            currentState = EnemyState.Chasing;
        }
        else if (playerDetected && currentState == EnemyState.Chasing)
        {
            currentState = EnemyState.Searching;
        }
    }

    void PatrolBehavior()
    {
        if (navAgent.remainingDistance < 0.5f)
        {
            patrolWaitTimer += Time.deltaTime;
            if (patrolWaitTimer >= waitTime)
            {
                StartPatrol();
            }
        }
    }

    void ChaseBehavior()
    {
        if (gamemanager.instance.player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, gamemanager.instance.player.transform.position);

            if (distanceToPlayer <= attackRange)
            {
                currentState = EnemyState.Attacking;
                navAgent.stoppingDistance = originalStoppingDistance;
            }
            else
            {
                navAgent.SetDestination(gamemanager.instance.player.transform.position);
                navAgent.stoppingDistance = attackRange;
            }
        }
    }

    void AttackBehavior()
    {
        if (gamemanager.instance.player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, gamemanager.instance.player.transform.position);

            if (distanceToPlayer > attackRange)
            {
                currentState = EnemyState.Chasing;
                return;
            }

            
            FacePlayer();

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                FireProjectile();
            }
        }
    }

    void SearchBehavior()
    {
       
        patrolWaitTimer += Time.deltaTime;
        if (patrolWaitTimer >= waitTime * 2)
        {
            currentState = EnemyState.Patrolling;
            patrolWaitTimer = 0;
        }
    }

    void StartPatrol()
    {
        patrolWaitTimer = 0;
        navAgent.stoppingDistance = 0;

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += homePosition;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, NavMesh.AllAreas))
        {
            navAgent.SetDestination(navHit.position);
        }
    }

    bool CanSeePlayer()
    {
        if (gamemanager.instance?.player == null) return false;

        directionToPlayer = gamemanager.instance.player.transform.position - visionPoint.position;
        playerAngle = Vector3.Angle(directionToPlayer, transform.forward);

        
        Debug.DrawRay(visionPoint.position, directionToPlayer, Color.red);

        if (playerAngle <= viewAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(visionPoint.position, directionToPlayer.normalized, out hit, attackRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void FacePlayer()
    {
        Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            lastAttackTime = Time.time;
            Instantiate(projectilePrefab, firePoint.position, transform.rotation);

            
            Debug.Log($"{gameObject.name} fired at player!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            navAgent.stoppingDistance = 0;
            currentState = EnemyState.Searching;
            patrolWaitTimer = 0;
        }
    }

    IEnumerator FlashDamageColor()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        enemyRenderer.material.color = originalColor;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        
        if (gamemanager.instance.player != null)
        {
            playerDetected = true;
            currentState = EnemyState.Chasing;
            navAgent.SetDestination(gamemanager.instance.player.transform.position);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashDamageColor());
        }
    }

    void Die()
    {
        
        Debug.Log($"{gameObject.name} has been defeated!");

        
        if (gamemanager.instance != null)
        {
            
        }

        Destroy(gameObject);
    }

    public void Open(GameObject opener)
    {
        throw new System.NotImplementedException();
    }

    public void Close()
    {
        throw new System.NotImplementedException();
    }

    public bool CanOpen(GameObject opener)
    {
        throw new System.NotImplementedException();
    }
}
