using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 255f;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack;

    private float horizontalInput;
    private bool jumpRequest = false; // Biến tạm để lưu yêu cầu nhảy
    private bool attackRequest = false; // Biến tạm để lưu yêu cầu tấn công
    private bool throwRequest = false; // Biến tạm để lưu yêu cầu ném

    [SerializeField] private string currentAnimName;

    void Update()
    {
        // Thu nhận input trong Update
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequest = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            attackRequest = true;
        }

        if (Input.GetKeyDown(KeyCode.V) && isGrounded)
        {
            throwRequest = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = IsCheckGrounded();

        // Nếu đang trong trạng thái attack, không xử lý hành động mới
        if (isAttack)
        {
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            // Xử lý nhảy
            if (jumpRequest)
            {
                Jump();
                jumpRequest = false;
            }

            // Xử lý tấn công
            if (attackRequest)
            {
                Attack();
                attackRequest = false;
            }

            // Xử lý ném
            if (throwRequest)
            {
                Throw();
                throwRequest = false;
            }

            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                ChangeAnim("run");
            }
        }

        // Kiểm tra rơi
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        // Di chuyển
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            ChangeAnim("run");
            rb.velocity = new Vector2(horizontalInput * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(0, horizontalInput < 0 ? 180 : 0, 0);
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    private bool IsCheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void ResetAttack()
    {
        Debug.Log("Reset Attack");
        isAttack = false;
        animator.ResetTrigger(currentAnimName);
        currentAnimName = "idle";
        animator.SetTrigger("idle");
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }
}