using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using System;
using Utils;

public class printer : MonoBehaviour
{
    private Game game = null;

    [SerializeField]
    public Sprite[] Trigger;
    [SerializeField]
    public Sprite[] Clip;
    [SerializeField]
    public Sprite[] Grip;
    [SerializeField]
    public Sprite[] Barrel;
    [SerializeField]
    public Sprite[] Scope;
    [SerializeField]
    public Sprite[] Stock;
    [SerializeField]
    public GameObject Gun;


    private bool printed = false;
    private GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        game = GameObject.Find("Grid").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        Collider2D o = other;
        try{
            if(o != null){
                if(o.CompareTag("Player") && !printed){
                    weaponGen();
                }
            }
        }catch(NullReferenceException e){
            Debug.Log(e.ToString());
        }
    }


    void weaponGen(){

        printed = true;

        //FireRate * Trigger
        //AmmoNum * Clip
        //ReloadSpeed * Grip
        //ProjectileType * Barrel
            //AOE Cannon
            //Tracking Missile
            //Cursor-Following Ball
            //Beam
            //Bullet
        //Distance * Scope
        //Accuracy * Stock

        var pos = transform.position;

        GameObject g = Instantiate(Gun, transform.position, new Quaternion(0,0,0,1));
        
        var t = new GameObject().AddComponent<SpriteRenderer>();

        int tRand = UnityEngine.Random.Range(0, 3);
        t.GetComponent<SpriteRenderer>().sprite = Trigger[tRand];
        t.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
        t.transform.localScale = new Vector3(3,3,1);
        t.transform.position = new Vector2(pos[0], pos[1] - 0.15f);
        t.transform.parent = g.transform;


        int cRand = UnityEngine.Random.Range(0, 3);
        var c = new GameObject().AddComponent<SpriteRenderer>();
        c.GetComponent<SpriteRenderer>().sprite = Clip[cRand];
        c.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
        c.transform.localScale = new Vector3(3,3,1);
        
        c.transform.position = new Vector2(pos[0] - 0.3f, pos[1] - 0.2f);
        c.transform.parent = g.transform;


        int grRand = UnityEngine.Random.Range(0, 3);
        var gr = new GameObject().AddComponent<SpriteRenderer>();
        gr.GetComponent<SpriteRenderer>().sprite = Grip[grRand];
        gr.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
        gr.transform.localScale = new Vector3(3,3,1);
        
        gr.transform.position = new Vector2(pos[0] + 0.23f, pos[1] - 0.18f);
        gr.transform.parent = g.transform;


        int bRand = UnityEngine.Random.Range(0, 5);
        var b = new GameObject().AddComponent<SpriteRenderer>();
        b.GetComponent<SpriteRenderer>().sprite = Barrel[bRand];
        b.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
        b.transform.localScale = new Vector3(5,3,1);
        
        b.transform.position = transform.position;
        b.transform.parent = g.transform;


        int scRand = UnityEngine.Random.Range(0, 3);
        var sc = new GameObject().AddComponent<SpriteRenderer>();
        sc.GetComponent<SpriteRenderer>().sprite = Scope[scRand];
        sc.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
        sc.transform.localScale = new Vector3(3,3,1);
        
        sc.transform.position = new Vector2(pos[0], pos[1] + 0.2f);
        sc.transform.parent = g.transform;


        int stRand = UnityEngine.Random.Range(0, 3);
        var st = new GameObject().AddComponent<SpriteRenderer>();
        st.GetComponent<SpriteRenderer>().sprite = Stock[stRand];
        st.GetComponent<SpriteRenderer>().sortingLayerName = "Weapon";
        st.transform.localScale = new Vector3(3,3,1);
        
        st.transform.position = new Vector2(pos[0] - 0.6f, pos[1]);
        st.transform.parent = g.transform;

        g.transform.position = new Vector2(player.transform.position[0], player.transform.position[1]);

        g.transform.GetChild(0).GetComponent<GunDetails>().noSprite = true;
        g.transform.GetChild(0).GetComponent<GunDetails>().fireTime = 1.1f - tRand/2f;
        g.transform.GetChild(0).GetComponent<GunDetails>().ammoMax = (cRand + 1) * 5;
        g.transform.GetChild(0).GetComponent<GunDetails>().reloadTime = 2.1f - grRand;
        g.transform.GetChild(0).GetComponent<GunDetails>().distance = (float)(scRand)/1.5f + 0.2f;
        g.transform.GetChild(0).GetComponent<GunDetails>().accuracy = 1 - (float)(stRand)/2f;
        g.transform.GetChild(0).GetComponent<GunDetails>().barrel = bRand;
    }
}
