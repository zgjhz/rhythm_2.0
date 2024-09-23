using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public Rigidbody2D player;
    private float angularVelocity;
    private float velocity = 5f;
    private bool isGrounded;
    public MenuManager menuManager;
    public GameObject tilePrefab;
    public float rayDistance = 0.8f;
    public Transform spawnPoint;
    public Transform tileCollector;

    private float interval;
    private float gravity = 20f;
    public float tileSpace = 5f;
    private float timer;
    private float xVelocity;
    private bool isFirst = true;
    private GameObject newTile;
    private int tileIndex = 0;
    private float deltaTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        interval = menuManager.interval;
        timer = interval;
        xVelocity = tileSpace / interval;
        //player.gravityScale = -0.34f * interval + 1.13f;
        //gravity = (-0.34f * interval + 1.13f) * fixedGravity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "spike") {
        //    Debug.Log("you lose");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        tileCollector.position = new Vector2(player.position.x - 50f, 0);
        if (isFirst == false)
        {
            timer -= Time.deltaTime;
        }

        if (isFirst == false && tileIndex >= 4) {
            tileSpace = newTile.transform.position.x - player.transform.position.x - 20f;
        }
        interval = menuManager.interval + deltaTime;
        velocity = gravity * interval / 2;
        xVelocity = tileSpace / interval;
        if (Input.GetKeyDown("space") && isGrounded)
        {
            isFirst = false;
            newTile = Instantiate(tilePrefab, spawnPoint.position + new Vector3(tileIndex * 5f, 0f, 0f), spawnPoint.rotation);
            tileIndex++;
            deltaTime = interval - timer;
            xVelocity += 10 * xVelocity * (interval - timer) / interval;
            angularVelocity = 90 * gravity / (velocity * 2);
            player.angularVelocity = -angularVelocity;
            player.velocity = new Vector2(xVelocity, velocity);
        }

        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D hit_left = Physics2D.Raycast(player.position, new Vector2(0.2f, -1), rayDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D hit_right = Physics2D.Raycast(player.position, new Vector2(-0.2f, -1), rayDistance, LayerMask.GetMask("Ground"));

        if (hit.collider != null || hit_left.collider != null || hit_right.collider != null)
        {
            timer = interval;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if(player.transform.position.y < -1)
        {

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
