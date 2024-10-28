using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Не забудьте подключить UI, если используется Image

public class MainMenu : MonoBehaviour
{
    // Ссылки на панели превью для каждой игры
    public GameObject metronomPreview;
    public GameObject ritmamidaPreview;
    public GameObject yourRhythmPreview;
    public GameObject frogGamePreview;
    public GameObject arrowGamePreview;
    public GameObject statsWindow;

    // Ссылка на затемняющий фон (Image)
    public Image darkenBackground; // Используем Image вместо GameObject

    // Метод для кнопки "Play"
    public void PlayMetronom()
    {
        SceneManager.LoadScene("Metronom"); // Замени на название твоей игровой сцены
    }
    public void PlayRitmamida()
    {
        SceneManager.LoadScene("Ritmamida"); // Замени на название твоей игровой сцены
    }
    public void PlayYourRhythm()
    {
        SceneManager.LoadScene("YourRhythm"); // Замени на название твоей игровой сцены
    }
    public void PlayFrogGame()
    {
        SceneManager.LoadScene("FrogGame"); // Замени на название твоей игровой сцены
    }
    public void PlayArrowGame()
    {
        SceneManager.LoadScene("Arrowgame"); // Замени на название твоей игровой сцены
    }

    // Метод для кнопки "Exit"
    public void QuitGame()
    {
        Debug.Log("Игра завершена!"); // Сообщение для проверки в редакторе
        Application.Quit(); // Работает только в билде игры, а не в редакторе
    }

    // Добавляем метод Start() для скрытия панелей и затемнения при старте игры
    private void Start()
    {
        HideAllPreviews(); // Скрываем все превью при старте
        HideDarkenBackground(); // Скрываем затемняющий фон при старте
    }

    // Метод для скрытия всех превью
    private void HideAllPreviews()
    {
        metronomPreview.SetActive(false);
        ritmamidaPreview.SetActive(false);
        yourRhythmPreview.SetActive(false);
        frogGamePreview.SetActive(false);
        arrowGamePreview.SetActive(false);
        statsWindow.SetActive(false);
    }

    // Показ затемняющего фона (делаем активным и меняем прозрачность)
    private void ShowDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(true); // Включаем Image
        var color = darkenBackground.color;
        color.a = 0.5f; // Устанавливаем полупрозрачный фон (от 0 до 1)
        darkenBackground.color = color;
    }

    // Скрытие затемняющего фона (деактивируем объект)
    private void HideDarkenBackground()
    {
        darkenBackground.gameObject.SetActive(false); // Отключаем Image
    }

    // Методы для открытия превью по кнопке "Info" с затемнением
    public void ShowMetronomInfo()
    {
        HideAllPreviews(); // Скрываем другие панели
        metronomPreview.SetActive(true); // Показываем нужную
        ShowDarkenBackground(); // Включаем затемнение
    }

    public void ShowRitmamidaInfo()
    {
        HideAllPreviews();
        ritmamidaPreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowYourRhythmInfo()
    {
        HideAllPreviews();
        yourRhythmPreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowFrogGameInfo()
    {
        HideAllPreviews();
        frogGamePreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowArrowGameInfo()
    {
        HideAllPreviews();
        arrowGamePreview.SetActive(true);
        ShowDarkenBackground();
    }

    public void ShowStats()
    {
        HideAllPreviews();
        statsWindow.SetActive(true);
        ShowDarkenBackground();
    }

    // Метод для закрытия превью и затемнения
    public void ClosePreview()
    {
        HideAllPreviews(); // Скрываем все превью
        HideDarkenBackground(); // Отключаем затемнение
    }
}
