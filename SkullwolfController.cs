using UnityEngine;
using System.Collections;

public class SkullwolfController : MonoBehaviour
{
    public int WolfattackDamage = 5;
    public Animator animator;
    public int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;
    public PlayerController playerController;
    public PlayerAttack playerAttack;
    private bool isDead = false;
    private bool isAttacking = false;  // Để kiểm soát trạng thái tấn công
    public float attackCooldown = 1f;  // Thời gian cooldown cho đòn tấn công
    public float attackRange = 1.5f;   // Phạm vi tấn công
    public LayerMask playerLayer;
    public GameObject healthCanvas; // Canvas thanh máu
    public HealthBarWolf healthBarWolf;     // Script thanh máu
    public GameObject itemDropPrefab;


    private void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (healthBar != null)
        {
            healthBarWolf.SetMaxHealth(maxHealth);
        }

        if (healthCanvas != null)
        {
            healthCanvas.SetActive(false);
        }

        if (itemDropPrefab != null)
        {
            itemDropPrefab.SetActive(false); 
        }
    }

    public void TakeDamage(int attackDamage)
    {
        if (isDead) return;

        currentHealth -= attackDamage;

        if (healthCanvas != null && !healthCanvas.activeSelf)
        {
            healthCanvas.SetActive(true); 
        }

        if (healthBar != null)
        {
            healthBarWolf.SetHealth(currentHealth); 
        }

        animator.SetTrigger("isHurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Hiển thị và tách vật phẩm ra khỏi con sói
        if (itemDropPrefab != null)
        {
            itemDropPrefab.transform.parent = null; // Tách vật phẩm khỏi con sói
            itemDropPrefab.SetActive(true); // Hiển thị vật phẩm
        }

        this.enabled = false;
        Destroy(gameObject, 1f); // Xóa con sói sau 1 giây
    }



    private IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.5f);  
        animator.SetTrigger("isHurt");  
    }

    private void Update()
    {
        if (!isDead && !isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        if (hitPlayers.Length > 0)
        {
            isAttacking = true;
            animator.SetTrigger("isAttacking");

            foreach (Collider player in hitPlayers)
            {
                if (player.CompareTag("Player"))
                {
                    isAttacking = true;
                    PlayerAttack playerAttack = player.GetComponentInParent<PlayerAttack>();
                    if (playerAttack != null)
                    {
                        playerController.TakeDamage(5);
                        Debug.Log("Skullwolf gây sát thương cho player");
                    }
                }
            }

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);  
        isAttacking = false;  
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
