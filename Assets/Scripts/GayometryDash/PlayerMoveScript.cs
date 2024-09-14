using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public Rigidbody2D player;
    private float angularVelocity;
    private float fixedGravity = 20f;
    private float velocity = 5f;
    private bool isGrounded;
    public MenuManager menuManager;
    public float rayDistance = 2f;

    private float interval;
    private float gravity;

    // Start is called before the first frame update
    void Start()
    {
        interval = menuManager.interval;
        player.gravityScale = -0.34f * interval + 1.13f;
        gravity = (-0.34f * interval + 1.13f) * fixedGravity;
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
        interval = menuManager.interval;
        player.gravityScale = -0.34f * interval + 1.13f;
        gravity = (-0.34f * interval + 1.13f) * fixedGravity;
        Debug.Log(gravity);
        velocity = gravity * interval / 2;
        if (Input.GetKeyDown("space") && isGrounded)
        {
            angularVelocity = 90 * gravity / (velocity * 2);
            player.angularVelocity = -angularVelocity;
            player.velocity = new Vector2(0f, velocity);
        }

        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D hit_left = Physics2D.Raycast(player.position, new Vector2(0.2f, -1), rayDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D hit_right = Physics2D.Raycast(player.position, new Vector2(-0.2f, -1), rayDistance, LayerMask.GetMask("Ground"));

        if (hit.collider != null || hit_left.collider != null || hit_right.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("floor"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("floor"))
    //    {
    //        isGrounded = false;
    //    }
    //}
}
