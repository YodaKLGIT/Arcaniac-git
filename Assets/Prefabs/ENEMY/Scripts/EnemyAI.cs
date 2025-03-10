using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing, Attacking }
    private EnemyState currentState;

    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float detectionRange = 15f;
    public float attackRange = 7f;
    public float fireRate = 1.5f;
    public float projectileSpeed = 20f;
    private float nextFireTime;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    private NavMeshAgent agent;

    // Health System
    public float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private EnemyHealthbar _healthBar;
    [SerializeField] private EnemyHealthbar _healthBarEffect;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;

        // Initialize health
        currentHealth = maxHealth;
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);

        // Start at a random patrol point
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Chasing:
                Chase();
                if (distanceToPlayer <= attackRange)
                    currentState = EnemyState.Attacking;
                else if (distanceToPlayer > detectionRange)
                    currentState = EnemyState.Patrolling;
                break;

            case EnemyState.Attacking:
                Attack();
                if (distanceToPlayer > attackRange)
                    currentState = EnemyState.Chasing;
                break;
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            MoveToNextPatrolPoint();
        }
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        agent.SetDestination(player.position);

        // Rotate toward player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        if (Vector3.Distance(transform.position, player.position) <= attackRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Attack()
    {
        agent.ResetPath();

        // Look at player
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Ignore self-collision
        Collider enemyCollider = GetComponentInChildren<Collider>();
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), enemyCollider);

        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        Destroy(projectile, 5f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
        _healthBarEffect.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play death animation or effects here
        Destroy(gameObject); // Destroy enemy
    }
}
