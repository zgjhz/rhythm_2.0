using UnityEngine;
using TMPro;
using System.Collections;

public class ArrowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public MenuManager menuManager;
    public GameObject newArrow;
    public float movementSpeed = 5f;
    public float movementRange = 12f;
    public float arrowSpeed = 10f;
    public TMP_Text scoreText;
    private bool isFired = false;
    private Vector3 direction;
    private float interval = 1f;
    private int isFirst = 0;
    private float testTimer = 0.5f;
    private bool isInMenu = false;
    private bool shouldMove = true;
    private float oldTime = 0;
    private int score = 0;
    private float positionError = 0;

    private void Start()
    {
        interval = menuManager.interval;
        Debug.Log("On start" + interval);
        movementSpeed = movementRange / interval;
        testTimer = interval;
        direction = Vector3.right;
        scoreText.text = "Счёт: 0";
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
                newArrow.transform.position = transform.position;
                newArrow.transform.rotation = transform.rotation;
                //if (testTimer <= 0.02 && testTimer >= -0.02) {
                //    movementSpeed = (movementRange + transform.position.x) / interval;
                //}
                if (Input.GetKeyDown(KeyCode.Space) && isFirst == 2)
                {
                    FireArrow(testTimer);
                    oldTime = Time.time;
                    testTimer = interval;
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

    public void OnMenuOpened()
    {
        isInMenu = true;
        //shouldMove = false;
    }

    public void OnIntervalChanged()
    {
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
        if (Mathf.Abs(transform.position.x) <= 1.5f)
        {
            score++;
            scoreText.text = "Счёт: " + score;
        }

        StartCoroutine(ShrinkArrow(newArrow, 1f));

        SpawnNewArrow();
        // Создание новой стрелы через время
        //Invoke("SpawnNewArrow", 2f);
    }

    private IEnumerator ShrinkArrow(GameObject arrow, float duration)
    {
        if (arrow != null) {
            Vector3 initialScale = arrow.transform.localScale;
            Vector3 targetScale = Vector3.zero; // Размер, к которому будем уменьшать стрелу
            float elapsed = 0f;

            while (elapsed < duration)
            {
                arrow.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            arrow.transform.localScale = targetScale;
        }
    }


    void SpawnNewArrow()
    {
        newArrow = Instantiate(arrowPrefab, transform.position, transform.rotation); // Новая стрела
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //Destroy(gameObject); // Удалить старую стрелу
    }
}