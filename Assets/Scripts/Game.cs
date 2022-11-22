using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Enums;
using TMPro;
using UnityEngine.UI;

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

            player.transform.position = safeSpawns[Random.Range(0, safeSpawns.Count)]; 

            cam.transform.position = new Vector3(player.transform.position[0], player.transform.position[1], -10);

            Spawner.Spawn(Shrieker, safeSpawns, 1);
            Spawner.Spawn(printer, safeSpawns, 1);
            Spawner.Spawn(Gun, safeSpawns, 1, NailGun);
        }

        
        private void Update() {
            if(player != null){
                healthStat.text = hr.health.ToString();
                  
                if(hr.primary != null){
                    ammoStat.enabled = true;
                    ammoStat.text = hr.primary.GetComponent<GunGen>().ammo.ToString();
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