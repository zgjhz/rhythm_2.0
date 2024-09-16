using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMoveScript : MonoBehaviour
{
    public Rigidbody2D spike;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("floor")) {
            Debug.Log("huy");
            spike.velocity = new Vector2(-spikeSpeed, 0);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("floor"))
        {
            Debug.Log("huy");
            spike.velocity = new Vector2(-spikeSpeed, 0);
        }
    }
}
