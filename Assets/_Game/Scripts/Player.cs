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

    [SerializeField] private string currentAnimName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = IsCheckGrounded();
        horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 ==> 1
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
            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                ChangeAnim("run");
            }

            //Attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
                Attack();
            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
                Throw();
        }

        //Check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        //Moving
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            ChangeAnim("run");
            rb.velocity = new Vector2(horizontalInput * Time.fixedDeltaTime * speed, rb.velocity.y);
            // transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1); 
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
        // Check if the player is grounded
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);

        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.45f);
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
        Invoke(nameof(ResetAttack), 0.45f);
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
