using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Enums;
using TMPro;
using UnityEngine.UI;
using System;

namespace Gameplay
{
    public class Game : MonoBehaviour
    {
        public StationGen g = null;
        public Camera cam = null;

        public int points = 0;

        public DeathScreen deathScreen;

        [SerializeField]
        public GameObject Shrieker = null;
        [SerializeField]
        public GameObject printer = null;
        [SerializeField]
        public GameObject turret = null;
        [SerializeField]
        public GameObject health = null;
        [SerializeField]
        public GameObject shieldPod = null;
        [SerializeField]
        public GameObject hatch = null;
        [SerializeField]
        public GameObject Gun = null;
        public Sprite NailGun = null;
        public GameObject player = null;
        [SerializeField]
        public TextMeshProUGUI healthStat;
        [SerializeField]
        public TextMeshProUGUI ammoStat;
        public TextMeshProUGUI stimUI;
        public List<Vector3> safeSpawns;
        public Image secondarySlot;
        public GameObject secondaryUI;
        public AudioClip explosionSound;
        public AudioClip bulletImpactSound;
        public AudioClip energyImpactSound;
        public Sprite hatchOpen;
        public bool unlocked = false;

        private float floortime = 40f;
        private float floorTimer;

        public int floor = 1;

        private HitReg hr;

        // Start is called before the first frame update
        void Start()
        {
            hr = player.transform.Find("HitReg").GetComponent<HitReg>();

            healthStat.enabled = true;
            ammoStat.enabled = false;

            g = gameObject.GetComponent(typeof(StationGen)) as StationGen;
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
            player = GameObject.Find("Player");

            newRoom();
           
        }

        void killAll(){
            foreach(GameObject i in GameObject.FindGameObjectsWithTag("Hatch")){
                Destroy(i);
            }
            foreach(GameObject i in GameObject.FindGameObjectsWithTag("Weapon")){
                if(i.transform.parent == null)
                    Destroy(i);
            }foreach(GameObject i in GameObject.FindGameObjectsWithTag("Enemy")){
                Destroy(i);
            }
            foreach(GameObject i in GameObject.FindGameObjectsWithTag("Item")){
                Destroy(i);
            }foreach(GameObject i in GameObject.FindGameObjectsWithTag("Printer")){
                Destroy(i);
            }
        }

        public void newRoom(){
            unlocked = false;
            safeSpawns = new List<Vector3>();

            killAll();

            if(floor == 1){
                floorTimer = floortime;
            }else{
                floorTimer = 0;
                floortime = 10 + floor * 3;
            }


            int w = UnityEngine.Random.Range(10, floor * 10);
            int h = UnityEngine.Random.Range(10, floor * 10);

            g.Generate(w, h, UnityEngine.Random.Range(w*h/4, w*h));

            GridCell<bool>[] validSpawns = g.validSpawns;

            foreach(GridCell<bool> e in validSpawns){
                safeSpawns.Add(new Vector3(e.X * 1.28f + 0.5f * 1.28f, e.Y * 1.28f + 0.5f * 1.28f));
            }

            player.transform.position = safeSpawns[UnityEngine.Random.Range(0, safeSpawns.Count)]; 

            cam.transform.position = new Vector3(player.transform.position[0], player.transform.position[1], -10);


            
            Spawner.Spawn(hatch, safeSpawns, 1);
            Spawner.Spawn(health, safeSpawns, UnityEngine.Random.Range(0, 1 + (int)(floor/2)));

            if(floor != 1){

                Spawner.Spawn(Shrieker, safeSpawns, UnityEngine.Random.Range(1, 1 + (int)(floor/2)));
                if(UnityEngine.Random.Range(0, 1 + (int)(5/floor)) == 1)
                    Spawner.Spawn(Gun, safeSpawns, UnityEngine.Random.Range(1, 1 + (int)(floor/4)));
                Spawner.Spawn(turret, safeSpawns, UnityEngine.Random.Range(0, 1 + (int)(floor/4)));
            }else{
                Spawner.Spawn(Gun, safeSpawns, 1);
            }

            if(UnityEngine.Random.Range(0, 1 + (int)(50/floor)) == 1){
                Spawner.Spawn(shieldPod, safeSpawns, 1);
            }
            if(UnityEngine.Random.Range(0, 1 + (int)(10/floor)) == 1){
                Spawner.Spawn(printer, safeSpawns, 1);
            }
            floor++;
        }

        
        /*private void setPrimarySlot(){
            if(primaryUI != null && primaryUI.transform.GetChild(0).GetComponent<GunDetails>().uniqueID != hr.primary.transform.GetChild(0).GetComponent<GunDetails>().uniqueID){
                Destroy(primaryUI);
                primaryUI = Instantiate(hr.primary, primarySlot.transform.position,new Quaternion(0,0,0,1));
                Destroy(primaryUI.GetComponent<GunGen>());
                primaryUI.layer = LayerMask.NameToLayer("UI");
                foreach (Transform child in primaryUI.transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("UI");;
                }
                primaryUI.transform.parent = primarySlot.transform;
            }else if(primaryUI == null || primaryUI.Equals(null) ){
                primaryUI = Instantiate(hr.primary, primarySlot.transform.position,new Quaternion(0,0,0,1));
                Destroy(primaryUI.GetComponent<GunGen>());
                primaryUI.layer = LayerMask.NameToLayer("UI");
                foreach (Transform child in primaryUI.transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("UI");;
                }
                primaryUI.transform.parent = primarySlot.transform;
            }
        }*/
        private void setSecondarySlot(){
            if((secondaryUI != null && secondaryUI.transform.GetChild(0).GetComponent<GunDetails>().uniqueID != hr.secondary.transform.GetChild(0).GetComponent<GunDetails>().uniqueID) || (secondaryUI == null || secondaryUI.Equals(null) )){
                Destroy(secondaryUI);
                secondaryUI = Instantiate(hr.secondary, secondarySlot.transform.position,new Quaternion(0,0,0,1));
                Destroy(secondaryUI.GetComponent<GunGen>());
                secondaryUI.layer = LayerMask.NameToLayer("UI");
                secondaryUI.transform.localScale = new Vector3(Math.Abs(transform.localScale[0]), transform.localScale[1], transform.localScale[2]);
                secondaryUI.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                foreach (Transform child in secondaryUI.transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("UI");;
                }
                secondaryUI.transform.parent = secondarySlot.transform;
            }
        }

        private void Update() {
            if(player != null){
                floorTimer += Time.deltaTime;

                if(floorTimer >= floortime || GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
                    GameObject.FindGameObjectsWithTag("Hatch")[0].GetComponent<SpriteRenderer>().sprite = hatchOpen;
                    unlocked = true;
                }

                healthStat.text = hr.health.ToString();

                stimUI.text = "x " + hr.stims.ToString();
                  
                if(hr.primary != null){
                    ammoStat.enabled = true;
                    ammoStat.text = hr.primary.GetComponent<GunGen>().ammo.ToString();
                    //setPrimarySlot();
                }//else{
                    //if(primaryUI != null)
                        //Destroy(primaryUI);
                //}
                if(hr.secondary != null){
                    setSecondarySlot();
                }else{
                    if(secondaryUI != null)
                        Destroy(secondaryUI);
                }
                if(hr.health <= 0 && !hr.shieldActive){
                    GameOver();
                }
            }
        }


        public void GameOver(){
            healthStat.enabled = false;
            ammoStat.enabled = false;
            deathScreen.Setup(points, floor - 2);
        }

    }
}