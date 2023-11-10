using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    [Header("Movement")]
    public Animator playerAnimation;
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
    public int hitCount;
    public int maxHealth = 100;
    public int currentHealth;
    public GameManager gamemanager;

    [Header("Poop")]
    public GameObject[] poop;
    public GameObject[] poopImg;
    public Transform assHole;
    float distance = 10f;
    int randomNumber;
    public int poopCount;

    [Header("Snapping")]
    public GameObject snappingPoint;
    public bool isSnapping;
    public Vector3 offset;

    public bool isfleeing;



    public void Start()
    {
        playerAnimation = GetComponentInChildren<Animator>();  
        currentHealth = maxHealth;
        gamemanager.setMaxHealth(maxHealth);

        isSnapping = false;
    }


    // Update is called once per frame
    public void Update()
    {
        Vector3 direction = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = horizontal != 0 || vertical != 0;

       

        //movement
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(gamemanager.isPlaying)
        {
            if (isGrounded && velocity.y < 0)
            {
                jumpCount = 5;
                velocity.y = -2f;
                playerAnimation.SetBool("isFlying", false);
            }

            
            direction = new Vector3(horizontal, 0f, vertical).normalized;

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (isMoving)
            {
                playerAnimation.SetBool("isWalking", true);
            }
            else
            {
                playerAnimation.SetBool("isWalking", false);
            }
        }
        else
        {
            playerAnimation.SetBool("isWalking", false);
        }
        


        //camera
        
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
            playerAnimation.SetTrigger("isEating");
            Collider[] scannedObjects = Physics.OverlapSphere(transform.position, 1.5f);
            if(scannedObjects.Length != 0)
            {
                foreach(Collider g in scannedObjects)
                {
                    if(g.gameObject.CompareTag("Toast"))
                    { 
                        g.gameObject.GetComponent<toastScore>().pickUp(true);
                        

                    }
                    else if(g.gameObject.CompareTag("Toast Fake"))
                    {
                        g.gameObject.GetComponent<ToastFake>().pickUp2(true);
                    }
                }
            }
        }



        //healthBar
        if(currentHealth <= 0)
        {
            currentHealth = 0;
        }

        pooping();
        randomNumber = Random.Range(0,3);


        if(isSnapping)
        {
            transform.position = snappingPoint.transform.position + new Vector3(0,0.7f,0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSnapping = false;
            }
        }

    }


    //take damage
    public void TakeDamage(int damage)
    {
        hitCount += 1;
        currentHealth -= damage;
        gamemanager.setHealth(currentHealth);

    }


    //Poop
    private void pooping()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            poopCount += 1;
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


    //snapping
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Granny")
        {
            isSnapping = true;
            snappingPoint = other.gameObject;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AI" && Input.GetKeyDown(KeyCode.C))
        {
            isfleeing = true;
        }
    }
}
