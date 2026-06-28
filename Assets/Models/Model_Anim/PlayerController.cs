using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Enemy Facing")]
    public float autoFaceEnemyDistance = 6f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip enemyHitSound;
    public float hitSoundDuration = 0.3f;
    private bool hitSoundPlayedThisAttack = false;
    private Coroutine hitSoundCoroutine;

    [Header("Effects")]
    public GameObject bloodEffectPrefab;

    [Header("Health")]
    public int maxHealth = 10;
    private int currentHealth;
    private bool isDead = false;
    public Slider healthBar;

    [Header("Movement")]
    public float walkSpeed = 4f;
    public float rotationSpeed = 10f;
    public float gravity = -20f;

    [Header("Mobile Controls")]
    public Joystick joystick;

    private CharacterController controller;
    private Animator play;
    private Vector3 verticalVelocity;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Combat")]
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask enemyLayers;

    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        controller = GetComponent<CharacterController>();
        play = GetComponent<Animator>();

        if (controller == null)
            Debug.LogError("CharacterController missing on Player.");

        if (play == null)
            Debug.LogError("Animator missing on Player.");

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (isDead) return;

        MovePlayer();

        // Keyboard attack
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Attack();
        }
    }

    void MovePlayer()
    {
        if (controller == null || play == null) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Joystick only overrides keyboard when joystick is actually moved
        if (joystick != null &&
            (Mathf.Abs(joystick.Horizontal) > 0.1f || Mathf.Abs(joystick.Vertical) > 0.1f))
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (!isAttacking)
        {
            if (inputDirection.magnitude > 0.1f)
            {
                Vector3 moveDirection;

                if (cameraTransform != null)
                {
                    Vector3 cameraForward = cameraTransform.forward;
                    Vector3 cameraRight = cameraTransform.right;

                    cameraForward.y = 0f;
                    cameraRight.y = 0f;

                    cameraForward.Normalize();
                    cameraRight.Normalize();

                    moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
                }
                else
                {
                    moveDirection = inputDirection;
                }

                moveDirection.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

                controller.Move(moveDirection * walkSpeed * Time.deltaTime);

                play.SetFloat("Speed", 1f);
            }
            else
            {
                play.SetFloat("Speed", 0f);
            }
        }
        else
        {
            play.SetFloat("Speed", 0f);
        }

        // Gravity
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    public void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        hitSoundPlayedThisAttack = false;

        if (play != null)
        {
            play.SetTrigger("Attack");
        }

        if (attackPoint == null)
        {
            Debug.LogWarning("Attack point not assigned.");
            StartCoroutine(ResetAttack());
            return;
        }

        // Face nearest enemy only when attacking
        Transform nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            Vector3 direction = nearestEnemy.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // 1. Raycast only for requirement/demo
        RaycastHit rayHit;

        if (Physics.Raycast(attackPoint.position, transform.forward, out rayHit, attackRange, enemyLayers))
        {
            Debug.Log("Raycast hit just for show: " + rayHit.collider.name);
        }
        else
        {
            Debug.Log("Raycast missed, but OverlapSphere will still check.");
        }

        // 2. OverlapSphere for actual gameplay hit
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        bool damagedEnemy = false;

        foreach (Collider col in hitColliders)
        {
            EnemyAI enemy = col.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                enemy.TakeDamage(1);

                // Hit sound first
                PlayHitSound();

                // Blood particle after a tiny delay
                StartCoroutine(SpawnBloodAfterDelay(col));

                Debug.Log("OverlapSphere hit enemy: " + enemy.name);

                damagedEnemy = true;
                break;
            }
        }

        if (!damagedEnemy)
        {
            Debug.Log("OverlapSphere found no enemy in range.");
        }

        StartCoroutine(ResetAttack());
    }
    IEnumerator SpawnBloodAfterDelay(Collider enemyCollider)
    {
        yield return new WaitForSeconds(0.08f);
        SpawnBloodEffect(enemyCollider);
    }
    void PlayHitSound()
    {
        if (hitSoundPlayedThisAttack) return;

        if (audioSource != null && enemyHitSound != null)
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.clip = enemyHitSound;
            audioSource.Play();

            if (hitSoundCoroutine != null)
            {
                StopCoroutine(hitSoundCoroutine);
            }

            hitSoundCoroutine = StartCoroutine(StopHitSoundAfterDelay());

            hitSoundPlayedThisAttack = true;
        }
    }

    IEnumerator StopHitSoundAfterDelay()
    {
        yield return new WaitForSeconds(hitSoundDuration);

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void SpawnBloodEffect(Collider enemyCollider)
    {
        if (bloodEffectPrefab == null) return;
        if (attackPoint == null) return;

        Vector3 bloodPosition = enemyCollider.ClosestPoint(attackPoint.position);
        bloodPosition.y += 0.8f;

        Quaternion bloodRotation = Quaternion.LookRotation(transform.forward);

        GameObject blood = Instantiate(bloodEffectPrefab, bloodPosition, bloodRotation);

        ParticleSystem ps = blood.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();
        }

        Destroy(blood, 1.5f);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player died");

        if (play != null)
        {
            play.SetTrigger("Die");
        }

        if (controller != null)
        {
            controller.enabled = false;
        }

        Invoke(nameof(LoadLoseScene), 1f);
    }

    void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScene");
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearest = null;
        float minDist = autoFaceEnemyDistance;
        Vector3 currentPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(currentPos, enemy.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + transform.forward * attackRange);
    }
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log("Player healed. Health: " + currentHealth);
    }
}