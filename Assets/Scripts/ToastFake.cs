using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastFake : MonoBehaviour
{
    public int point;
    public int damagePoint;
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
    public ParticleSystem eatup_rainbow;
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
                GameObject.FindObjectOfType<GameManager>().gainScore(point);
                var toastNOW = Instantiate(toastPrefab[0], gameObject.transform.position, Quaternion.identity);
                var toastNOW1 = Instantiate(toastPrefab[1], gameObject.transform.position, Quaternion.identity);
                var toastNOW2 = Instantiate(toastPrefab[2], gameObject.transform.position, Quaternion.identity);
                toastNOW.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * 150f);
                toastNOW1.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * 150f);
                toastNOW2.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * 150f);
                Instantiate(eatup_rainbow, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            Instantiate(eatup_AI, gameObject.transform.position, Quaternion.identity);
        }
        //Destroy(gameObject);
    }
}
