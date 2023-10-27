using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toastScore : MonoBehaviour
{

    //public GameObject Toast_type;

    public int point;
    public GameManager gamemanager;

    public void pickUp()
    {
        GameObject.FindObjectOfType<GameManager>().gainScore(point);
        Destroy(gameObject);
    }

}
