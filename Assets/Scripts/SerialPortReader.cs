using System.IO.Ports;
using UnityEngine;

public class SerialPortReader : MonoBehaviour
{
    private SerialPort serialPort;
    public string portName = "COM3"; // Название COM-порта, например, "COM3"
    public int baudRate = 9600; // Скорость передачи данных
    public GameObject MainController;
    public string gameTag = "";

    private dynamic script;

    void Start()
    {
        // Инициализация и открытие порта
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        serialPort.ReadTimeout = 1000; // Таймаут чтения в миллисекундах

        switch (gameTag) {
            case ("YourRhythm"):
                script = MainController.GetComponent<SpawnBall>();
                break;
            case ("ArrowGame"):
                script = MainController.GetComponent<ArrowController>();
                break;
            case ("Metronom"):
                script = MainController.GetComponent<BallMovement>();
                break;
            case ("Ritmamida"):
                script = MainController.GetComponent<Ritmamida>();
                break;
            case ("FrogJump"):
                script = MainController.GetComponent<FrogJump>();
                break;
        }
    }

    void Update()
    {
        // Проверка, открыт ли порт, и чтение данных, если они доступны
        if (serialPort.IsOpen)
        {
            try
            {
                // Чтение строки данных из порта
                string message = serialPort.ReadLine();
                script.OnSpacePressed();
                Debug.Log("Получено сообщение: " + message);
            }
            catch (System.TimeoutException)
            {
                // Обработка таймаута, если данных нет
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    script.OnSpacePressed();
        //}
    }

    void OnApplicationQuit()
    {
        // Закрытие порта при выходе из приложения
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
