/*This script file reads and writes to the JSON file for keeping key data  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    //create a static singleton for data management, to be persistent and accessible across scenes
    public static DataManager Instance;

    //The public properties below are used to show on game menu / other text displays
    //the current player's name
    public string PlayerName;
    //the current in-game score
    public int CurrentScore;
    //the high scores
    public List<HighScore> HighScores;

    //in-game database to be saved to JSON
    SaveData data = new SaveData();

    //the in-game data contains the current player name and a list of high scores
    [System.Serializable]
    class SaveData
    {
        public string PlayerName;
        public List<HighScore> HighScores = new List<HighScore>();
    }

    //high score record is made up by a name and a score
    [System.Serializable]
    public class HighScore
    {
        public string Name;
        public int Score;
    }

    private void Awake()
    {
        //dont create multiple instances
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //load the data from JSON file upon awaking
        data = LoadData();
        Debug.Log("High scores on awake: " + data.HighScores);

        //store JSON data to temporary proprieties
        LoadPlayerName();
        LoadHighScores();
    }

    //this function reads the JSON file and saves it to in-game usable data
    private SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        data.PlayerName ??= "Anonymous player";
        data.HighScores ??= new List<HighScore>();

        return data;
    }

    //this function writes to the JSON file
    public void SavetoJson()
    {
        Debug.Log("DataManager: SaveToJson: Data to be saved: " + data);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Json saved: " + json);
    }

    //this function updates the current player name placeholder
    public void SavePlayerName()
    {
        data.PlayerName = PlayerName;
        Debug.Log("Name saved. " + PlayerName);
    }

    //this function reads the current player name from the in-game database to the temp placeholder
    public void LoadPlayerName()
    {
        if (data.PlayerName != null)
        {
            PlayerName = data.PlayerName;
            Debug.Log("Loaded player name: " + PlayerName);
        }
        else
        {
            PlayerName = "Anonymous player";
        }
    }

    //this function loads the highscores (max 10)
    private void LoadHighScores()
    {
        if (data.HighScores.Count > 0)
        {
            HighScores = data.HighScores;
            Debug.Log("Loaded HighScores; Lowest best Score: " + HighScores[9]);
        }
        else
        {
            Debug.Log("LoadHighScores: no high scores");
            for (int i = 0; i < 10; i++)
            {
                HighScores.Insert(0, new HighScore());
                HighScores[0].Name = "Name";
                HighScores[0].Score = 0;
            }
            Debug.Log("LoadHighScores: added placeholders to database. Number of placeholder: " + HighScores.Count);
            data.HighScores = HighScores;
        }
    }

    //this function saves a new high score after game-over
    public void SaveHighScore()
    {
        for (int i = 0; i < HighScores.Count; i++)
        {
            if (CurrentScore > HighScores[i].Score)
            {
                HighScores.Insert(i, new HighScore());
                HighScores[i].Name = PlayerName;
                HighScores[i].Score = CurrentScore;

                HighScores.RemoveAt(HighScores.Count - 1); //remove the lowest score in the list
                break;
            }
        }
        Debug.Log("DataManager: SaveHighScore(): score and name saved " + PlayerName + " " + CurrentScore);

        data.HighScores = HighScores;
        SavetoJson();

        Debug.Log("HighScores Saved; Best Score: " + data.HighScores[0]);
    }
}

