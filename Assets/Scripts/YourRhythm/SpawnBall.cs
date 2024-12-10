using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnBall : MonoBehaviour
{
    public GameObject markerPrefab;  // Префаб для создания новой точки
    public RectTransform accuracyBar; // Зона для отображения точности
    public AudioSource audioSource;        // Звук, который будет проигрываться
    public AudioClip hitClip;
    public AudioClip missClip;
    public TMP_Text scoreText;
    public Transform spawnPoint;
    public float interval = 2f;   // Интервал между звуками (в секундах)
    public Sprite hitSprite;
    public Sprite missSprite;
    public MenuManager menuManager;

    private float lastSoundTime = 0f;     // Время последнего звука
    private bool canClick = true;   // Можно ли нажимать кнопку
    private bool isCounting;
    private float startTime;
    private int hitStreakNum = 0;
    private int score = 0;

    public void UpdateInterval(float newInterval) {
        interval = newInterval;
    }

    void Start()
    {
        //InvokeRepeating("CountSeconds", 1.0f, interval); // Начало ритма
        isCounting = false;
        interval = menuManager.interval;
        canClick = menuManager.canClick;
    }

    void Update()
    {
        canClick = menuManager.canClick;
        interval = menuManager.interval;
        if (isCounting)
        {
            lastSoundTime = Time.time - startTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed();
        }
    }

    bool PlaySound(float markerPosition)
    {
        float accuracyBarLen = accuracyBar.localScale.x;
        Debug.Log(accuracyBarLen);
        bool flag = false;
        if (markerPosition < accuracyBarLen / 2 && markerPosition > -accuracyBarLen / 2)
        {
            audioSource.clip = hitClip;
            flag =  true;
        }
        else {
            hitStreakNum = 0;
            audioSource.clip = missClip;
            flag = false;
        }
        audioSource.Play();
        score += hitStreakNum;
        //scoreText.text = "Счёт: " + score;
        return flag;
    }

    public void OnSpacePressed()
    {
        if (canClick)
        {
            startTime = Time.time;
            isCounting = true;
            float screenWidth = spawnPoint.transform.lossyScale.x / 2;
            float deltaTime = (lastSoundTime - interval) / interval * screenWidth;
            GameObject newMarker = Instantiate(markerPrefab, spawnPoint.transform);
            SpriteRenderer sr = newMarker.GetComponent<SpriteRenderer>();
            float accuracyBarHeight = accuracyBar.localScale.y / 2 - 1;
            float rndY = 0;
            if (Mathf.Abs(deltaTime) >= 5 && Mathf.Abs(deltaTime) < 6)
            {
                rndY = Random.Range(1, -1);
            }
            else if (Mathf.Abs(deltaTime) >= 3.5f && Mathf.Abs(deltaTime) < 5)
            {
                rndY = Random.Range(3.5f, -3.5f);
            }
            else if (Mathf.Abs(deltaTime) < 3.5f)
            {
                rndY = Random.Range(accuracyBarHeight, -accuracyBarHeight);
            }
            newMarker.transform.position += new Vector3(deltaTime, rndY);
            newMarker.transform.localScale = new Vector3(0.05f, 0.05f);
            hitStreakNum = 1;
            if (PlaySound(newMarker.transform.position.x))
            {
                sr.sprite = hitSprite;
                menuManager.UpdateScore();
            }
            else
            {
                sr.sprite = missSprite;
                menuManager.ResetStreak();
            }
        }
    }
}
