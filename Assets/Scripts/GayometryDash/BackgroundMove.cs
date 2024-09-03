using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public Rigidbody2D background;
    public float backgroundSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        background.velocity = new Vector2(-backgroundSpeed, 0);
    }
}