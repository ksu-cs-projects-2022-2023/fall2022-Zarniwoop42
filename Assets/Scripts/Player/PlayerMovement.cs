using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
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
    private float steptime = 0.3f;
    private float steptimer;
    private float shieldsteptime = 0.3f;
    private float shieldsteptimer;
    public List<AudioClip> ACS;
    public AudioClip shield;
    private int step = 0;
    void Start()
    {
        shieldtimer = Time.time;
        steptimer = Time.time;
        shieldsteptimer = Time.time;
        player = GameObject.Find("Player");
        hr = player.transform.Find("HitReg").GetComponent<HitReg>();
    }


    // Update is called once per frame
    void Update()
    {        
        shieldtimer += Time.deltaTime;
        steptimer += Time.deltaTime;
        shieldsteptimer += Time.deltaTime;
        if(hr.allowInput)
            ProcessInputs();
        
       if(hr.shieldActive && shieldtimer >= shieldtime){
            hr.shieldActive = false;
            shieldtimer = -10;
            hr.noShield.enabled = true;
       }else if(hr.shieldEquip && shieldtimer >= shieldtime){
            hr.noShield.enabled = false;
       }
       if(hr.shieldActive && shieldsteptimer >= shieldsteptime){
            GetComponent<AudioSource>().PlayOneShot(shield, 0.1F);
            shieldsteptimer = 0;
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

    
    void PlayAudio(){
        GetComponent<AudioSource>().PlayOneShot(ACS[step], 0.3F);
        step++;
        if(step >= ACS.Count)
            step = 0;
    }

    void ProcessInputs()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && hr.stims > 0 && hr.health < 100){
            hr.health += 20;
            hr.stims--;
            if(hr.health > 100)
                hr.health = 100;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && hr.shieldEquip && shieldtimer > shieldtime){
            hr.shieldActive = true;
            shieldtimer = 0;

        }  if(Input.GetKeyDown(KeyCode.Q)){
            if(hr.primary != null){
                foreach(Transform i in player.transform){
                    if(i.CompareTag("Weapon") && i.transform.GetChild(0).GetComponent<GunDetails>().uniqueID == hr.primary.transform.GetChild(0).GetComponent<GunDetails>().uniqueID){
                        GetComponent<AudioSource>().PlayOneShot(hr.pickup, 0.3F);
                        i.transform.SetParent(null);
                        i.transform.GetChild(0).GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Ground");
                        i.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 2;
                        hr.primary = null;
                        i.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }

        }
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX != 0 || moveY != 0){
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Play("spacewalk");
            if(steptimer >= steptime){
                steptimer = 0;
                PlayAudio();
            }
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
