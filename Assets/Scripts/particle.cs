using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{

    public float particleTime = 0.5f;
    private float particleTimer;

    // Start is called before the first frame update
    void Start()
    {
        particleTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        particleTimer += Time.deltaTime;
        if(particleTimer >= particleTime){
            Destroy(gameObject); 
        }
    }
}
