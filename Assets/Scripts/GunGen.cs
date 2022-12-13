using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gameplay
{
    public class GunGen : MonoBehaviour
    {
        private GameObject player = null;
        private SpriteRenderer sp;
        private PlayerMovement pm;
        private HitReg hr;

        [SerializeField]
        public GameObject projectile;


        private float reloadTime = 1.0f;
        private float reloadTimer;
        private float fireTime = 1.0f;
        private float fireTimer;
        private float turnOffset = 1.0f;

        public int ammoMax = 10;
        public int ammo;

        private bool reloading = false;
        private GunDetails GD;
        public GunDetails.fireType ft;

        private GameObject particles = null;
        private GameObject noFire = null;
        private GameObject reloadSprite = null;

        private bool noSprite = false;
        private bool beam = false;
        private AudioSource AS;

        public AudioClip shot;
        public AudioClip cannon;
        public AudioClip missile;
        public AudioClip energy;
        public AudioClip laser;
        public AudioClip nail;
        public AudioClip energyReload;
        public AudioClip bulletReload;
        public AudioClip cannonReload;
        public AudioClip laserReload;
        public AudioClip missileReload;
        public AudioClip nailReload;
        private float reloadAudioTime = 0.3f;
        private float reloadAudioTimer;

        void Start()
        {
            AS = GetComponent<AudioSource>();
            fireTimer = Time.time; //set timer initial values
            reloadTimer = Time.time;
            reloadAudioTimer = Time.time;

            if(transform.parent != null)
               transform.localPosition = new Vector3(0, 0, 0);

            //set various useful links
            GD = transform.GetChild(0).GetComponent<GunDetails>();
            GD.uniqueID = gameObject.GetInstanceID();
            player = GameObject.Find("Player");
            sp = transform.GetChild(0).GetComponent<SpriteRenderer>();
            ft = GD.ft;
            pm = player.GetComponent<PlayerMovement>();
            hr = player.transform.Find("HitReg").GetComponent<HitReg>();
            reloadTime = GD.reloadTime;
            fireTime = GD.fireTime;
            turnOffset = GD.turnOffset;
            ammoMax = GD.ammoMax;
            ammo = ammoMax;

            noSprite = GD.noSprite;

            if(noSprite){
                sp.enabled = false;
                if(GD.barrel == 3){
                    beam = true;
                }
            }
        }

        void Update()
        {
            fireTimer += Time.deltaTime; //increment timers each frame
            reloadTimer += Time.deltaTime;
            reloadAudioTimer += Time.deltaTime;

            if(fireTimer >= fireTime && noFire != null)
                Destroy(noFire);

            if(particles != null){
                if(player.GetComponent<SpriteRenderer>().flipX){
                    particles.transform.position = new Vector2(player.transform.position[0] - 0.6f, player.transform.position[1] + 0.2f);
                }else{
                    particles.transform.position = new Vector2(player.transform.position[0] + 0.8f, player.transform.position[1] + 0.2f);
                }
            }
            if(noFire != null){
                if(player.GetComponent<SpriteRenderer>().flipX){
                    noFire.transform.position = new Vector2(player.transform.position[0] - 0.6f, player.transform.position[1] + 0.3f);
                }else{
                    noFire.transform.position = new Vector2(player.transform.position[0] + 0.8f, player.transform.position[1] + 0.3f);
                }
            }

            if(reloading){
                if(reloadAudioTimer >= reloadAudioTime){
                    reloadAudioTimer = 0;
                    if(GD.barrel == 4){
                        AS.PlayOneShot(bulletReload, 0.4F);
                    }else if(GD.barrel == 0){
                        AS.PlayOneShot(cannonReload, 0.1F);
                    }else if(GD.barrel == 1){
                        AS.PlayOneShot(missileReload, 0.1F);
                    }else if(GD.barrel == 2){
                        AS.PlayOneShot(energyReload, 0.3F);
                    }else if(GD.barrel == 3){
                        AS.PlayOneShot(laserReload, 0.1F);
                    }else{
                        AS.PlayOneShot(nailReload, 0.3F);
                    }
                }

            }

            if(hr.primary == gameObject){ //check this weapon equipped
                sp.enabled = true;
                if(noSprite){
                    for(int i = 1; i < 7; i++){
                        gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
                    }
                }

                if(reloadTimer >= reloadTime && reloading){ //finish reloading
                    ammo = ammoMax;
                    reloading = false;
                    if(reloadSprite != null)
                        Destroy(reloadSprite);
                }

                if(!player.GetComponent<SpriteRenderer>().flipX){ //adjust weapon direction
                    transform.position = new Vector2(player.transform.position.x + turnOffset, player.transform.position.y);
                    sp.flipX = false;
                    if(noSprite)
                        transform.localScale = new Vector3(Math.Abs(transform.localScale[0]), transform.localScale[1], transform.localScale[2]);
                }else{
                    transform.position = new Vector2(player.transform.position.x - turnOffset, player.transform.position.y);
                    sp.flipX = true;
                    if(noSprite)
                        transform.localScale = new Vector3((-1)*Math.Abs(transform.localScale[0]), transform.localScale[1], transform.localScale[2]);
                }
                if(hr.allowInput){
                    if(Input.GetKeyDown(KeyCode.R)){
                        reload();
                    }
                    if (Input.GetButtonDown("Fire1"))
                    {
                        fire();
                    }
                }
            }else if(hr.secondary == gameObject){
                sp.enabled = false;
                if(noSprite){
                    for(int i = 1; i < 7; i++){
                        gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        }


        void pickUp(bool prim){

            player.GetComponent<AudioSource>().PlayOneShot(hr.pickup, 0.4F);

            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            transform.parent = player.transform;
            
            projectile = transform.GetChild(0).GetComponent<GunDetails>().projectile;
            
            if(prim){
                hr.primary = gameObject;
            }else{
                hr.secondary = gameObject;
            }

            if(!player.GetComponent<SpriteRenderer>().flipX){
                transform.position = new Vector2(player.transform.position.x + turnOffset, player.transform.position.y);
                sp.flipX = false;

            }else{
                transform.position = new Vector2(player.transform.position.x - turnOffset, player.transform.position.y);
                sp.flipX = true;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            try{
            if(other.CompareTag("Player")){
                //Debug.Log(gameObject.ToString()); 

                if(hr.primary == null && hr.secondary != gameObject){
                    pickUp(true);
                }else if(hr.secondary == null && hr.primary != gameObject){
                    pickUp(false);
                }    
            }
            }catch(Exception){};
        }


        void reload(){
            if(!reloading && ammo < ammoMax){
                reloadTimer = 0;
                reloading = true;
                reloadSprite = Instantiate((GameObject)Resources.Load("reloadBar", typeof(GameObject)), new Vector2(player.transform.position[0] +0.2f, player.transform.position[1] + 0.6f), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
                reloadSprite.transform.parent = player.transform;
                reloadSprite.GetComponent<Animator>().Play("reload");
            }
        }

        void fire(){
            if(fireTimer >= fireTime && ammo > 0 && !reloading){
                fireTimer = 0;
                Vector3 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                ammo--;
    

                if(mousePos[0] <= transform.position.x){
                    if(!player.GetComponent<SpriteRenderer>().flipX){
                        pm.turnRight(player);
                    }
                }else if(mousePos[0] > transform.position.x){
                    if(player.GetComponent<SpriteRenderer>().flipX){
                        pm.turnLeft(player);
                    }
                }
                
                var particleOffset = 1.2f;
                if(player.GetComponent<SpriteRenderer>().flipX)
                    particleOffset = -1f;
                particles = Instantiate((GameObject)Resources.Load("smoke", typeof(GameObject)), new Vector2(player.transform.position[0] + particleOffset, player.transform.position[1] + 0.2f), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));

                particles.transform.parent = player.transform;

                noFire = Instantiate((GameObject)Resources.Load("noFire", typeof(GameObject)), new Vector2(player.transform.position[0] + particleOffset, player.transform.position[1] + 0.3f), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
                noFire.GetComponent<SpriteRenderer>().sortingOrder = 1;


                noFire.transform.parent = player.transform;

                if(!beam){
                    var angle = (mousePos - transform.position).normalized;
                    var deg = Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;

                    if (deg<0)
                        deg+=360;

                    
                    if(!noSprite){
                        particleOffset = particleOffset/1.6f;
                        deg += 140; //for nail orientaion correction
                    }
                    
                    Instantiate(projectile, new Vector2(player.transform.position[0] + particleOffset, player.transform.position[1]), Quaternion.Euler(0, 0, deg));
                    if(GD.barrel == 4){
                        AS.PlayOneShot(shot, 0.3F);
                    }else if(GD.barrel == 0){
                        AS.PlayOneShot(cannon, 0.3F);
                    }else if(GD.barrel == 1){
                        AS.PlayOneShot(missile, 0.3F);
                    }else if(GD.barrel == 2){
                        AS.PlayOneShot(energy, 0.3F);
                    }else{
                        AS.PlayOneShot(nail, 0.3F);
                    }
                }else{
                    Vector2 point = new Vector2(mousePos[0] + UnityEngine.Random.Range(-GD.accuracy, GD.accuracy), mousePos[1] + UnityEngine.Random.Range(-GD.accuracy, GD.accuracy));
                    Vector2 origin = new Vector2(player.transform.position[0] + particleOffset/1.6f, player.transform.position[1]);

                    RaycastHit2D hit = Physics2D.Raycast(origin, (point - origin).normalized, Mathf.Infinity, (1 << LayerMask.NameToLayer("Action2")) | (1 << LayerMask.NameToLayer("Action")) );
                    //Debug.DrawLine(origin, point, Color.black, 2f);

                    DrawLine(new Vector2(player.transform.position[0] + particleOffset/1.6f, player.transform.position[1]), hit.point);
                    var dis = Vector3.Distance(hit.point, origin);
                    //Debug.Log("Distance: " + dis.ToString());
                    if(dis > 15)
                        dis = 15;
                    
                    AS.PlayOneShot(laser, 0.3F);
                    
                    for(int i = 5; i < ((16 - dis)/5) * ((1 + GD.distance)*2); i += 4){
                        Instantiate((GameObject)Resources.Load("explosion", typeof(GameObject)), new Vector3(hit.point[0] + UnityEngine.Random.Range(-0.5f, 0.5f), hit.point[1] + UnityEngine.Random.Range(-0.5f, 0.5f), 0), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
                    }
                    Instantiate((GameObject)Resources.Load("explosion", typeof(GameObject)), hit.point, new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));

                }

            }
        }

         void DrawLine(Vector3 start, Vector3 end, float duration = 0.2f)
         {
             GameObject myLine = new GameObject();
             myLine.transform.position = start;
             myLine.AddComponent<LineRenderer>();
             LineRenderer lr = myLine.GetComponent<LineRenderer>();

            lr.sortingOrder = 1;
            lr.material = new Material (Shader.Find ("Sprites/Default"));

            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.blue;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            lr.colorGradient = gradient;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            GameObject.Destroy(myLine, duration);
         }
        
    }
}
