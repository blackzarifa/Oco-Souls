using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCounter : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreenBoss;

    public float counter = 0;
    private bool resetCounters;
    private PlayerMovement playerMovement;

    void Awake()
    {
        resetCounters = true;
        counter = PlayerPrefs.GetFloat("Counter");
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerMovement.isActiveAndEnabled && !gameOverScreenBoss.activeSelf)
        {
            counter += Time.deltaTime;
            PlayerPrefs.SetFloat("Counter", counter);
        }

        if (gameOverScreenBoss.activeSelf && resetCounters)
        {
            PlayerPrefs.SetFloat("PreviousCounter", counter);
            PlayerPrefs.SetFloat("Counter", 0f);
            PlayerMovement.lastCheckPointPos = new Vector2(-7.517738f, -88.60466f);
            resetCounters = false;
        }
    }
}
