using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject tilePrefab;
    public GameObject backgroundPrefab;
    public Transform spawnPoint;
    public Transform spawnBackgroundPoint;
    public float tileDelay = 1f;
    public float backgroundDelay = 1f;
    public float spikeScale = 0.5f;
    public float tileSpace = 5f;
    public MenuManager menuManager;

    private GameObject newTile;
    private float interval;
    //private List<GameObject> tiles;
    private bool isTileSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnSpikeTimer());
        //tilePrefab.transform.localScale = new Vector2(spikeScale, spikeScale / 3);
        //tiles = new List<GameObject> { };
        interval = menuManager.interval;
        //SpawnSpike();
    }

    // Update is called once per frame
    void Update()
    {
        interval = menuManager.interval;
        if (isTileSpawned)
        {
            if (spawnPoint.position.x - newTile.transform.position.x > tileSpace)
            {
                //SpawnSpike();
            }
            //if (tiles[0].transform.position.x < -10)
            //{
            //    Destroy(tiles[0]);
            //    tiles.RemoveAt(0);
            //}
        }
    }

    //IEnumerator SpawnSpikeTimer()
    //{
    //    while (true)
    //    {
    //        SpawnSpike();
    //        yield return new WaitForSeconds(tileDelay);
    //    }
    //}

    void SpawnSpike()
    {
        isTileSpawned = true;
        newTile = Instantiate(tilePrefab, spawnPoint.position, spawnPoint.rotation);
        //tiles.Add(newTile);
        //newTile.GetComponent<Rigidbody2D>().velocity = new Vector2(-tileSpace / interval, 0);
    }



    //IEnumerator SpawnBackgroundTimer()
    //{
    //    while (true)
    //    {
    //        SpawnBackground();
    //        yield return new WaitForSeconds(backgroundDelay);
    //    }
    //}

    //void SpawnBackground()
    //{
    //    Instantiate(backgroundPrefab, spawnBackgroundPoint.position, spawnBackgroundPoint.rotation);
    //}
}
