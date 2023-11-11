using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("GameScene")]
    public AudioManager audioManager;
    public Player player;
    public GameObject Overlay;
    public TMP_Text totalText;
    public TMP_Text poopTotal;
    public TMP_Text hitTotal;
    public bool isPlaying;
    public bool gameOver;
    public bool ischangingScene = false;
    public float restartDelay = 2f;

    [Header("Score")]
    public float totalScore = 0f;
    public TMP_Text score;
    public bool scoreUP;

    [Header("HealthBar")]
    public Slider slider;

    [Header("Timer")]
    public Timer timer;

    private void Start()
    {
        //audioManager.Play("BGM(birds)");
        isPlaying = true;
        Overlay.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Updating Time
    public void Update()
    {
        timer.countDown();
    }


    //GameOver
    public void endGame()
    {
        if(gameOver == false)
        {
            audioManager.Play("TimeUp");
            gameOver = true;
            isPlaying = false;
            Cursor.lockState = CursorLockMode.Confined;
            Overlay.SetActive(true);
            score.gameObject.SetActive(false);
            totalText.text = totalScore.ToString();
            poopTotal.text = "Pooped\n" + player.poopCount + " times";
            hitTotal.text = "Hitted\n" + player.hitCount + " times";
        }

    }
    public void Restart()
    {
        SceneManager.LoadScene("momo");
        ischangingScene = true;

    }
    public void Quit()
    {
        Application.Quit();
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
        scoreUP = true;
        updateUI();
    }

    public void loseScore(float point)
    {
        totalScore -= point;
        //scoreUP = true;
        updateUI();
    }



    void updateUI()
    {
        if (totalScore <= 0)
        {
            totalScore = 0;
        }
        score.text = totalScore.ToString();

    }

}
