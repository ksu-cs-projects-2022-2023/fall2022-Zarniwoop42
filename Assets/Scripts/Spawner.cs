using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
public class Spawner : MonoBehaviour
{
    public static GameObject c;
    public static void Spawn(GameObject Creature, List<Vector3> safeSpawns, int numSpawn, Sprite sp = null){
        
        for(int n = 0; n < numSpawn; n++){
            c = Instantiate(Creature, safeSpawns[Random.Range(0, safeSpawns.Count)], new Quaternion(0,0,0,1));
        }
    }
}
