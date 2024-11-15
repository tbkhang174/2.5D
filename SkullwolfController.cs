using UnityEngine;
using System.Collections;

public class SkullwolfController : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    private int currentHealth;

    private bool isDead = false;
    private bool isAttacking = false;  // Để kiểm soát trạng thái tấn công
    public float attackCooldown = 1f;  // Thời gian cooldown cho đòn tấn công
    public float attackRange = 1.5f;   // Phạm vi tấn công
    public LayerMask playerLayer;

    private void Start()
    {
        currentHealth = maxHealth;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("isHurt");

        StartCoroutine(ResetHurtAnimation());  // Đảm bảo reset "isHurt" sau khi animation hoàn thành

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");

        // Kiểm tra và vô hiệu hóa Collider nếu có
        Collider collider = GetComponent<Collider>();  // Lấy Collider thông thường
        if (collider != null)
        {
            collider.enabled = false;  // Vô hiệu hóa Collider khi chết
        }

        // Vô hiệu hóa script sau khi chết
        this.enabled = false;

        // Xóa đối tượng Skullwolf sau một khoảng thời gian để animation death hoàn thành
        Destroy(gameObject, 1f);  // 1f là thời gian trì hoãn, có thể thay đổi tuỳ thuộc vào độ dài animation
    }

    private IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.5f);  // Điều chỉnh thời gian này với độ dài animation "hurt"
        animator.SetTrigger("isHurt");  // Reset lại trigger nếu cần
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
        // Kiểm tra xem có đối tượng player nào trong phạm vi tấn công không
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        if (hitPlayers.Length > 0)
        {
            isAttacking = true;
            animator.SetTrigger("isAttacking");  // Kích hoạt animation tấn công

            // Gây sát thương cho tất cả các đối tượng trong phạm vi tấn công
            foreach (Collider player in hitPlayers)
            {
                if (player.CompareTag("Player"))
                {
                    PlayerAttack playerAttack = player.GetComponentInParent<PlayerAttack>();
                    if (playerAttack != null)
                    {
                        //playerAttack.TakeDamage(50);  // Tạo sát thương cho player, có thể tùy chỉnh
                        Debug.Log("Skullwolf gây sát thương cho player");
                    }
                }
            }

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);  // Chờ thời gian cooldown
        isAttacking = false;  // Tắt trạng thái tấn công sau cooldown
    }

    private void OnDrawGizmosSelected()
    {
        // Hiển thị phạm vi tấn công trong Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
