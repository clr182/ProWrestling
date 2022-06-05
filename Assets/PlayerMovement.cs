using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb2D;
    Vector2 moveDir;
    public float runSpeed;
    GameObject feetAnchor;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        moveSpeed = 3;
        runSpeed = 0;
    }

    void Start()
    {
        
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
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            runSpeed = 0;
        }

    }

    void Move()
    {
        rb2D.velocity = new Vector2(moveDir.x * (moveSpeed + runSpeed), moveDir.y * (moveSpeed + runSpeed));
    }

}
