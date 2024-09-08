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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSpikeTimer());
        //tilePrefab.transform.localScale = new Vector2(spikeScale, spikeScale / 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnSpikeTimer()
    {
        while (true) // ����������� ����
        {
            SpawnSpike();
            yield return new WaitForSeconds(tileDelay); // �������� � ���� �������
        }
    }

    void SpawnSpike()
    {
        // ����� ������� ������ � ��������� ������� � ��� ������������ �����������
        Instantiate(tilePrefab, spawnPoint.position, spawnPoint.rotation);
    }



    IEnumerator SpawnBackgroundTimer()
    {
        while (true) // ����������� ����
        {
            SpawnBackground();
            yield return new WaitForSeconds(backgroundDelay); // �������� � ���� �������
        }
    }

    void SpawnBackground()
    {
        // ����� ������� ������ � ��������� ������� � ��� ������������ �����������
        Instantiate(backgroundPrefab, spawnBackgroundPoint.position, spawnBackgroundPoint.rotation);
    }
}
