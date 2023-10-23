using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField]float remainingTime;

    // Update is called once per frame
    public void countDown()
    {

        if(remainingTime >0)
        {
            remainingTime -= Time.deltaTime;
        }
        
        if(remainingTime <0)
        {
            remainingTime = 0;
            Debug.Log("time's up!");
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 360);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}