using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Health")]
    [SerializeField] private HealtBar healthBar;
    [SerializeField] private int maxHealth = 100;

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackRate = 3f;

    [Header("PlayerMovement")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform player;

    private float nextAttackTime = 0f;
    private int currentHealth;
    private int noOfClicks = 0;
    private float lastClickedTime = 0f;
    private float maxComboDelay = 0.9f;
    private bool isBlocking = false;
    public int score;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        score = 0;
    }

    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                lastClickedTime = Time.time;
                noOfClicks++;
                noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                if (noOfClicks == 3)
                    noOfClicks = 0;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                playerMovement.enabled = false;
                playerMovement.body.velocity = Vector2.zero;
                Block();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                playerMovement.enabled = true;
                isBlocking = false;
                animator.SetBool("IdleBlock", false);
            }
        }
    }

    private void Attack()
    {
        // Play attack animation
        switch (noOfClicks)
        {
            case 1:
                animator.SetTrigger("Attack1");
                break;
            case 2:
                animator.SetTrigger("Attack2");
                break;
            case 3:
                animator.SetTrigger("Attack3");
                break;
            default:
                break;
        }


        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("damage!");

            if (enemy.GetComponent<Enemy>())
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void Block()
    {
        isBlocking = true;
        animator.SetTrigger("Block");
        animator.SetBool("IdleBlock", true);
    }

    public void TakeDamage(int damage)
    {
        if (playerMovement.state == PlayerMovement.State.Normal)
        {

            if (isBlocking)
            {
            }
            else if (!isBlocking || currentHealth <= 0)
            {
                currentHealth -= damage;
                healthBar.SetHealth(currentHealth);

                animator.SetTrigger("Hurt");

                if (currentHealth <= 0)
                {
                    Die();
                }
            }

        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        animator.SetBool("IsDead", true);
        gameObject.layer = 10;

        canvas.GetComponent<PauseMenu>().GameOver();
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Potion" && currentHealth != maxHealth)
        {
            currentHealth += 30;
            healthBar.SetHealth(currentHealth);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Chest")
        {
           Chest chest = collision.gameObject.GetComponent<Chest>();        

            if (chest.isActive)
            {
                chest.Open();
                score += chest.score;
                UpdateScore();
            }
        }

        if (collision.gameObject.tag == "Spike")
        {
            TakeDamage(10);
            if (player.localScale.x == 1)
            {
                body.velocity = new Vector2(-15, 3);
            }
            else
            {
                body.velocity = new Vector2(15, 3);
            }
        }

        if (collision.gameObject.tag == "Finish") {
            canvas.GetComponent<PauseMenu>().Finished();
        }
    }

    public void UpdateScore()
    {
        scoreText.text = ("Score: " + score);
    }
}
