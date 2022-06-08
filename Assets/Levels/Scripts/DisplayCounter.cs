using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCounter : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject gameOverScreen;

    private GUIStyle guiStyle = new GUIStyle();
    private float maxCounter;
    private float counter;

    void Start()
    {
        if (PlayerPrefs.GetFloat("PreviousCounter") > 0)
        {
            maxCounter = PlayerPrefs.GetFloat("PreviousCounter") - PlayerPrefs.GetFloat("Counter");
        } 
        else
        {
            maxCounter = PlayerPrefs.GetFloat("PreviousCounter");
        }
        
        counter = maxCounter;
    }

    void Update()
    {
        if (playerMovement.isActiveAndEnabled)
        {
            counter -= Time.deltaTime;
        }
        Debug.Log(counter);
    }
    void OnGUI()
    {
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.white;
        
        if (counter > 0 && !gameOverScreen.activeSelf)
        {
            GUI.Label(new Rect(20, 100, 100, 100), "Time left:  " + counter, guiStyle);
        }
    }
}
