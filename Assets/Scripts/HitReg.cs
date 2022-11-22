using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

public class HitReg : MonoBehaviour
{
    [SerializeField]
    public int health = 100;

    public bool allowInput = true;

    public GameObject primary = null;
    public GameObject secondary = null;

    void OnTriggerEnter2D(Collider2D other){
        Collider2D o = other;
        if(o != null){
            if(o.CompareTag("Enemy")){
                health -= o.GetComponent<Shrieker>().damage;
            }
        }

    }

    private void Update() {
        if(health <= 0){
            allowInput = false;
        }    
        if(Input.GetKeyDown(KeyCode.F)){
            var temp = primary;
            primary = secondary;
            secondary = temp;
        }
    }

    private void Start() {
        allowInput = true;
    }
}
