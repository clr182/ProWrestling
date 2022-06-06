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

    Vector2 repulseV;
    bool running;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        moveSpeed = 3;
        runSpeed = 0;
        running = false;
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
            running = false;
        }

    }

    void Move()
    {
        if (!running)
        {
            rb2D.velocity = new Vector2(moveDir.x * (moveSpeed + runSpeed), moveDir.y * (moveSpeed + runSpeed));
        }
        else
        {
            rb2D.AddForce(repulseV);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        running = true;
            switch (collision.gameObject.tag)
            {
                case "RopeTop":
                    repulseV = new Vector2(0, (moveSpeed + runSpeed) * -1);
                    break;
                case "RopeBottom":
                    repulseV = new Vector2(0, (moveSpeed + runSpeed) * 1);
                    break;
                case "RopeLeft":
                    repulseV = new Vector2((moveSpeed + runSpeed) * 1, 0);
                    break;
                case "RopeRight":
                    repulseV = new Vector2((moveSpeed + runSpeed) * -1, 0);
                    break;
                default:
                    break;
            }

            //
            //running = true;
            //if(collision.gameObject.tag == "RopeTop")
            //{
            //    repulseV = new Vector2(0, (moveSpeed + runSpeed) * 1);
            //}
            //else if (collision.gameObject.tag == "RopeBottom")
            //{
            //    repulseV = new Vector2(0, (moveSpeed + runSpeed) * -1);
            //}
            //else if (collision.gameObject.tag == "RopeLeft")
            //{
            //    repulseV = new Vector2((moveSpeed + runSpeed) * 1, 0);
            //}
            //else if (collision.gameObject.tag == "RopeRight")
            //{
            //    repulseV = new Vector2((moveSpeed + runSpeed) * -1, 0);
            //}
    }

    

}
