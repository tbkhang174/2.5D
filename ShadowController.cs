using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float groundDist;
    private bool isMoving = false;
    private bool isPlayerInRange = false;
    private bool isAttacking = false;

    private SphereCollider detectionCollider;
    public Transform spriteTransform;
    public Animator animator;
    public Rigidbody rb;
    public Transform player;

    public LayerMask terrainLayer;
    private Vector3 originalScale;

    private string[] attackAnimations = { "Attack1", "Attack2" };

    public int health = 100;  // Thêm thuộc tính máu
    public BoxCollider hitbox; // Collider để nhận sát thương

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        Collider[] colliders = GetComponents<Collider>();

        foreach (Collider col in colliders)
        {
            if (col is SphereCollider)
            {
                detectionCollider = (SphereCollider)col;
                detectionCollider.isTrigger = true;
                break;
            }
        }

        originalScale = spriteTransform.localScale;
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            FollowPlayer();
            AdjustPosition();
        }

        if (health <= 0)
        {
            Die(); // Gọi hàm chết khi máu <= 0
        }
    }

    // Xử lý khi máu giảm hết
    private void Die()
    {
        animator.SetTrigger("Die");
        // Logic cho việc chết (có thể xóa đối tượng hoặc dừng hoạt động)
        Destroy(gameObject);  // Xóa đối tượng quái sau khi chết
    }

    // Va chạm với BoxCollider của player
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có tag "Player" không
        if (other.CompareTag("Player"))
        {
            // Tìm component PlayerAttack trên đối tượng "Player"
            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();

            if (playerAttack != null)
            {

                // Xử lý logic khi người chơi vào tầm tấn công
                isPlayerInRange = true;
                StartAttack();
            }
        }
    }

    // Hàm trừ máu
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage; // Trừ số máu khi nhận sát thương
        Debug.Log("Enemy took damage, current health: " + health);
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && detectionCollider != null)
        {
            isPlayerInRange = false;
            isMoving = false;
            isAttacking = false;
            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isAttacking", isAttacking);
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        Vector3 targetPosition = transform.position + direction.normalized * moveSpeed * Time.deltaTime;

        if (!IsColliding(targetPosition))
        {
            rb.MovePosition(targetPosition);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        animator.SetBool("isMoving", isMoving);

        Flip(direction.x);
        AdjustPosition();
    }

    private bool IsColliding(Vector3 targetPosition)
    {
        RaycastHit hit;
        return Physics.Raycast(targetPosition + Vector3.up, Vector3.down, out hit, 1f, LayerMask.GetMask("Object"));
    }

    private void AdjustPosition()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }
    }

    void Flip(float moveDirection)
    {
        if (moveDirection > 0)
        {
            spriteTransform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else if (moveDirection < 0)
        {
            spriteTransform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", isAttacking);

        int randomIndex = Random.Range(0, attackAnimations.Length);
        string selectedAttack = attackAnimations[randomIndex];

        animator.SetTrigger(selectedAttack);

        StartCoroutine(StopAttackAfterAnimation());
    }

    IEnumerator StopAttackAfterAnimation()
    {
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }
}