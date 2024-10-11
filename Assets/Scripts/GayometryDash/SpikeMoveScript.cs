using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMoveScript : MonoBehaviour
{
    public GameObject spike;
    public float spikeSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //spike.velocity = new Vector2(-spikeSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("collector"))
        {
            Destroy(gameObject);
        }
    }

}
