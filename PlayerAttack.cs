﻿using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage;
    public LayerMask enemyLayer;
    public Animator animator;
    public BoxCollider attackCollider; // Collider cần bật tắt
    public float attackDelay = 0.65f;
    public float attackDelay02 = 0.30f;
    public float attackDelay03 = 1.02f;

    public bool isAttacking = false;

    private void Start()
    {
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
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy: " + other.name);

            ShadowController shadowController = other.GetComponent<ShadowController>();

            if (shadowController != null)
            {
                Debug.Log("Collided with: " + other.tag);
                shadowController.TakeDamage(attackDamage);
            }
        }
    }
}