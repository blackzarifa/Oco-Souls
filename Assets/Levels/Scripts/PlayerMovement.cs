using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    private float cooldownTimer = Mathf.Infinity;

    private Rigidbody2D body;
    private Animator animator;
    private bool grounded;
    private bool attack;


    private void Awake()
    {
        // Interage com o codigo do Unity diretamente
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Mudar pra onde olha conforme move pra Esquerda/Direita
        if (horizontalInput > 0.01f) transform.localScale = new Vector3(10, 10, 10);
        else if (horizontalInput < -0.01f) transform.localScale = new Vector3(-10, 10, 10);

        if (Input.GetKey(KeyCode.Space) && grounded) Jump();
        else if (Input.GetKey(KeyCode.Mouse0) && grounded) Attack();

        // Set Animator params
        animator.SetBool("run", horizontalInput != 0);

        animator.SetBool("grounded", grounded);
    }

    private void Jump ()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        animator.SetTrigger("jump");
        grounded = false;
    }

    private void Attack ()
    {
        cooldownTimer += Time.deltaTime;
         
        if (cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            animator.SetTrigger("attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") grounded = true;
    }

    private bool isGrounded() { return false; }
}
 