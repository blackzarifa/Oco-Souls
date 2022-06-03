using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, ICollisionHandler
{
    [Header("Player Options")]
    public float speed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attack Params")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;

    [Header("Sound Effects")]
    [SerializeField] AudioSource footstep;
    [SerializeField] AudioSource slash;

    [Header("Idle Punish")]
    [SerializeField] RectTransform idleCanvas1;
    [SerializeField] RectTransform idleCanvas2;
    [SerializeField] RectTransform idleCanvasRed;

    public static Vector2 lastCheckPointPos = new Vector2(-7.517738f, -88.60466f);  // Correct spawn
    //public static Vector2 lastCheckPointPos = new Vector2(257f, -127f);           // Test spawn

    private float cooldownTimer = Mathf.Infinity;
    private float idleCounter = 0.0f;
    private int idleCooldown = 10;

    private Rigidbody2D body;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    private Health health;
    
    private void Awake()
    {
        // Interage com o codigo do Unity diretamente
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        health = GetComponent<Health>();

        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckPointPos;
    }

    private void Update()
    {
        HorizontalMove();
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !health.boss.activeSelf) {
            idleCounter += Time.deltaTime;
        } else
        {
            idleCounter = 0.0f;
            idleCooldown = 10;
        }
        //Debug.Log(idleCounter);

        if (idleCounter >= idleCooldown) {
            IdleDamageBlink();
            health.TakeDamage(2);
            idleCooldown += 5;
        }
        
        if ((idleCanvas1.gameObject.activeSelf == false) && (idleCounter >= 7)) {
            IdleAnimation();
        }

        if ((idleCanvas1.gameObject.activeSelf == true) && (idleCounter <= 1))
        {
            RemoveIdleCanvas();
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isGrounded()) Jump();
        else if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J)) && isGrounded()) Attack();

        anim.SetBool("grounded", isGrounded());
    }

    private void HorizontalMove ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Mudar pra onde olha conforme move pra Esquerda/Direita
        if (horizontalInput > 0.01f) transform.localScale = new Vector3(10, 10, 10);
        else if (horizontalInput < -0.01f) transform.localScale = new Vector3(-10, 10, 10);

        anim.SetBool("run", horizontalInput != 0);

    }

    private void Jump ()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("jump");
    }

    private void Attack ()
    {
        cooldownTimer += Time.deltaTime;
         
        if (cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            anim.SetTrigger("attack");
            
            /*Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    enemy.GetComponent<Health>().TakeDamage(damage);
                }
            }*/
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center,
            capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private void Footsteps()
    {
        footstep.Play();
    }

    private void Slash()
    {
        slash.Play();
    }

    private void IdleAnimation() {
        idleCanvas1.gameObject.SetActive(true);
        LeanTween.alpha(idleCanvas1, 0, 0);
        idleCanvas2.gameObject.SetActive(true);
        LeanTween.alpha(idleCanvas2, 0, 0);

        LeanTween.alpha(idleCanvas1, 0.4f, 2f);
        LeanTween.alpha(idleCanvas2, 0.7f, 2f);
    }

    private void RemoveIdleCanvas() {
        idleCanvas1.gameObject.SetActive(false);
        idleCanvas2.gameObject.SetActive(false);
    }

    private void IdleDamageBlink() {
        idleCanvasRed.gameObject.SetActive(true);
        LeanTween.alpha(idleCanvasRed, 0, 0);
        LeanTween.alpha(idleCanvasRed, 0.7f, 0.15f).setOnComplete(() => { idleCanvasRed.gameObject.SetActive(false); });
    }
    
    public void CollisionEnter(string colliderName, GameObject other)
    {
        if (colliderName == "KnifeHitBox" && other.tag == "Enemy")
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
 