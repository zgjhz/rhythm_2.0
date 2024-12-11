using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XCharts.Runtime;
using UnityEngine.UI; // ??? ????????????? UI ?????????
using TMPro;

public class DrawGraphWithXCharts : MonoBehaviour
{
    private string filePath = "path_to_your_csv_file.csv";
    public BaseChart chart;
    public List<string> gameTagList;
    private int chartIndex = -1;
    private int numGames = 0;
    public GameObject chartPanel;
    public string sceneName;
    public TMP_Dropdown dropdown;
    private string selectedOption;

    private Dictionary<string, List<float>> playerData;

    void Start()
    {
        numGames = gameTagList.Count;
        filePath = Path.Combine(Application.dataPath, "stats.csv");
        playerData = LoadPlayerData(filePath);
        chartPanel.SetActive(false);
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        selectedOption = dropdown.options[index].text;
        Debug.Log("Выбранный элемент: " + selectedOption);
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

    private Dictionary<string, List<float>> LoadPlayerData(string path)
    {
        Dictionary<string, List<float>> data = new Dictionary<string, List<float>>();

        using (StreamReader sr = new StreamReader(path))
        {
            string header = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                string[] parts = sr.ReadLine().Split(';');
                string playerName = parts[0];
                string scoreString = parts[7 + chartIndex];

                if (!float.TryParse(scoreString, out float score))
                    continue;

                if (!data.ContainsKey(playerName))
                    data[playerName] = new List<float>();

                data[playerName].Add(score);
            }
        }
        return data;
    }


    private void PlotGraph()
    {
        playerData = LoadPlayerData(filePath);
        string playerName = sceneName == "MainMenu" ? PlayerPrefs.GetString("current_user") : selectedOption;
        List<float> scores = new List<float>(0);
        if (playerData.ContainsKey(playerName))
        {
            scores = playerData[playerName];
        }
        chart.ClearData();

        var yAxis = chart.GetChartComponent<YAxis>();
        yAxis.minMaxType = Axis.AxisMinMaxType.Custom;
        yAxis.min = 0;
        yAxis.max = 100;

        var title = chart.EnsureChartComponent<Title>();
        title.text = gameTagList[chartIndex];
        switch(gameTagList[chartIndex]){
            case "Metronom": 
                title.text = "Метроном";
                break;
            case "Ritmamida": 
                title.text = "Ритмамида";
                break;
            case "YourRhythm": 
                title.text = "Твой Ритм";
                break;
            case "FrogGame": 
                title.text = "Ритмогушка";
                break;
            case "ArrowGame": 
                title.text = "Почтальон";
                break;
            case "Svetofor": 
                title.text = "Светофор";
                break;
        }
        
        chart.AddSerie<Line>(playerName);

        for (int i = 0; i < scores.Count; i++)
        {
            chart.AddXAxisData("Сессия" + (i + 1));
            chart.AddData(playerName, i, scores[i]);
        }
    }
}
