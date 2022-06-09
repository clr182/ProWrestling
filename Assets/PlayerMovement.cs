using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2D;
    Vector2 moveDir;
    public float runSpeed;

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
    }

    private void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            Move();
        }
    }

    void Update()
    {
        if (this.isLocalPlayer)
        {
            ProcessInputs();
        }
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
            ropeBouncing = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            checkIfOnTurnbuckle();
        }
    }

    void Move()
    {
        if (!anchored & !ropeBouncing)
        {
            rb2D.velocity = new Vector2(moveDir.x * (moveSpeed + runSpeed), moveDir.y * (moveSpeed + runSpeed));
        }
        else if(ropeBouncing && !anchored)
        {
            rb2D.AddForce(repulseV);
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
        if (running)
        {

            if (collision.gameObject.CompareTag("RopeTop"))
            {
                repulseV = new Vector2(0, (moveSpeed + runSpeed) * -1);
                ropeBouncing = true;
            }
            else if (collision.gameObject.CompareTag("RopeBottom"))
            {
                repulseV = new Vector2(0, (moveSpeed + runSpeed) * 1);
                ropeBouncing = true;
            }
            else if (collision.gameObject.CompareTag("RopeLeft"))
            {
                repulseV = new Vector2((moveSpeed + runSpeed) * 1, 0);
                ropeBouncing = true;
            }
            else if (collision.gameObject.CompareTag("RopeRight"))
            {
                repulseV = new Vector2((moveSpeed + runSpeed) * -1, 0);
                ropeBouncing = true;
            }
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
