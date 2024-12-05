using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ArrowController : MonoBehaviour, ISpacePressHandler
{
    public List<GameObject> arrowPrefabs;
    public MenuManager menuManager;
    public GameObject newArrow;
    public float movementSpeed = 5f;
    public float movementRange = 12f;
    private float arrowSpeed = 5f;
    public TMP_Text scoreText;
    private bool isFired = false;
    private Vector2 direction;
    private float interval = 1f;
    private int isFirst = 0;
    private float testTimer = 0.5f;
    private bool isInMenu = false;
    private bool shouldMove = true;
    private float oldTime = 0;
    private int score = 0;
    private float positionError = 0;
    private bool canClick = true;
    private int prefabsNum = 0;
    private Rigidbody2D rb;
    private int errorIndex;

    private void Start()
    {
        interval = menuManager.interval;
        Debug.Log("On start" + interval);
        movementSpeed = movementRange / interval;
        testTimer = interval;
        direction = Vector2.right;
        scoreText.text = "Счёт: 0";
        prefabsNum = arrowPrefabs.Count;
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        canClick = menuManager.canClick;
        if (Input.GetKeyDown(KeyCode.Space) && isFirst == 0)
        {
            isFirst = 1;
        }
        if (isFirst == 1 || isFirst == 2 && canClick)
        {
            //testTimer -= Time.deltaTime;
            if (!isFired && canClick)
            {
                newArrow.transform.position = transform.position;
                newArrow.transform.rotation = transform.rotation;
                //if (testTimer <= 0.02 && testTimer >= -0.02) {
                //    movementSpeed = (movementRange + transform.position.x) / interval;
                //}
                if (Input.GetKeyDown(KeyCode.Space) && isFirst == 2)
                {
                    OnSpacePressed();
                }
                isFirst = 2;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isFirst == 1 || isFirst == 2 && canClick)
        {
            MoveBow();
        }
    }

    public void OnSpacePressed()
    {
        if (isFirst == 0) {
            isFirst = 1;
        }

        if (isFirst == 2)
        {
            FireArrow(testTimer);
            oldTime = Time.time;
            testTimer = interval;
        }
    }

    private void MoveBow()
    {
        testTimer -= Time.fixedDeltaTime;
        rb.MovePosition(rb.position + direction * movementSpeed * Time.fixedDeltaTime);
    }   

    public void MetronomTicked()
    {
        transform.position = new Vector2(0, -3.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    public void OnMenuClosed()
    {
        transform.position = new Vector3(0, -3.5f, 0);
        interval = menuManager.interval;
        movementSpeed = movementRange / interval;
        transform.position = new Vector3(0, -3.5f, 0);
        isInMenu = false;
        testTimer = interval;
        testTimer = interval;
        isFirst = 0;
    }

    void FireArrow(float deltaTime)
    {
        isFired = false;
        Rigidbody2D rb = newArrow.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * arrowSpeed;
        rb.freezeRotation = true;
        if (Mathf.Abs(transform.position.x) <= 1.5f)
        {
            menuManager.UpdateScore();
        }
        else
        {
            menuManager.ResetStreak();
        }
        float percent = (8 - Mathf.Abs(transform.position.x)) / 8 * 100;

        StartCoroutine(ShrinkArrowToSize(newArrow, new Vector3(0.7f, 0.7f, 0.7f), 0.01f));

        SpawnNewArrow();
        // Создание новой стрелы через время
        //Invoke("SpawnNewArrow", 2f);
    }

    private IEnumerator ShrinkArrowToSize(GameObject arrow, Vector3 targetScale, float shrinkStep)
    {
        if (arrow != null)
        {
            Vector3 initialScale = arrow.transform.localScale;

            // Уменьшать, пока текущий масштаб не станет меньше или равен целевому
            while (arrow.transform.localScale.x > targetScale.x ||
                   arrow.transform.localScale.y > targetScale.y ||
                   arrow.transform.localScale.z > targetScale.z)
            {
                arrow.transform.localScale = Vector3.MoveTowards(arrow.transform.localScale, targetScale, shrinkStep);
                yield return null; // Ждем следующий кадр
            }

            // Убедимся, что достигли точно целевого масштаба
            arrow.transform.localScale = targetScale;
        }
    }


    void SpawnNewArrow()
    {
        int index = Random.Range(0, prefabsNum);
        newArrow = Instantiate(arrowPrefabs[index], transform.position, transform.rotation); // Новая стрела
        Rigidbody2D rb = newArrow.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        //Destroy(gameObject); // Удалить старую стрелу
    }
}