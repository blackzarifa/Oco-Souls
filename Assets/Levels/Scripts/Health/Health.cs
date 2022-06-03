using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] AudioSource hitEffect;
    [SerializeField] AudioSource potionEffect;

    [Header("Player and Boss Only")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] RectTransform fader;
    public GameObject boss;

    public float currentHealth { get; private set; }
    public int healLimit { get; private set; }

    private Animator animator;
    private bool dead;
    private AudioSource bossMusic;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        if (GetComponent<PlayerMovement>() != null)
        {
            bossMusic = boss.GetComponent<AudioSource>();
        }
    }

    public void TakeDamage(float _damage)
    {
        hitEffect.Play();

        if (GetComponent<Boss>() != null)
        {
            if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Taunt")))
            {
                currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            }
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        }

        if (currentHealth > 0)
        {
            
            //iframes
        }
        else if (!dead)
        {
            animator.SetTrigger("die");

            if (GetComponent<PlayerMovement>() != null)
            {
                GetComponent<PlayerMovement>().enabled = false;
                

                fader.gameObject.SetActive(true);

                LeanTween.alpha(fader, 0, 0);
                LeanTween.alpha(fader, 1, 1.5f).setOnComplete(() =>
                {
                    fader.gameObject.SetActive(false);

                    gameObject.transform.position = new Vector2(-19f, -72f);
                    bossMusic.Stop();

                    gameOverPanel.SetActive(true);
                });

                currentHealth = 10;
            }
            if (GetComponentInParent<EnemyBehaviour>() != null)
            {
                GetComponentInParent<EnemyBehaviour>().enabled = false;
                GetComponentInChildren<HotZoneCheck>().enabled = false;
            }
            if (GetComponent<Boss>() != null)
            {
                GetComponent<Boss>().enabled = false;

                fader.gameObject.SetActive(true);
                LeanTween.alpha(fader, 0, 0);
                LeanTween.alpha(fader, 1, 8f).setOnComplete(() =>
                {
                    fader.gameObject.SetActive(false);
                    gameOverPanel.SetActive(true);
                });
            }
            dead = true;
        }
    }

    public void PlusHealth()
    {
        currentHealth += 4;
    }
    private void Start()
    {
        healLimit = 0;
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K)) && healLimit < 1 && GetComponent<PlayerMovement>() != null)
        {
            if (currentHealth < 7)
            {
                potionEffect.Play();
                PlusHealth();
                healLimit++;
            } 
        }
    }
}
