using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class GunDetails : MonoBehaviour
    {

        public enum fireType
        {
            bullet,
            cannon,
            missile,
            ball,
            beam
        }

        public int uniqueID;

        public fireType ft = fireType.bullet;
        
        public float reloadTime = 1.0f;
        public float fireTime = 1.0f;
        public float turnOffset = 0.3f;
        public int ammoMax = 10;

        public float accuracy = 1.0f;

        public float distance = 5f;

        public GameObject projectile = null;
        public GameObject sprite = null;

        public bool noSprite = false;

        public int barrel = 10;
        void Start(){
            
            if(!noSprite){
                barrel = 10;
                projectile = (GameObject)Resources.Load("nail_0", typeof(GameObject));
                sprite = (GameObject)Resources.Load("NailGun_0", typeof(GameObject));
                gameObject.GetComponent<SpriteRenderer>().sprite = sprite.GetComponent<SpriteRenderer>().sprite;
                gameObject.transform.localScale = new Vector3(2,2,1);
            }else{
                sprite = null;
                
                if(barrel == 0)
                    projectile = (GameObject)Resources.Load("cannonball_0", typeof(GameObject));
                else if(barrel == 1)
                    projectile = (GameObject)Resources.Load("missile_0", typeof(GameObject));
                else if(barrel == 2)
                    projectile = (GameObject)Resources.Load("energyball_0", typeof(GameObject));
                else if(barrel == 4)
                    projectile = (GameObject)Resources.Load("bullet_0", typeof(GameObject));
            }
        }

        void Update(){
            
        }

    }
}