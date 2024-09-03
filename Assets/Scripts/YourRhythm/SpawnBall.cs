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

    private float lastSoundTime = 0f;     // Время последнего звука
    private bool canClick = true;   // Можно ли нажимать кнопку
    private bool isCounting;
    private float startTime;
    private int hitStreakNum = 0;
    private int score = 0;

    void Start()
    {
        //InvokeRepeating("CountSeconds", 1.0f, interval); // Начало ритма
        isCounting = false;
    }

    void Update()
    {
        if (isCounting)
        {
            lastSoundTime = Time.time - startTime;
        }
        if (canClick && Input.GetKeyDown(KeyCode.Space)) // Проверяем, нажата ли клавиша пробел
        {
            OnSpacePressed(); // Вызываем обработку нажатия пробела
        }
    }

    void PlaySound(float markerPosition)
    {
        float accuracyBarLen = accuracyBar.localScale.x;
        Debug.Log(accuracyBarLen);
        if (markerPosition < accuracyBarLen / 2 && markerPosition > -accuracyBarLen / 2)
        {
            audioSource.clip = hitClip;
        }
        else {
            hitStreakNum = 0;
            audioSource.clip = missClip;
        }
        audioSource.Play();
        score += hitStreakNum;
        scoreText.text = "Счёт: " + score;
    }

    void OnSpacePressed()
    {
        startTime = Time.time;
        isCounting = true;
        float screenWidth = spawnPoint.transform.lossyScale.x / 2;
        float deltaTime = (lastSoundTime - interval) / interval * screenWidth;
        GameObject newMarker = Instantiate(markerPrefab, spawnPoint.transform);
        float accuracyBarHeight = accuracyBar.localScale.y / 2 - 1;
        float rndY = Random.Range(accuracyBarHeight, -accuracyBarHeight);
        newMarker.transform.position += new Vector3(deltaTime, rndY);
        newMarker.transform.localScale = new Vector3(0.05f, 0.05f);
        hitStreakNum++;
        PlaySound(newMarker.transform.position.x);
    }
}
