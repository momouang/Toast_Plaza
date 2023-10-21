using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public float totalScore = 0f;
    public TMP_Text score;


    // Update is called once per frame
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
