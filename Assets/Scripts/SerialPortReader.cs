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
        // ������������� � �������� �����
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
        serialPort.ReadTimeout = 1000; // ������� ������ � �������������

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
