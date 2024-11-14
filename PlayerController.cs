using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDist;
    public float dashSpeed;
    public float dashDuration;
    private float dashTimeLeft;
    private bool isDashing = false;
    private bool isPaused = false;

    public Animator animator;
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    private PlayerAttack playerAttack;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerAttack = gameObject.GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (isPaused)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Kiểm tra nếu đang tấn công thì dừng di chuyển trong 0.8 giây
        if (playerAttack != null && playerAttack.isAttacking)
        {
            StartCoroutine(PauseMovement(0.8f));
        }

        Dash();
        AdjustPosition();

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);

        // Chỉ cập nhật vị trí nếu không đang tấn công
        if (!isPaused)
        {
            Move(moveDir);
        }

        if (x != 0)
        {
            sr.flipX = x < 0;

            // Lật collider theo hướng người chơi
            if (x < 0 && playerAttack.attackCollider.transform.localScale.x > 0)
            {
                FlipAttackCollider();
            }
            else if (x > 0 && playerAttack.attackCollider.transform.localScale.x < 0)
            {
                FlipAttackCollider();
            }
        }
        animator.SetBool("isMoving", moveDir.magnitude > 0);
    }

    private void Move(Vector3 moveDir)
    {
        Vector3 targetPosition = rb.position + moveDir.normalized * speed * Time.deltaTime;

        if (!IsColliding(targetPosition))
        {
            rb.MovePosition(targetPosition);
        }
    }

    private bool IsColliding(Vector3 targetPosition)
    {
        RaycastHit hit;
        return Physics.Raycast(targetPosition + Vector3.up, Vector3.down, out hit, 1f, LayerMask.GetMask("Object"));
    }

    private void FixedUpdate()
    {
        if (isPaused)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (playerAttack != null && playerAttack.isAttacking)
        {
            StartCoroutine(PauseMovement(0.8f));
        }

        Dash();
        AdjustPosition();
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

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.L) && !isDashing)
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            animator.SetBool("isDashing", true);
        }

        if (isDashing)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector3 moveDir = new Vector3(x, 0, y).normalized;

            rb.velocity = moveDir * dashSpeed;

            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                animator.SetBool("isDashing", false);
                rb.velocity = Vector3.zero; // Reset velocity after dashing
            }
        }
        else
        {
            // Set the vertical velocity to maintain gravity but zero out horizontal movement
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private IEnumerator PauseMovement(float pauseDuration)
    {
        isPaused = true;
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(pauseDuration);

        isPaused = false;
    }

    private void FlipAttackCollider()
    {
        Vector3 colliderScale = playerAttack.attackCollider.transform.localScale;
        colliderScale.x *= -1;  // Lật scale của collider theo trục X
        playerAttack.attackCollider.transform.localScale = colliderScale;
    }
}
