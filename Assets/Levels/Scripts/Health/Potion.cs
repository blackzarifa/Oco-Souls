using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image img;
    
    void Start()
    {
        img.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K))
        {
            if (playerHealth.healLimit == 1)
            {
                img.enabled = false;
            }
        }
    }
}
