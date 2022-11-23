using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using System;
public class ProjectileBehavior : MonoBehaviour
{
    public float moveSpeed = 15f;
    public int damage = 40;
    
    private Vector3 mousePos;
    private Vector3 directionToTarget;
    private Rigidbody2D rb;

    private float accuracy;
    private float distance = 0;
    private float distanceTimer;

    private int barrel;
    private GunDetails GD;

    private void Start(){
        GD = GameObject.Find("Player").transform.Find("HitReg").GetComponent<HitReg>().primary.transform.GetChild(0).GetComponent<GunDetails>();

        distanceTimer = 0;
        accuracy = GD.accuracy;
        distance = GD.distance;

        mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        rb = gameObject.GetComponent<Rigidbody2D>();
        
        barrel = GD.barrel;

        directionToTarget = ((new Vector3(mousePos[0] + UnityEngine.Random.Range(-accuracy, accuracy), mousePos[1] + UnityEngine.Random.Range(-accuracy, accuracy), 0)) - transform.position).normalized;
    }
    private void Update(){
        float distAdjust = 0;
        if(barrel == 2){ //Energy ball
            distAdjust = 4;
            mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            directionToTarget = ((new Vector3(mousePos[0] + UnityEngine.Random.Range(-accuracy, accuracy), mousePos[1] + UnityEngine.Random.Range(-accuracy, accuracy), 0)) - transform.position).normalized;
        }else if(barrel == 1){
            double r = 2.0;
            distAdjust = 2;
            for(double angle = 0; angle < 360; angle++){
                float xCord = (float)(r * Math.Cos(angle) + transform.position.x);
                float yCord = (float)(r * Math.Sin(angle) + transform.position.y);
            
                Vector2 point = new Vector2(xCord, yCord);

                //Debug.DrawLine(transform.position, point, Color.green);

                RaycastHit2D hit = Physics2D.Linecast(transform.position, point, (1 << LayerMask.NameToLayer("Action2")) |(1 << LayerMask.NameToLayer("Action")) );

                Debug.DrawLine(transform.position, point, Color.green);

                if(hit.collider != null){
                    var ET = hit.collider.gameObject.transform;
                    Debug.Log(hit.collider.gameObject.name.ToString());
                    if(hit.collider.gameObject.CompareTag("Enemy")){
                        Debug.DrawLine(transform.position, hit.point, Color.yellow);
                        var angle2 = (ET.position - transform.position).normalized;
                        var deg2 = Mathf.Atan2(angle2.y, angle2.x) * Mathf.Rad2Deg;

                        if (deg2<0)
                            deg2+=360;
                        directionToTarget = ((new Vector3(ET.position[0] + UnityEngine.Random.Range(-accuracy, accuracy), ET.position[1] + UnityEngine.Random.Range(-accuracy, accuracy), 0)) - transform.position).normalized;
                        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, deg2), Time.deltaTime * 5f);
                    }
                }
            }
        }

        distanceTimer += Time.deltaTime;
        rb.velocity = new Vector2(directionToTarget.x * moveSpeed, directionToTarget.y * moveSpeed).normalized * moveSpeed;

        if(distanceTimer >= distance + distAdjust){
           Instantiate((GameObject)Resources.Load("smoke", typeof(GameObject)), new Vector2(transform.position[0], transform.position[1]), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
           
           if(barrel == 0)
                explode();
            
           Destroy(gameObject);
        }
    }

    void explode(){
        double r = 0.5;
        for(double angle = 0; angle < 360; angle += 20){
            float xCord = (float)(r * Math.Cos(angle) + transform.position.x);
            float yCord = (float)(r * Math.Sin(angle) + transform.position.y);
        
            Vector2 point = new Vector2(xCord + UnityEngine.Random.Range(-1f, 1f), yCord + UnityEngine.Random.Range(-1f, 1f));
            Instantiate((GameObject)Resources.Load("explosion", typeof(GameObject)), point, new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag("Player") && !other.CompareTag("Weapon") && !other.CompareTag("Printer")){
           Instantiate((GameObject)Resources.Load("smoke", typeof(GameObject)), new Vector2(transform.position[0], transform.position[1]), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
            if(barrel == 0)
                explode();
           Destroy(gameObject);
        }
    }
}
