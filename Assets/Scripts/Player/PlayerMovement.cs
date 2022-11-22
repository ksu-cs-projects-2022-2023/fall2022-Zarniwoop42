using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public bool moving = false; 

    public GameObject player;


    void Start()
    {
        player = GameObject.Find("Player");
    }


    // Update is called once per frame
    void Update()
    {
        if(player.transform.GetChild(1).GetComponent<HitReg>().allowInput)
            ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void turnLeft(GameObject p){
        p.GetComponent<SpriteRenderer>().flipX = false;
    }

    public void turnRight(GameObject p){
        p.GetComponent<SpriteRenderer>().flipX = true;
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX != 0 || moveY != 0){
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Play("spacewalk");
        }else{
            GetComponent<Animator>().enabled = false;
        }

        if (moveX > 0)
        {
            turnLeft(player);
        }
        else if(moveX < 0)
        {
            turnRight(player);
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
