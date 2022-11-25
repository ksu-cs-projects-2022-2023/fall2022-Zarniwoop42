using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay;

public class consumable : MonoBehaviour
{
    public float time = 1f;
    public float timer;
    private HitReg hr;
    private GameObject player;
    
    
    
    private Vector3 star;
    void Start()
    {            
        player = GameObject.Find("Player");
        hr = player.transform.Find("HitReg").GetComponent<HitReg>();
        timer = Time.time + UnityEngine.Random.Range(-4f,4f);
        star = transform.position;
    }

    void Update()
    {   
        timer += Time.deltaTime * 15;
        transform.position = star + new Vector3(0.0f, Mathf.Sin(timer/10f) / 5f, 0.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            if(gameObject.name.ToString().Contains("health"))
                hr.stims++;
            else if(gameObject.name.ToString().Contains("shieldPod"))
                hr.shieldEquip = true;
            Destroy(gameObject);
        }
    }
}
