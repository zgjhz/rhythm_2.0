using System.IO.Ports;
using UnityEngine;

public class SerialPortReader : MonoBehaviour
{
    private SerialPort serialPort;
    public string portName = "COM3"; // �������� COM-�����, ��������, "COM3"
    public int baudRate = 9600; // �������� �������� ������
    public GameObject MainController;
    public string gameTag = "";

    private dynamic script;

    void Start()
    {
        // Попытка подключения к COM-порту с обработкой ошибок
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            serialPort.ReadTimeout = 1000; // Установка таймаута чтения
            Debug.Log("Успешное подключение к порту: " + portName);
        }
        catch (System.IO.IOException e)
        {
            Debug.LogError($"Ошибка подключения к порту {portName}: {e.Message}");
            serialPort = null; // Оставляем объект null, чтобы избежать вызовов в Update
        }
        catch (System.UnauthorizedAccessException e)
        {
            Debug.LogError($"Доступ к порту {portName} запрещён: {e.Message}");
            serialPort = null;
        }

        switch (gameTag)
        {
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
            case ("FrogGame"):
                script = MainController.GetComponent<FrogJump>();
                break;
        }
    }

    void Update()
    {
        // ��������, ������ �� ����, � ������ ������, ���� ��� ��������
        if (serialPort.IsOpen)
        {
            try
            {
                // ������ ������ ������ �� �����
                string message = serialPort.ReadLine();
                script.OnSpacePressed();
                Debug.Log("�������� ���������: " + message);
            }
            catch (System.TimeoutException)
            {
                // ��������� ��������, ���� ������ ���
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    script.OnSpacePressed();
        //}
    }

    void OnApplicationQuit()
    {
        // �������� ����� ��� ������ �� ����������
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
