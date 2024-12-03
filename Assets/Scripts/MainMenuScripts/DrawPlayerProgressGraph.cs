using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XCharts.Runtime;
using UnityEngine.UI; // Для использования UI элементов
using TMPro;

public class DrawGraphWithXCharts : MonoBehaviour
{
    private string filePath = "path_to_your_csv_file.csv"; // Путь к файлу
    public BaseChart chart; // Ссылка на объект графика из сцены
    public MainMenuScript mainMenuScript;
    public List<string> gameTagList;
    private int chartIndex = -1;
    private int numGames = 0;
    public GameObject chartPanel;

    private Dictionary<string, List<float>> playerData;

    void Start()
    {
        numGames = gameTagList.Count;
        filePath = @"Z:\Unity\rhythm_2.0\Assets\stats.csv";
        // Загружаем данные из CSV
        playerData = LoadPlayerData(filePath);
    }

    public void ResetValues()
    {
        chartIndex = -1;
    }

    public void OnNextButtonClicked()
    {
        if (chartIndex < numGames - 1)
        {
            chartIndex++;
        }
        else
        {
            chartIndex = 0;
        }
        Debug.Log("chartIndex: " + chartIndex);
        if (chartIndex == 0)
        {
            OpenChartPanel();
        }
        else
        {
            PlotGraph();
        }
    }

    public void OnPreviousButtonClicked()
    {
        chartIndex--;
        if (chartIndex >= 0)
        {
            PlotGraph();
        }
        else
        {
            CloseChartPanel();
        }
        Debug.Log("chartIndex: " + chartIndex);
    }

    public void OpenChartPanel()
    {
        chartPanel.SetActive(true);
        PlotGraph();
    }

    public void CloseChartPanel()
    {
        chartPanel.SetActive(false);
    }

    // Метод для загрузки данных из CSV
    private Dictionary<string, List<float>> LoadPlayerData(string path)
    {
        Dictionary<string, List<float>> data = new Dictionary<string, List<float>>();

        using (StreamReader sr = new StreamReader(path))
        {
            // Пропускаем заголовок
            string header = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(';');
                string playerName = line[0]; // Первый столбец - имя игрока
                float score = float.Parse(line[6+chartIndex]); // Столбец YourRhythmPercentHits

                if (!data.ContainsKey(playerName))
                    data[playerName] = new List<float>();
                data[playerName].Add(score);
            }
        }
        return data;
    }
    
    // Метод для построения графика
    private void PlotGraph()
    {
        playerData = LoadPlayerData(filePath);
        string playerName = PlayerPrefs.GetString("current_user");
        List<float> scores = playerData[playerName];
        chart.ClearData(); // Очистить старые данные (если необходимо)

        var yAxis = chart.GetChartComponent<YAxis>();
        yAxis.minMaxType = Axis.AxisMinMaxType.Custom;
        yAxis.min = 0;
        yAxis.max = 100;

        var title = chart.EnsureChartComponent<Title>();
        Debug.Log(chartIndex);
        title.text = gameTagList[chartIndex];

        // Добавить серию (Series) для игрока
        chart.AddSerie<Line>(playerName);

        for (int i = 0; i < scores.Count; i++)
        {
            chart.AddXAxisData("Сессия" + (i + 1));
            chart.AddData(playerName, i, scores[i]); // Добавить точки в график
        }
    }
}
