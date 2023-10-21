using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toastScore : MonoBehaviour
{

    public float point = 10f;

    public void pickUp()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().gainScore(point);
        Destroy(gameObject);
    }
}
