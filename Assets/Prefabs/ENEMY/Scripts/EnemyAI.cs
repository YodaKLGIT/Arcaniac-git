using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
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

    public float maxHealth = 100f;

    public float currentHealth;
    [SerializeField] private EnemyHealthbar _healthBar;
    [SerializeField] private EnemyHealthbar _healthBarEffect;

    private bool isFrozen = false;
    private float freezeTimer = 0f;
    private float freezeDuration = 0f;

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;

        currentHealth = maxHealth;
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (isDead) return;

        if (isFrozen)
        {
            freezeTimer += Time.deltaTime;
            if (freezeTimer >= freezeDuration)
            {
                Unfreeze();
            }
            return;
        }

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
        FaceTarget(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Attack()
    {
        agent.ResetPath();
        FaceTarget(player.position);

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Ignore collision with self
        Collider enemyCollider = GetComponentInChildren<Collider>();
        if (projectile.TryGetComponent(out Collider projCollider))
        {
            Physics.IgnoreCollision(projCollider, enemyCollider);
        }

        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        Destroy(projectile, 5f);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

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
        isDead = true;
        agent.isStopped = true;
        // Optional: Play animation or effects
        Destroy(gameObject);
    }

    public void Freeze(float duration)
    {
        if (isFrozen || isDead) return;

        isFrozen = true;
        freezeDuration = duration;
        freezeTimer = 0f;

        if (agent.isOnNavMesh && agent.enabled)
        {
            agent.isStopped = true;
        }
    }

    public void Unfreeze()
    {
        isFrozen = false;
        if (agent.isOnNavMesh && agent.enabled)
        {
            agent.isStopped = false;
        }
    }
}
