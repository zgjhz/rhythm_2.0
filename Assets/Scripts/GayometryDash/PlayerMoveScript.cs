using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public Rigidbody2D player;
    public float angularVelocity;
    private float gravity = 20f;
    public float velocity = 5f;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "spike") {
            Debug.Log("you lose");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            angularVelocity = 90 * gravity / (velocity * 2);
            player.angularVelocity = -angularVelocity;
            player.velocity = new Vector2(0f, velocity);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isGrounded = false;
        }
    }
}
