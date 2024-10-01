using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
    public void PlayGayometryDash()
    {
        SceneManager.LoadScene("GayometryDash"); // Замени на название твоей игровой сцены
    }

    // Метод для кнопки "Exit"
    public void QuitGame()
    {
        Debug.Log("Игра завершена!"); // Сообщение для проверки в редакторе
        Application.Quit(); // Работает только в билде игры, а не в редакторе
    }
}
