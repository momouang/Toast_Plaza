using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toastScore : MonoBehaviour
{

    public int point;
    public int damagePoint;
    public GameManager gamemanager;
    public player player;
    public AIscript Aiscript;

    public float damageTime = 0.5f;
    float currentTime;
    public bool isHurting;

    public float eatNowTime = 3f;
    public int pickupCount = 0;
    public float eatingAmount = 0f;
    Vector3 changingScale;

    public ParticleSystem eatupParticle;

    private void Start()
    {
        //isEating = false;
    }

    public void Update()
    {

        // when the toast can take damage to player
        isHurting = true;
        currentTime += Time.deltaTime;

        if(currentTime >= damageTime)
        {
            currentTime = damageTime;
            isHurting = false;
        }

    }

    //hitting the player and take damage
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && isHurting)
        {
            other.gameObject.GetComponent<player>().TakeDamage(damagePoint);
            Debug.Log(damagePoint);
        }
    }


    // when player picks up the toast
    public void pickUp()
    {
        pickupCount += 1;
        changingScale = gameObject.transform.localScale -= new Vector3(0.2f,0.2f,0.2f);
        Debug.Log(changingScale);

        if (pickupCount >= eatingAmount)
        {
            GameObject.FindObjectOfType<GameManager>().gainScore(point);

            Destroy(gameObject);
            
        }
    }

}
