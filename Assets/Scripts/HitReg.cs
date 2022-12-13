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
    public Image noShield;

    public float time = 0.3f;
    public float hurttimer;
    public float healtimer;
    public float shieldtimer;
    public bool shieldEquip = false;
    public bool shieldActive = false;
	private int index = 0;
    private int frame = 0;
    public Sprite[] sprites;

    private AudioSource AS;

    public AudioClip pain;
    public AudioClip block;
    public AudioClip healSound;
    public AudioClip pickup;
    public AudioClip swap;

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

        if(Physics2D.IsTouching(GetComponent<Collider2D>(), GameObject.FindGameObjectsWithTag("Hatch")[0].GetComponent<Collider2D>())){
            if(GameObject.Find("Grid").GetComponent<Game>().unlocked && Input.GetKeyDown(KeyCode.E)){
                GameObject.Find("Grid").GetComponent<Game>().newRoom();
            }
        }

        if(shieldActive){
            if (index != sprites.Length){ //inspiration from: https://gist.github.com/almirage/e9e4f447190371ee6ce9
                frame ++;
                if (frame >= 6){
                    shieldPod.GetComponent<Image>().sprite = sprites[index];
                    frame = 0;
                    index ++;
                    if (index >= sprites.Length) {
                        index = 0;
                    }
                }
            }
        }else{
            shieldPod.GetComponent<Image>().sprite = sprites[0];
        }

        if(health <= 0 && !shieldActive){
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
                    AS.PlayOneShot(pain, 0.3F);
                }else{
                    health = healthUpdate;
                    shieldtimer = 0;
                    shield.enabled = true;
                    AS.PlayOneShot(block, 0.3F);
                }
            }

            if(heal.enabled == false && healthUpdate < health && healtimer > time){
                healthUpdate = health;
                healtimer = 0;
                heal.enabled = true;
                AS.PlayOneShot(healSound, 0.3F);
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
            AS.PlayOneShot(swap, 0.3F);
        }
    }

    private void Start() {
        AS = GameObject.Find("Player").GetComponent<AudioSource>();

        hurttimer = Time.time;
        healtimer = Time.time;
        shieldtimer = Time.time;
        healthUpdate = health;
        allowInput = true;
        noShield.GetComponent<Image>().enabled = false;
    }
}
