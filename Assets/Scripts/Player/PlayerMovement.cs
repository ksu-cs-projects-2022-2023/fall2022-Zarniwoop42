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
    public HitReg hr;

    private float shieldtime = 5f;
    private float shieldtimer;

    void Start()
    {
        shieldtimer = Time.time;
        player = GameObject.Find("Player");
        hr = player.transform.Find("HitReg").GetComponent<HitReg>();

    }


    // Update is called once per frame
    void Update()
    {        
        shieldtimer += Time.deltaTime;
        if(hr.allowInput)
            ProcessInputs();
        
       if(hr.shieldActive && shieldtimer >= shieldtime){
            hr.shieldActive = false;
            shieldtimer = -10;
       } 
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
        if(Input.GetKeyDown(KeyCode.E) && hr.stims > 0 && hr.health < 100){
            hr.health += 20;
            hr.stims--;
            if(hr.health > 100)
                hr.health = 100;
        }
        if(Input.GetKeyDown(KeyCode.Space) && hr.shieldEquip && shieldtimer > shieldtime){
            hr.shieldActive = true;
            shieldtimer = 0;

        }
        
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
