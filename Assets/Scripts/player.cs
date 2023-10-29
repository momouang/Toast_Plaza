using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    [Header("Movement")]
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

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


    public void Start()
    {
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
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //camera
        if(direction.magnitude >= 0.1f)
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
            jumpCount -= 1;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Collider[] scannedObjects = Physics.OverlapSphere(transform.position, 1);
            if(scannedObjects.Length != 0)
            {
                foreach(Collider g in scannedObjects)
                {
                    if(g.gameObject.CompareTag("Toast"))
                    {
                        g.gameObject.GetComponent<toastScore>().pickUp();
                    }
                }
            }

        }

        //healthBar
        if(currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        gamemanager.setHealth(currentHealth);

    }

    /*

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(other.CompareTag("Toast"))
            {
                other.gameObject.GetComponent<toastScore>().pickUp();
            }
            Debug.Log("collected");
        }

    }
    */
}
