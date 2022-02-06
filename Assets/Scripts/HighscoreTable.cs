using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Struct for time
[System.Serializable]
public struct TimeRes
{
    public float _seconds;

    // Converts all time to seconds
    public float GetSeconds()
    {
        return _seconds;
    }
    // Returns formatted strig
    public string ConvertToText()
    {
        return ((int)_seconds / 60).ToString("D2") + ":" + (((int)_seconds) % 60).ToString("D2");
    }
    // Resets time to zero
    public void ResetTime()
    {
        _seconds = 0;
    }
}

// Represents a single High score entry
[System.Serializable]
public class HighscoreEntry
{
    public int score;
    public TimeRes time;
    public string name;
}

// Highscores
public class Highscores
{
    public Highscores() { highscoreEntries = new List<HighscoreEntry>(); }
    public List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();
}

public class HighscoreTable : MonoBehaviour
{
    // JSON save preference
    private static string JSON_SAVE_PREFERENCE = "highscoreTable";
    // Max amount of rows in highscore table
    private const int MAX_ROW_AMOUNT = 7;
    // Height offest
    private const float HEIGHT_OFFSET = 15;

    // Column position
    [SerializeField]
    private Transform _columnPosition;
    // Template position
    [SerializeField]
    private Transform _templatePosition;

    // Entry container
    [SerializeField]
    private Transform _entryContainer;
    // Entry template
    [SerializeField]
    private Transform _entryTemplate;

    // Highscore lists
    Highscores _highscores;
    private List<Transform> _highscoreEntryTransformList;

    // Awake
    private void Awake()
    {
        // Hide entry template
        _entryTemplate.gameObject.SetActive(false);

        // Instantiate highscores
        _highscores = new Highscores();

        // Load data
        LoadData();

        // Sorting by coefficient ( coefficient = score / time )
        for (int i = 0; i < _highscores.highscoreEntries.Count; i++)
        {
            for (int j = i + 1; j < _highscores.highscoreEntries.Count; j++)
            {
                if (_highscores.highscoreEntries[j].score / _highscores.highscoreEntries[j].time.GetSeconds()
                    > _highscores.highscoreEntries[i].score / _highscores.highscoreEntries[i].time.GetSeconds())
                {
                    //Swap
                    HighscoreEntry tmp = _highscores.highscoreEntries[i];
                    _highscores.highscoreEntries[i] = _highscores.highscoreEntries[j];
                    _highscores.highscoreEntries[j] = tmp;
                }
            }
        }

        // Delete rows from the end, until there will only be maximum amount of available
        while(_highscores.highscoreEntries.Count > MAX_ROW_AMOUNT) 
        { _highscores.highscoreEntries.RemoveAt(_highscores.highscoreEntries.Count - 1); }

        // Create highscore list
        _highscoreEntryTransformList = new List<Transform>();
        foreach(HighscoreEntry highscoreEntry in _highscores.highscoreEntries)
        {
            CreateHighscoreEntryTransform(highscoreEntry, _entryContainer, _highscoreEntryTransformList);
        }
    }

    private void OnDestroy()
    {
        SaveData();
    }

    // Saves data
    private void SaveData()
    {
        string json = JsonUtility.ToJson(_highscores);
        PlayerPrefs.SetString(JSON_SAVE_PREFERENCE, json);
        PlayerPrefs.Save();
    }

    // Load data
    private void LoadData()
    {
        string jsonString = PlayerPrefs.GetString(JSON_SAVE_PREFERENCE);
        if (String.IsNullOrEmpty(jsonString)) { return; }
        _highscores = JsonUtility.FromJson<Highscores>(jsonString);
    }

    // Adds new highscore entry
    public static void AddHighscoreEntry(HighscoreEntry newHighscoreEntry)
    {
        // Load data
        string jsonString = PlayerPrefs.GetString(JSON_SAVE_PREFERENCE);
        Highscores highscores = new Highscores();
        if (!String.IsNullOrEmpty(jsonString))
        {
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }
        // Add new high score entry
        highscores.highscoreEntries.Add(newHighscoreEntry);
        // Save data
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(JSON_SAVE_PREFERENCE, json);
        PlayerPrefs.Save();
    }

    // Creates highscore entry
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry,
        Transform container, List<Transform> transformList)
    {
        // Get template height offset
        float templateHeightOffset = HEIGHT_OFFSET;

        // Instantiate template instance
        Transform entryTransform = Instantiate(_entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeightOffset * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        // Rank forming
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("PosColTemplate").GetComponent<Text>().text = rankString;
        entryTransform.Find("CheckpointColTemplate").GetComponent<Text>().text = highscoreEntry.score.ToString();
        entryTransform.Find("TimeColTemplate").GetComponent<Text>().text = highscoreEntry.time.ConvertToText();
        entryTransform.Find("NameColTemplate").GetComponent<Text>().text = highscoreEntry.name;

        transformList.Add(entryTransform);
    }
}
