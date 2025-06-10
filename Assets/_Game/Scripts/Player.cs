using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 255f;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;


    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack;

    [SerializeField] private bool isControlKey = false;

    private float horizontalInput;


    private int coin = 0;

    private Vector3 savePoint;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    void Update()
    {

        if (isControlKey)
        {

            // Thu nhận input trong Update
            horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }
    }

    void FixedUpdate()
    {
        if (IsDead)
        {
            return; // Nếu đã chết, không xử lý gì thêm
        }

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
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(0, horizontalInput < 0 ? 180 : 0, 0);
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();

        UIManager.instance.SetCoinText(coin);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    public void SetSavePoint()
    {
        savePoint = transform.position;
    }

    protected override void OnDeath()
    {
        //base.OnDeath();

        OnInit();
    }
    private bool IsCheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
        return hit.collider != null;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.75f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.66f);
    }

    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void ResetAttack()
    {
        Debug.Log("Reset Attack");
        ChangeAnim("idle");
        isAttack = false;
    }

    public void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.75f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void SetMove(float horizontal)
    {
        this.horizontalInput = horizontal;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coin"))
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoinText(coin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("DeathZone"))
        {
            Debug.Log("Player has died!");
            // Thêm logic xử lý khi player chết, ví dụ: reset vị trí, giảm mạng, v.v.
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f); // Reset player sau 2 giây
        }
    }
}