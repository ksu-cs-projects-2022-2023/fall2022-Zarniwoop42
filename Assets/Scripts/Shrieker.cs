using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Shrieker : MonoBehaviour
{

    private GameObject player = null;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sp;
    private Vector3 directionToTarget;

    public float moveSpeed = 4;
    public float agroRange = 6;

    private bool left = true;

    [SerializeField]
    public int health = 100;
    public int damage = 20;

    private bool jumping = false;

    private float time = 0.3f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time;
        player = GameObject.Find("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        sp = gameObject.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(jumping && timer >= time){
            jumping = false;
            if(sp.flipX){
                sp.flipX = false;
                left = true;
            }
            else{
                sp.flipX = true;
                left = false;
            }
        }

        if(health <= 0){
            Destroy(gameObject);
        }

        if(!jumping){
            if(visiblePlayer(agroRange))
                ChasePlayer();
            else
                StopChase();    
        }

        if (rb.velocity[0] < 0)
        {
            sp.flipX = false;
            left = true;
        }
        else if(rb.velocity[0] > 0)
        {
            sp.flipX = true;
            left = false;
        }
     
    }

    void ChasePlayer(){
        directionToTarget = (player.transform.position - transform.position).normalized;

        rb.velocity = new Vector2(directionToTarget.x * moveSpeed, directionToTarget.y * moveSpeed);
        
        if(rb.velocity != Vector2.zero){
            animator.enabled = true;
            animator.SetTrigger("Run");
        }else{
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Jump");
            animator.SetTrigger("Stop");
        }
    }

    void StopChase(){
        rb.velocity = new Vector2(0,0);
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Jump");
        animator.SetTrigger("Stop");
    }

    void jumpBack(Collision2D c){
        jumping = true;
        timer = 0;
        animator.ResetTrigger("Run");
        animator.SetTrigger("Jump");
        var direction = Vector3.Reflect(rb.velocity.normalized, c.contacts[0].normal);
        var speed = rb.velocity.magnitude * 5;

        rb.velocity = direction * MathF.Max(speed, 0f);
    }


    bool visiblePlayer(float distance){

        double r = (double)distance;

        for(double angle = 0; angle < 360; angle++){

            float xCord = (float)(r * Math.Cos(angle) + transform.position.x);
            float yCord = (float)(r * Math.Sin(angle) + transform.position.y);
        
            if(left && xCord < transform.position.x && ( yCord > transform.position.y - 1 && yCord < transform.position.y + 3)){
                xCord = (float)((r+20) * Math.Cos(angle) + transform.position.x);
                yCord = (float)((r+20) * Math.Sin(angle) + transform.position.y);
            }else if(!left && xCord > transform.position.x && ( yCord > transform.position.y - 1 && yCord < transform.position.y + 3)){
                xCord = (float)((r+20) * Math.Cos(angle) + transform.position.x);
                yCord = (float)((r+20) * Math.Sin(angle) + transform.position.y);
            }else{

            }
            
            Vector2 point = new Vector2(xCord, yCord);

            //Debug.DrawLine(transform.position, point, Color.green);

            RaycastHit2D hit = Physics2D.Linecast(transform.position, point, 1 << LayerMask.NameToLayer("Action"));

            Debug.DrawLine(transform.position, point, Color.green);

            if(hit.collider != null)
                if(hit.collider.gameObject.CompareTag("Player")){
                    Debug.DrawLine(transform.position, hit.point, Color.yellow);
                    return true;
                }else{
                    //Debug.DrawLine(transform.position, point, Color.blue);
                }
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other){
        try{
            Collider2D o = other;
            Debug.Log(o.name.ToString());
            if(o != null){
                if(o.GetComponent<ProjectileBehavior>() != null){
                    health -= o.GetComponent<ProjectileBehavior>().damage;
                }
            }
        }catch(NullReferenceException e){
            Debug.Log(e.ToString());
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.CompareTag("Player")){
            jumpBack(other);
        }
    }
}
