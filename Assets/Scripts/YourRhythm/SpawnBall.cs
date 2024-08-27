using UnityEngine;
using UnityEngine.UI;

public class SpawnBall : MonoBehaviour
{
    public GameObject markerPrefab;  // Префаб для создания новой точки
    public RectTransform accuracyBar; // Зона для отображения точности
    public AudioSource audioSource;        // Звук, который будет проигрываться
    public AudioClip hitClip;
    public AudioClip missClip;

    private float interval = 1f;   // Интервал между звуками (в секундах)
    private float lastSoundTime = 0f;     // Время последнего звука
    private bool canClick = true;   // Можно ли нажимать кнопку
    private bool isCounting;
    private float startTime;

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
        if (markerPosition < accuracyBarLen / 2 && markerPosition > -accuracyBarLen / 2)
        {
            audioSource.clip = hitClip;
        }
        else {
            audioSource.clip = missClip;
        }
        audioSource.Play();
    }

    void OnSpacePressed()
    {
        startTime = Time.time;
        isCounting = true;
        float screenWidth = accuracyBar.transform.lossyScale.x / 2;
        float deltaTime = (lastSoundTime - interval) / interval * screenWidth;
        Debug.Log(lastSoundTime);
        GameObject newMarker = Instantiate(markerPrefab, accuracyBar.transform);
        newMarker.transform.position += new Vector3(deltaTime, 0);
        newMarker.transform.localScale = new Vector3(0.05f, 0.05f);
    }
}
