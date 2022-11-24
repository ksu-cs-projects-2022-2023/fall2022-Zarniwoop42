using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using System;
using UnityEngine.UI;

public class HitReg : MonoBehaviour
{
    [SerializeField]
    public int health = 100;

    public bool allowInput = true;

    public GameObject primary = null;
    public GameObject secondary = null;

    private int healthUpdate;

    public Image hurt;

    public float time = 0.3f;
    public float timer;

    void OnTriggerEnter2D(Collider2D other){
        Collider2D o = other;
        if(o != null){
            if(o.CompareTag("Enemy") && o.name.ToString().Contains("shrieker")){
                health -= o.GetComponent<Shrieker>().damage;
            }else if(o.GetComponent<ExplosionBehavior>() != null){
                health -= o.GetComponent<ExplosionBehavior>().damage;
            }
        }

    }


    private void Update() {
        timer += Time.deltaTime;

        if(health <= 0){
            allowInput = false;
        }else{

            if(hurt.enabled == false && healthUpdate > health && timer > time){
                healthUpdate = health;
                timer = 0;
                hurt.enabled = true;
            }else if (hurt.enabled && timer > time){
                hurt.enabled = false;
            }

            if(healthUpdate > health){
                healthUpdate = health;
            }
        }



        
        if(Input.GetKeyDown(KeyCode.F)){
            var temp = primary;
            primary = secondary;
            secondary = temp;
        }
    }

    private void Start() {
        timer = Time.time;
        healthUpdate = health;
        allowInput = true;
    }
}
