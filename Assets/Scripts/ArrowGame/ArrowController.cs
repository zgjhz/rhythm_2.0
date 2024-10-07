using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public MenuManager menuManager;
    public GameObject newArrow;
    public Transform spawnPoint;
    public float rotationSpeed = 30f; 
    public float arrowSpeed = 10f; 
    private bool isFired = false; 
    private Vector3 direction;
    private float interval = 1f;
    private int isFirst = 0;
    private float angle = 0f;
    private float testTimer = 0.5f;
    private bool isInMenu = false;

    private void Start()
    {
        interval = menuManager.interval;
        rotationSpeed = 60 / interval;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        angle = 0;
        testTimer = 0.5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isFirst == 0)
        {
            isFirst = 1;
        }
        if (isFirst == 1 || isFirst == 2 && !isInMenu) {
            testTimer -= Time.deltaTime;
            if (!isFired)
            {
                angle = Mathf.PingPong(Time.time * rotationSpeed, 60) - 30; // Вращение от -30 до +30
                transform.rotation = Quaternion.Euler(0, 0, angle); // Вращение вокруг оси Y
                newArrow.transform.position = spawnPoint.position;
                newArrow.transform.rotation = transform.rotation;
                if (testTimer <= 0 && isFirst == 2)
                {
                    testTimer = interval;
                    FireArrow();
                }
                isFirst = 2;
            }
        }
    }

    public void OnMenuOpened() {
        isInMenu = true;
    }

    public void OnIntervalChanged() {
        interval = menuManager.interval;
        rotationSpeed = 60 / interval;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        angle = 0;
        isInMenu = false;
        Debug.Log("Intreval changed HUUUY" + transform.rotation);
    }

    void FireArrow()
    {
        isFired = false;
        direction = transform.up;
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        rb.velocity = direction * arrowSpeed;
        rb.freezeRotation = true;
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
