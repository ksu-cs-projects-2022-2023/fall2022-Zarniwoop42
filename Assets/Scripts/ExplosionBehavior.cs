using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

public class ExplosionBehavior : MonoBehaviour
{
    public int damage = 20;
    private void Start(){
        GameObject.Find("Grid").GetComponent<AudioSource>().PlayOneShot(GameObject.Find("Grid").GetComponent<Game>().explosionSound, 0.01F);
    }

}
