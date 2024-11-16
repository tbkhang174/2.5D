using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 100;
    public LayerMask enemyLayer;
    public Animator animator;
    public HealthBar healthBar;
    public BoxCollider attackCollider; // Collider cần bật tắt
    public float attackDelay = 0.65f;
    public float attackDelay02 = 0.30f;
    public float attackDelay03 = 1.02f;
    public int maxHealth = 100;
    private int currentHealth;

    public bool isAttacking = false;

    private void Start()
    {
        attackDamage = 100;
        currentHealth = maxHealth;
        // Đặt collider thành trigger để tránh đẩy nhau ra
        attackCollider.isTrigger = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.K) && !isAttacking)
        {
            Attack02();
        }

        if (Input.GetKeyDown(KeyCode.I) && !isAttacking)
        {
            Attack03();
        }
    }

        void Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        attackCollider.enabled = true;

        StartCoroutine(DisableAttackAreaAfterDelay(attackDelay));
    }

    void Attack02()
    {
        isAttacking = true;
        animator.SetBool("isAttacking02", true);

        attackCollider.enabled = true;

        StartCoroutine(DisableAttackAreaAfterDelay(attackDelay02));
    }

    void Attack03()
    {
        isAttacking = true;
        animator.SetBool("isAttacking03", true);

        attackCollider.enabled = true;

        StartCoroutine(DisableAttackAreaAfterDelay(attackDelay03));
    }

    private IEnumerator DisableAttackAreaAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        attackCollider.enabled = false;
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isAttacking02", false);
        animator.SetBool("isAttacking03", false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            Debug.Log($"Player attackDamage: {attackDamage}");

            Debug.Log("Hit enemy: " + other.name);

            ShadowController shadowController = other.GetComponent<ShadowController>();
            SkullwolfController skullwolfController = other.GetComponent<SkullwolfController>();

            if (shadowController != null)
            {
                Debug.Log("Collided with: " + other.tag);
                shadowController.TakeDamage(attackDamage);
            }

            if (skullwolfController != null)
            {
                Debug.Log("Collided with: " + other.tag);
                skullwolfController.TakeDamage(attackDamage);
            }
        }
    }
}