using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine;

// Интерфейс для обработки события "SpacePressed"
public interface ISpacePressHandler
{
    void OnSpacePressed();
}

public class SerialPortReader : MonoBehaviour
{
    private SerialPort serialPort; // Работа с COM-портом
    private string portName; // Имя порта (например, "COM3")
    public int baudRate = 38400; // Скорость передачи данных (бит/с)
    public GameObject MainController; // Главный объект для управления
    public string gameTag = ""; // Тег для определения типа игры

    private ISpacePressHandler script; // Интерфейс для обработки нажатия Space
    private Thread serialThread; // Поток для работы с COM-портом
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>(); // Очередь сообщений
    private bool isRunning = true; // Флаг для работы потока
    public MenuManager menuManager;

    void Start()
    {
        // Попытка подключения к COM-порту
        portName = PlayerPrefs.GetString("Comport");
        try
        {
            serialPort = new SerialPort(portName, baudRate)
            {
                ReadTimeout = 1000 // Таймаут для чтения данных
            };
            serialPort.Open();
            Debug.Log("Успешное подключение к порту: " + portName);

            // Запуск потока для работы с COM-портом
            serialThread = new Thread(ReadFromPort)
            {
                IsBackground = true // Фоновый поток
            };
            serialThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка подключения к порту {portName}: {e.Message}");
            serialPort = null; // Оставляем null, чтобы избежать вызовов
        }

        script = MainController.GetComponent<ISpacePressHandler>();

        if (script == null)
        {
            Debug.LogError($"Объект {MainController.name} не содержит компонента, реализующего ISpacePressHandler.");
        }
    }

    void Update()
    {
        // Обрабатываем сообщения из очереди
        while (messageQueue.TryDequeue(out string message))
        {
            Debug.Log("Получено сообщение: " + message);
            script?.OnSpacePressed(); // Вызов метода через интерфейс
            menuManager.OnSpacePressed();
        }
    }

    private void ReadFromPort()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string message = serialPort.ReadLine();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    messageQueue.Enqueue(message); // Добавление сообщения в очередь
                }
            }
            catch (System.TimeoutException)
            {
                // Таймаут чтения — ничего не делаем
            }
            catch (System.Exception e)
            {
                Debug.LogError("Ошибка чтения из порта: " + e.Message);
                isRunning = false; // Останавливаем поток в случае ошибки
            }
        }
    }

    public void OnApplicationQuitSuka()
    {
        // Остановка потока
        isRunning = false;
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Join();
        }

        // Закрытие порта
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Порт закрыт.");
        }
    }
}
