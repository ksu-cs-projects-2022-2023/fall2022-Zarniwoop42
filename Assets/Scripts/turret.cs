using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay;

public class turret : MonoBehaviour
{
    [SerializeField]
    public int health = 1000;
    private Game game;

    private float time;
    private float timer;
    private float smoketime = 0.3f;
    private float smoketimer;

    private float spawnTime = 60;

    public int state = 0;

    public Animator anim;
    private BoxCollider2D box;
    private float tRound2 = 100;

    public string SortingLayer;
    public int OrderInLayer;

    private GameObject player;
    private float steptime = 0.05f;
    private float steptimer;
    public List<AudioClip> ACS;

    public AudioClip hurt;
    public AudioClip up;
    public AudioClip down;


    // Start is called before the first frame update
    void Start()
    {        
        steptimer = Time.time;

        player = GameObject.Find("Player");
        transform.position = new Vector3(transform.position[0], transform.position[1], 1);

        game = GameObject.Find("Grid").GetComponent<Game>();
        timer = Time.time;
        time = UnityEngine.Random.Range(5f, spawnTime);
        smoketimer = Time.time;

        box = gameObject.GetComponent<BoxCollider2D>();

        anim = gameObject.GetComponent<Animator>();

        anim.enabled = false;
    }

    private void laser(float i){
        RaycastHit2D N; 
        RaycastHit2D E;
        RaycastHit2D S;
        RaycastHit2D W;

        double rad = (i * (Math.PI));

        float xCord = (float)(1.2 * Math.Cos(i) + transform.position.x);
        float yCord = (float)(1.2 * Math.Sin(i) + transform.position.y);

        float xCordE = (float)(1.2 * Math.Cos(i + 0.5*(Math.PI)) + transform.position.x);
        float yCordE = (float)(1.2 * Math.Sin(i + 0.5*(Math.PI)) + transform.position.y);

        float xCordS = (float)(1.2 * Math.Cos(i + (Math.PI)) + transform.position.x);
        float yCordS = (float)(1.2 * Math.Sin(i + (Math.PI)) + transform.position.y);

        float xCordW = (float)(1.2 * Math.Cos(i - 0.5*(Math.PI)) + transform.position.x);
        float yCordW = (float)(1.2 * Math.Sin(i - 0.5*(Math.PI)) + transform.position.y);

        N = Physics2D.Raycast(transform.position, (transform.position - new Vector3(xCord,yCord, 0)).normalized, Mathf.Infinity,(1 << LayerMask.NameToLayer("Action")) );
        DrawLine(transform.position, N.point);
        E = Physics2D.Raycast(transform.position, (transform.position - new Vector3(xCordE,yCordE, 0)).normalized, Mathf.Infinity, (1 << LayerMask.NameToLayer("Action")) );
        DrawLine(transform.position, E.point);
        S = Physics2D.Raycast(transform.position, (transform.position - new Vector3(xCordS,yCordS, 0)).normalized, Mathf.Infinity,(1 << LayerMask.NameToLayer("Action")) );
        DrawLine(transform.position, S.point);
        W = Physics2D.Raycast(transform.position, (transform.position - new Vector3(xCordW,yCordW, 0)).normalized, Mathf.Infinity, (1 << LayerMask.NameToLayer("Action")) );
        DrawLine(transform.position, W.point);
        if(steptimer >= steptime){
            PlayAudio();
            steptimer = 0;
        }
        

        if(N.transform.name.ToString().Contains("Player")){
            Instantiate((GameObject)Resources.Load("laserblast", typeof(GameObject)), N.point, new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
            GetComponent<AudioSource>().PlayOneShot(hurt, 0.1F);

        }
        if(E.transform.name.ToString().Contains("Player")){
            Instantiate((GameObject)Resources.Load("laserblast", typeof(GameObject)), E.point, new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
            GetComponent<AudioSource>().PlayOneShot(hurt, 0.1F);

        }
        if(S.transform.name.ToString().Contains("Player")){
            Instantiate((GameObject)Resources.Load("laserblast", typeof(GameObject)), S.point, new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
            GetComponent<AudioSource>().PlayOneShot(hurt, 0.1F);

        }
        if(W.transform.name.ToString().Contains("Player")){
            Instantiate((GameObject)Resources.Load("laserblast", typeof(GameObject)), W.point, new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
            GetComponent<AudioSource>().PlayOneShot(hurt, 0.1F);

        }
    }

    void DrawLine(Vector3 start, Vector3 end, float duration = 0.1f)
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
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.5f;
        colorKey[1].color = Color.blue;
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

    // Update is called once per frame
    void Update()
    {
        smoketimer += Time.deltaTime;
        timer += Time.deltaTime;
        steptimer += Time.deltaTime;


        float tRound = (float)Math.Round(timer, 1);

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("RotateTurret")){
            if(tRound != tRound2){
                laser(timer);
                tRound2 = tRound; 
            }
        }

        if(state == 0 && box.enabled){
            box.enabled = false;
        }
        
        if(state == 1 && !box.enabled){
            box.enabled = true;
        }

        if(timer >= time){
            time = UnityEngine.Random.Range(5f, spawnTime);
            anim.enabled = true;
            timer = 0;
            if(state == 0){
                state = 1;
                anim.SetBool("despawn", false);
                anim.SetBool("spawn", true);
                GetComponent<AudioSource>().PlayOneShot(up, 0.7F);

            }else if(state == 1){
                state = 0;
                anim.SetBool("spawn", false);
                anim.SetBool("despawn", true);
                GetComponent<AudioSource>().PlayOneShot(down, 0.7F);

            }
        }


        if(health <= 0){
            game.points += 100;
            explode();

            Destroy(gameObject);
        }
    }



    void PlayAudio(){
        GetComponent<AudioSource>().PlayOneShot(ACS[0], 0.1F);

    }

    void explode(){
        double r = 0.5;
        for(double angle = 0; angle < 360; angle += 20){
            float xCord = (float)(r * Math.Cos(angle) + transform.position.x);
            float yCord = (float)(r * Math.Sin(angle) + transform.position.y);
        
            Vector2 point = new Vector2(xCord + UnityEngine.Random.Range(-1f, 1f), yCord + UnityEngine.Random.Range(-1f, 1f));
            Instantiate((GameObject)Resources.Load("explosion", typeof(GameObject)), point, new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        if(state != 0){
            try{
                Collider2D o = other;
                //Debug.Log(o.name.ToString());
                if(o != null){
                    if(o.GetComponent<ProjectileBehavior>() != null){
                        health -= o.GetComponent<ProjectileBehavior>().damage;
                        smoke();
                    }else if(o.GetComponent<ExplosionBehavior>() != null && !o.name.ToString().Contains("laserblast")){
                        health -= o.GetComponent<ExplosionBehavior>().damage;
                        smoke();
                    }
                }
            }catch(NullReferenceException e){
                Debug.Log(e.ToString());
            }
        }
        
    }
    
    void smoke(){
        if(smoketimer >= smoketime){
            var smoke = Instantiate((GameObject)Resources.Load("smokeDamage", typeof(GameObject)), new Vector2(transform.position[0] + UnityEngine.Random.Range(-0.2f,0.2f), transform.position[1] + UnityEngine.Random.Range(-0.2f,0.2f)), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
            var smoke2 = Instantiate((GameObject)Resources.Load("smokeDamage", typeof(GameObject)), new Vector2(transform.position[0] + UnityEngine.Random.Range(-0.2f,0.2f), transform.position[1] + UnityEngine.Random.Range(-0.2f,0.2f)), new Quaternion(0,0,0,UnityEngine.Random.Range(0, 360)));
            smoke.transform.parent = transform;
            smoke2.transform.parent = transform;
        }
        smoketimer = 0;
    }

}
