using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMoveScript : MonoBehaviour
{
    public Rigidbody2D spike;
    public float spikeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spike.velocity = new Vector2(-spikeSpeed, 0);
    }
}
