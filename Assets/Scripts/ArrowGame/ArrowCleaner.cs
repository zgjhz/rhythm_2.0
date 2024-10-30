using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCleaner : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("collector")) {
            Destroy(gameObject);
        }
    }
}
