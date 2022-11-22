using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
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

    private void Start(){
        distanceTimer = 0;
        accuracy = GameObject.Find("Player").transform.Find("HitReg").GetComponent<HitReg>().primary.transform.GetChild(0).GetComponent<GunDetails>().accuracy;
        distance = GameObject.Find("Player").transform.Find("HitReg").GetComponent<HitReg>().primary.transform.GetChild(0).GetComponent<GunDetails>().distance;

        mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        rb = gameObject.GetComponent<Rigidbody2D>();
        
        directionToTarget = ((new Vector3(mousePos[0] + UnityEngine.Random.Range(-accuracy, accuracy), mousePos[1] + UnityEngine.Random.Range(-accuracy, accuracy), 0)) - transform.position).normalized;
    }
    private void Update(){
        distanceTimer += Time.deltaTime;
        rb.velocity = new Vector2(directionToTarget.x * moveSpeed, directionToTarget.y * moveSpeed).normalized * moveSpeed;

        if(distanceTimer >= distance){
           Instantiate((GameObject)Resources.Load("smoke", typeof(GameObject)), new Vector2(transform.position[0], transform.position[1]), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
           Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag("Player") && !other.CompareTag("Weapon") && !other.CompareTag("Printer")){
           Instantiate((GameObject)Resources.Load("smoke", typeof(GameObject)), new Vector2(transform.position[0], transform.position[1]), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
           Destroy(gameObject);
        }
    }
}
