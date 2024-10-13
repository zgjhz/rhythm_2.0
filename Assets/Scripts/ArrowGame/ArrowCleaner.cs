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
        Debug.Log("HUY1");
        if (collision.collider.CompareTag("collector")) {
            Destroy(gameObject);
            Debug.Log("HUY2");
        }
    }
}
