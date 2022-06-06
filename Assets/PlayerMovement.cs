using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2D;
    Vector2 moveDir;
    public float runSpeed;
    
    // Bounce Vars
    public float bounceForce = 100;
    int bounceCount; // Used to calculate how many ticks to apply force
    public int bounceCountMax = 10;

    Vector2 repulseV;
    
    Vector2 lastKnownPos;
    Vector2 anchorPointPos;

    bool isInActiveTurnbuckleRadius;
    bool running;
    bool anchored;
    bool ropeBouncing;
    int SpaceBtncounter;
    bool isOnTurnBuckle;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        moveSpeed = 3;
        runSpeed = 0;
        running = false;
        anchored = false;
        SpaceBtncounter = 0;
        isInActiveTurnbuckleRadius = false;
        isOnTurnBuckle = false;
        bounceCount = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        ProcessInputs();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        //running
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            runSpeed = 2;
            running = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            runSpeed = 0;
            running = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            checkIfOnTurnbuckle();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            rb2D.AddRelativeForce(new Vector2(-2 * bounceForce, 0), ForceMode2D.Force);
        }
    }

    void Move()
    {
        if (!anchored & !ropeBouncing)
        {
            rb2D.velocity = new Vector2(moveDir.x * (moveSpeed + runSpeed), moveDir.y * (moveSpeed + runSpeed));
        }
        else if(ropeBouncing)
        {
            rb2D.AddForce(repulseV);
            bounceCount++;

            if (bounceCount >= bounceCountMax)
            {
                ropeBouncing = false;
                bounceCount = 0;
            }
        }
    }

    private void checkIfOnTurnbuckle()
    {
        if (!isOnTurnBuckle)
        {
            if (isInActiveTurnbuckleRadius)
            {
                gameObject.layer = 7;
                gameObject.transform.position = anchorPointPos;
                isOnTurnBuckle = true;
            }
        }
        else
        {
            gameObject.layer = 6;
            gameObject.transform.position = lastKnownPos;
            isOnTurnBuckle = false;
        }

        if(isOnTurnBuckle)
        {
            anchored = true;
        }
        else
        {
            anchored = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // if (!running)
        //     return;

        if(collision.gameObject.CompareTag("RopeTop"))
        {
            repulseV = new Vector2(0, (moveSpeed + runSpeed) * -1 * bounceForce);
            ropeBouncing = true;
        }
        else if(collision.gameObject.CompareTag("RopeBottom"))
        {
            repulseV = new Vector2(0, (moveSpeed + runSpeed) * 1 * bounceForce);
            ropeBouncing = true;
        }
        else if (collision.gameObject.CompareTag("RopeLeft"))
        {
            repulseV = new Vector2((moveSpeed + runSpeed) * 1 * bounceForce, 0);
            ropeBouncing = true;
        }
        else if (collision.gameObject.CompareTag("RopeRight"))
        {
            repulseV = new Vector2((moveSpeed + runSpeed) * -1 * bounceForce, 0);
            Debug.Log(repulseV);
            ropeBouncing = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("AnchorPoint"))
        {
            isInActiveTurnbuckleRadius = true;
            Vector2 childAnchorPos = collision.gameObject.GetComponentInChildren<CircleCollider2D>().transform.position;
            lastKnownPos = childAnchorPos;
            anchorPointPos = collision.gameObject.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AnchorPoint"))
        {
            isInActiveTurnbuckleRadius = false;
        }
    }
    
}
