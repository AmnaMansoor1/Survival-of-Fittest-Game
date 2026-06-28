using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int health = 2;
    public float attackDistance = 1.8f;
    public float attackCooldown = 1.5f;
    public int damageToPlayer = 1;

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    [HideInInspector] public EnemySpawner spawner;

    private float nextAttackTime = 0f;
    private bool isDead = false;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            if (animator != null)
                animator.SetFloat("Speed", 1f);
        }
        else
        {
            agent.isStopped = true;

            if (animator != null)
                animator.SetFloat("Speed", 0f);

            FacePlayer();
            AttackPlayer();
        }
    }

    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 8f * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        if (animator != null)
            animator.SetTrigger("Attack");

        // Damage the player
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.TakeDamage(damageToPlayer);

        Debug.Log("Enemy attacked player");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Enemy took damage. Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("Enemy died");

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Die");
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
        if (spawner != null)
        {
            spawner.EnemyDied();
        }

        Destroy(gameObject, 3f);
    }
}