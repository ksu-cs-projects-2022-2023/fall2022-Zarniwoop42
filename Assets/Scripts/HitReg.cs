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
    public int stims = 0;

    public bool allowInput = true;

    public GameObject primary = null;
    public GameObject secondary = null;

    private int healthUpdate;

    public Image hurt;
    public Image heal;
    public Image shield;
    public Image shieldPod;

    public float time = 0.3f;
    public float hurttimer;
    public float healtimer;
    public float shieldtimer;
    public bool shieldEquip = false;
    public bool shieldActive = false;

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
        hurttimer += Time.deltaTime;
        healtimer += Time.deltaTime;
        shieldtimer += Time.deltaTime;

        if(shieldEquip){
            shieldPod.GetComponent<Image>().enabled = true;
        }else{
            shieldPod.GetComponent<Image>().enabled = false;
        }

        if(shieldActive){
            shieldPod.GetComponent<Animator>().enabled = true;
        }else{
            shieldPod.GetComponent<Animator>().enabled = false;
        }

        if(health <= 0){
            allowInput = false;
        }else{

            if (hurt.enabled && hurttimer > time){
                hurt.enabled = false;
            }
            if (shield.enabled && shieldtimer > time){
                shield.enabled = false;
            }

            if(hurt.enabled == false && healthUpdate > health && hurttimer > time){
                if(!shieldActive){
                    healthUpdate = health;
                    hurttimer = 0;
                    hurt.enabled = true;
                }else{
                    health = healthUpdate;
                    shieldtimer = 0;
                    shield.enabled = true;
                }
            }

            if(heal.enabled == false && healthUpdate < health && healtimer > time){
                healthUpdate = health;
                healtimer = 0;
                heal.enabled = true;
            }else if (heal.enabled && healtimer > time){
                heal.enabled = false;
            }


            if(healthUpdate != health){
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
        hurttimer = Time.time;
        healtimer = Time.time;
        shieldtimer = Time.time;
        healthUpdate = health;
        allowInput = true;
    }
}
