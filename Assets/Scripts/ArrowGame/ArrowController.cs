using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public MenuManager menuManager;
    public GameObject newArrow;
    public Transform spawnPoint;
    public float movementSpeed = 5f;
    public float movementRange = 12f;
    public float arrowSpeed = 10f; 
    private bool isFired = false; 
    private Vector3 direction;
    private float interval = 1f;
    private int isFirst = 0;
    private float testTimer = 0.5f;
    private bool isInMenu = false;
    private bool shouldMove = true;
    private float oldTime = 0;

    private void Start()
    {
        interval = menuManager.interval;
        Debug.Log("On start" + interval);
        movementSpeed = movementRange / interval;
        testTimer = interval;
        direction = Vector3.right;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isFirst == 0)
        {
            isFirst = 1;
        }
        if (isFirst == 1 || isFirst == 2 && !isInMenu)
        {
            testTimer -= Time.deltaTime;
            MoveBow();
            if (!isFired && !isInMenu)
            {
                newArrow.transform.position = spawnPoint.position;
                newArrow.transform.rotation = transform.rotation;
                if (Input.GetKeyDown(KeyCode.Space) && isFirst == 2)
                {
                    testTimer = interval;
                    FireArrow(Time.time - oldTime);
                    Debug.Log(Time.time - oldTime + "     " + interval);
                    oldTime = Time.time;
                }
                isFirst = 2;
            }
        }
    }

    private void MoveBow()
    {
        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = -direction;
            //Debug.Log(transform.position.x);
        }
    }

    //void Resume() {
    //    shouldMove = true;
    //}

    public void OnMenuOpened() {
        isInMenu = true;
        //shouldMove = false;
    }

    public void OnIntervalChanged() {
        interval = menuManager.interval;
        movementSpeed = movementRange / interval;
        transform.position = new Vector3(0, -3.5f, 0);
        isInMenu = false;
        testTimer = interval;
        //Invoke("Resume", 2f);
        Debug.Log("Intreval changed HUUUY" + transform.position);
    }

    void FireArrow(float deltaTime)
    {
        isFired = false;
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        rb.velocity = transform.up * arrowSpeed;
        rb.freezeRotation = true;
        if (Mathf.Abs(deltaTime - interval) / interval <= 0.1) {
            newArrow.transform.position = new Vector3(0, 0, 0);
        }
        SpawnNewArrow();
        // Создание новой стрелы через время
        //Invoke("SpawnNewArrow", 2f);
    }

    void SpawnNewArrow()
    {
        newArrow = Instantiate(arrowPrefab, spawnPoint.position, transform.rotation); // Новая стрела
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        //Destroy(gameObject); // Удалить старую стрелу
    }
}
