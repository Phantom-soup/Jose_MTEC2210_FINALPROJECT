using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Image currentHealth;
    public GameObject playerReference;
    public float playerHealth = 20f;

    public Image flightTime;
    public float fTimeRemaining = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        if (playerHealth < 0)
        {
            SceneManager.LoadScene(0);   
        }
    }

    public void AdjustHealth(float difference)
    {
        playerHealth += difference;
        currentHealth.fillAmount = playerHealth / 20f;
        if (playerHealth > 20f)
        {
            playerHealth = 20f;
        }
    }

    public void AdjustFlightTime(float difference)
    {
        fTimeRemaining += difference;
        flightTime.fillAmount = fTimeRemaining / 3f;
        if (fTimeRemaining > 3f)
        {
            fTimeRemaining = 3f;
        }
    }
}
