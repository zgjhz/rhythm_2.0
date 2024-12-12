using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.IO;

public class UIContrloller : MonoBehaviour
{

    private SerialPort serialPort;
    private string portName;
    public int baudRate = 38400;
    public TMP_Dropdown comportDropdown;
    string[] ports = SerialPort.GetPortNames();
    private bool isPortOpened = false;

    public Image statusImage;
    public Sprite disconnected;
    public Sprite connected;
    public Sprite connecting;

    public void OnMainButtonClick(string sceneName) {
        SceneManager.LoadScene(sceneName);
        if (isPortOpened)
        {
            serialPort.Close();
        }
    }

    public void OnexitButtonClick() {
        Application.Quit();
    }

    void UpdateDropdownOptions()
    {
        // Получаем доступные порты
        string[] ports = SerialPort.GetPortNames();

        Debug.Log(ports.Length);

        // Очищаем текущие опции Dropdown
        comportDropdown.ClearOptions();

        // Добавляем найденные порты в Dropdown
        if (ports.Length > 0)
        {
            comportDropdown.AddOptions(new System.Collections.Generic.List<string>(ports));
            Debug.Log("Порты добавлены в Dropdown.");
        }
        else
        {
            comportDropdown.AddOptions(new System.Collections.Generic.List<string> { "Нет доступных портов" });
            Debug.Log("Доступных портов не найдено.");
        }
    }

    void ConnectToSelectedPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Соединение с предыдущим портом закрыто.");
        }

        string selectedPort = comportDropdown.options[comportDropdown.value].text;
        PlayerPrefs.SetString("Comport", selectedPort);
        PlayerPrefs.Save();
        if (selectedPort == "Нет доступных портов")
        {
            Debug.LogWarning("Выбран невалидный порт.");
            return;
        }

        try
        {
            serialPort = new SerialPort(selectedPort, baudRate);
            serialPort.Open();
            serialPort.ReadTimeout = 1000; // Установка таймаута чтения
            Debug.Log("Успешное подключение к порту: " + selectedPort);

            // Отправка текстового сообщения после подключения
            string messageToSend = "400,100,1000,2,0,3,*";
            serialPort.WriteLine(messageToSend);
            Debug.Log("Сообщение отправлено: " + messageToSend);

            statusImage.sprite = connected;
            isPortOpened = true;
        }
        catch (System.IO.IOException e)
        {
            Debug.LogError($"Ошибка подключения к порту {selectedPort}: {e.Message}");
            serialPort = null; // Оставляем объект null, чтобы избежать вызовов в Update
            statusImage.sprite = disconnected;
        }
        catch (System.UnauthorizedAccessException e)
        {
            Debug.LogError($"Доступ к порту {selectedPort} запрещён: {e.Message}");
            serialPort = null;
            statusImage.sprite = disconnected;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateDropdownOptions();
        comportDropdown.onValueChanged.AddListener(delegate { ConnectToSelectedPort(); });
        statusImage.sprite = connecting;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
