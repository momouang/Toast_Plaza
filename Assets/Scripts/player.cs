using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    [Header("Movement")]
    Animator playerAnimation;
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [Header("Jumping")]
    Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    bool isGrounded;
    public int jumpCount = 5;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public GameManager gamemanager;

    [Header("Poop")]
    public GameObject[] poop;
    public GameObject[] poopImg;
    public Transform assHole;
    float distance = 10f;
    int randomNumber;

    public ParticleSystem eatupParticle;


    public void Start()
    {
        playerAnimation = GetComponentInChildren<Animator>();  
        currentHealth = maxHealth;
        gamemanager.setMaxHealth(maxHealth);
    }


    // Update is called once per frame
    public void Update()
    {
        //movement
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            jumpCount = 5;
            velocity.y = -2f;
            playerAnimation.SetBool("isFlying",false);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving)
        {
            playerAnimation.SetBool("isWalking", true);
        }
        else
        {
            playerAnimation.SetBool("isWalking", false);
        }
        


        //camera
        Cursor.lockState = CursorLockMode.Locked;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        // jumping
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount >0)
        {
            playerAnimation.SetBool("isFlying", true);
            jumpCount -= 1;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }



        //eating Toast
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimation.SetBool("isEating", true);
            Collider[] scannedObjects = Physics.OverlapSphere(transform.position, 1.5f);
            if(scannedObjects.Length != 0)
            {
                foreach(Collider g in scannedObjects)
                {
                    if(g.gameObject.CompareTag("Toast"))
                    { 
                        g.gameObject.GetComponent<toastScore>().pickUp();
                        Instantiate(eatupParticle, g.gameObject.transform);

                    }
                }
            }
        }
        else
        {
            playerAnimation.SetBool("isEating",false);
        }



        //healthBar
        if(currentHealth <= 0)
        {
            currentHealth = 0;
        }

        pooping();
        randomNumber = Random.Range(0,3);

    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        gamemanager.setHealth(currentHealth);

    }


    //Poop

    private void pooping()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RaycastHit hit;

            Instantiate(poop[0], assHole.transform.position,Quaternion.identity);
            if(Physics.Raycast(assHole.transform.position,assHole.transform.forward,out hit,distance))
            {
                //Debug.Log("poop");
                GameObject newPoop = Instantiate(poopImg[randomNumber], hit.point + new Vector3(0,-0.09f,0), Quaternion.FromToRotation(Vector3.forward,hit.normal));
                newPoop.transform.parent = hit.transform;
                
            }
        }
    }


}
