using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameObject;
    [SerializeField] private Animator animator;

    [Header("Health")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private HealtBar healthBar;
    [SerializeField] private int maxHealth = 100;

    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private float backSensorRange;
    [SerializeField] private float rangeOfset;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("Patroling")]
    [SerializeField] private EnemyPatrol enemyPatrol;

    [Header("PlayerScore")]
    [SerializeField] private Transform player;

    private int currentHealth;
    private float cooldownTimer = Mathf.Infinity;
    private PlayerCombat playerCombat;

    void Start()
    {
        cooldownTimer = 0;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerCombat = player.GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInRange();
        }


        if (currentHealth == maxHealth)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }

        cooldownTimer += Time.deltaTime;

        // Attack only when player in range
        if (cooldownTimer >= attackCooldown)
        {
            if (PlayerInRange())
            {
                cooldownTimer = 0;
                animator.SetTrigger("Attack");
            }
        }


    }

    private void DamagePlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center - transform.right * transform.localScale.x,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, Vector2.left, playerLayer);

        if (hit.collider != null && hit.collider.tag == "Player")
        {
            hit.collider.GetComponent<PlayerCombat>().TakeDamage(damage);
        }
    }

    private bool PlayerInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center - transform.right / 2 * transform.localScale.x * rangeOfset,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, Vector2.left, playerLayer);

        if (hit.collider != null && hit.collider.tag == "Player")
            return true;

        return false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Hurt");

        if (IsPlayerBehind())
        {
            Debug.Log(IsPlayerBehind());
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (enemyPatrol != null)
                enemyPatrol.enabled = false;
        }
        else
            if (enemyPatrol != null)
            enemyPatrol.enabled = true;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private bool IsPlayerBehind()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * transform.localScale.x * rangeOfset,
            new Vector3(boxCollider.bounds.size.x * backSensorRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0f, Vector2.left, playerLayer);

        if (hit.collider != null && hit.collider.tag == "Player")
            return true;
        else
            return false;
    }

    private void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        if (this.tag == "LightEnemy")
            playerCombat.score += 50;
        else if (this.tag == "HeavyEnemy")
            playerCombat.score += 100;

        playerCombat.UpdateScore();

        canvas.enabled = false;
        gameObject.layer = 10;
        if (enemyPatrol != null)
            enemyPatrol.enabled = false;
        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right / 2 * transform.localScale.x * rangeOfset,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * rangeOfset,
            new Vector3(boxCollider.bounds.size.x * backSensorRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
