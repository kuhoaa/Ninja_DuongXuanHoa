using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpForce = 100;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;


    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private float horizontal;
    private float vertical;
    private int coin = 0;
    private Vector3 savePoint;

    protected override void Start()
    {
        base.Start();
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    protected override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
        UIManager.ins.SetCoin(coin);

    }
    protected override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded();
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        if (IsDead)
        {
            return;
        }

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();    
            }
            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");

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
        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            isJumping = false;
            ChangeAnim("fall");
        }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.deltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0,horizontal > 0 ? 0 :180,0));
        }
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down *1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    public void Attack()
    {
        isAttack = true;
        ChangeAnim("attack");
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);

    }
    private void ResetAttack()
    {
        ChangeAnim("ilde");
        isAttack = false;
    }
    public void Throw()
    {

        isAttack = true;
        ChangeAnim("throw");
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
    
    internal void SavePoint()
    {
        savePoint = transform.position;
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
        this.horizontal = horizontal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.ins.SetCoin(coin);
            Destroy(collision.gameObject);
            coin++;
        }
        if(collision.tag == "DeathZone")
        {
            ChangeAnim("die");
            Invoke(nameof(OnInit), 0.5f);
        }
    }
}
