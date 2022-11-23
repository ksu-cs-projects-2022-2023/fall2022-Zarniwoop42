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
        public GameObject Gun = null;
        public Sprite NailGun = null;
        public GameObject player = null;
        [SerializeField]
        public TextMeshProUGUI healthStat;
        [SerializeField]
        public TextMeshProUGUI ammoStat;
        public List<Vector3> safeSpawns;

        //public Image primarySlot;
        public Image secondarySlot;

        //public GameObject primaryUI;
        public GameObject secondaryUI;

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

            g.Generate();

            GridCell<bool>[] validSpawns = g.validSpawns;

            foreach(GridCell<bool> e in validSpawns){
                safeSpawns.Add(new Vector3((int.Parse((e.X).ToString()))* 1.28f + 0.2f * 1.28f, ((int.Parse((e.Y).ToString())) + (0.5f * 1.28f)) * 1.28f));
            }

            player.transform.position = safeSpawns[UnityEngine.Random.Range(0, safeSpawns.Count)]; 

            cam.transform.position = new Vector3(player.transform.position[0], player.transform.position[1], -10);

            Spawner.Spawn(Shrieker, safeSpawns, 3);
            Spawner.Spawn(printer, safeSpawns, 1);
            Spawner.Spawn(Gun, safeSpawns, 1, NailGun);
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
                healthStat.text = hr.health.ToString();
                  
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
                if(hr.health <= 0){
                    GameOver();
                }
            }
        }


        public void GameOver(){
            healthStat.enabled = false;
            ammoStat.enabled = false;
            deathScreen.Setup(points);
        }

    }
}