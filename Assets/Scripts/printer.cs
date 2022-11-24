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
    private GameObject prin;

    private bool printed = false;
    private GameObject player; 
    public float time = 1f;
    public float timer;
    
    public string SortingLayer;
    public int OrderInLayer;


    // Start is called before the first frame update
    void Start()
    {   
        prin = gameObject.transform.parent.gameObject;
        timer = Time.time;
        player = GameObject.Find("Player");
        game = GameObject.Find("Grid").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other){
        Collider2D o = other;
        try{
            if(o != null && timer > time){
                if(o.CompareTag("Player") && !printed){
                    weaponGen();

                    DrawLine(new Vector2(prin.transform.position[0] + UnityEngine.Random.Range(-0.01f, 0.01f), prin.transform.position[1] + UnityEngine.Random.Range(-0.01f, 0.01f)), new Vector2(player.transform.position[0] + UnityEngine.Random.Range(-0.01f, 0.01f), player.transform.position[1] + UnityEngine.Random.Range(-0.01f, 0.01f)), UnityEngine.Random.Range(0.1f, 0.3f));
                    DrawLine(new Vector2(prin.transform.position[0] + UnityEngine.Random.Range(-0.01f, 0.01f), prin.transform.position[1] + UnityEngine.Random.Range(-0.01f, 0.01f)), new Vector2(player.transform.position[0] + UnityEngine.Random.Range(-0.01f, 0.01f), player.transform.position[1] + UnityEngine.Random.Range(-0.01f, 0.01f)), UnityEngine.Random.Range(0.1f, 0.3f));
                    DrawLine(new Vector2(prin.transform.position[0] + UnityEngine.Random.Range(-0.01f, 0.01f), prin.transform.position[1] + UnityEngine.Random.Range(-0.01f, 0.01f)), new Vector2(player.transform.position[0] + UnityEngine.Random.Range(-0.01f, 0.01f), player.transform.position[1] + UnityEngine.Random.Range(-0.01f, 0.01f)), UnityEngine.Random.Range(0.1f, 0.3f));


                }
            }
        }catch(NullReferenceException e){
            Debug.Log(e.ToString());
        }
    }

    void DrawLine(Vector3 start, Vector3 end, float duration = 0.5f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = new Vector3(start[0], start[1], 0);
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        lr.sortingLayerName = SortingLayer;
        lr.sortingOrder = OrderInLayer;
        lr.material = new Material (Shader.Find ("Sprites/Default"));

        Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.white;
        colorKey[0].time = 0.5f;
        colorKey[1].color = Color.yellow;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.5f;
        alphaKey[1].alpha = 0.5f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        lr.colorGradient = gradient;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
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

        Debug.Log("pos: " + prin.transform.position[0].ToString() + " , " + prin.transform.position[1].ToString());

        GameObject g = Instantiate(Gun, prin.transform.position, new Quaternion(0,0,0,1));
        
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
