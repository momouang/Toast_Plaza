using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("Score")]
    public float totalScore = 0f;
    public TMP_Text score;

    [Header("HealthBar")]
    public Slider slider;

    [Header("Timer")]
    public Timer timer;


    //Updating Time
    public void Update()
    {
        timer.countDown();
    }

    //healthBar Manager
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void setHealth(int health)
    {
        slider.value = health;
    }

    //score Manager
    public void gainScore(float point)   
    {
        totalScore += point;
        updateUI();
    }

    void updateUI()
    {
        score.text = totalScore.ToString();
    }

}
