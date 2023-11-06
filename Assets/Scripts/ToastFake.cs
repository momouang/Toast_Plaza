using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastFake : MonoBehaviour
{
    public int point;
    public int damagePoint;
    //public GameManager gamemanager;
    public player player;
    public AIscript Aiscript;
    public GameObject[] toastPrefab;

    [Header("When Hitting Player")]
    public float damageTime = 0.5f;
    float currentTime;
    public bool isHurting;

    [Header("Eating Toast")]
    public float eatNowTime = 3f;
    public int pickupCount = 0;
    public float eatingAmount = 0f;
    Vector3 changingScale;

    [Header("Particles")]
    public ParticleSystem eatup_player;
    public ParticleSystem eatup_AI;
    public ParticleSystem hitBlast;


    public void Update()
    {

        // when the toast can take damage to player
        isHurting = true;
        currentTime += Time.deltaTime;

        if (currentTime >= damageTime)
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
            ContactPoint contact = other.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(hitBlast, pos, rot);
            other.gameObject.GetComponent<player>().TakeDamage(damagePoint);
        }
    }


    // when player picks up the toast
    public void pickUp2(bool isPlayer)
    {
        if (isPlayer)
        {
            pickupCount += 1;
            changingScale = gameObject.transform.localScale += new Vector3(1f, 1f, 1f);
            Debug.Log(changingScale);

            if (pickupCount >= eatingAmount)
            {
                var toastNOW = Instantiate(toastPrefab[2], gameObject.transform.position, Quaternion.identity);

                toastNOW.GetComponent<Rigidbody>().AddForce(gameObject.transform.position * 150f);
                Instantiate(eatup_player, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
                //Debug.Log("fake eating");
            }
        }
        else
        {
            Instantiate(eatup_AI, gameObject.transform.position, Quaternion.identity);
        }
        //Destroy(gameObject);
    }
}
