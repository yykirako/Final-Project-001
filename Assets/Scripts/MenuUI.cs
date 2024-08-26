using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI startButton;

    private TMP_InputField playerNameInputField;
    //temp field to save name input
    private string playerName;

    

    void Start()
    {
        //Set name field on start
        playerNameInputField = GetComponentInChildren<TMP_InputField>();
        playerName = DataManager.Instance.PlayerName;

        playerNameInputField.text = playerName;

        //Display welcome text
        titleText.text = "Hihi, " + playerName + "!";
        
    }

    private void Update()
    {
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.IsGamePaused)
            {
                titleText.text = "Game is paused.";
                startButton.text = "Continue";
            }
            if (GameManager.Instance.IsGameOver)
            {
                titleText.text = "Game over :( Your score: " + DataManager.Instance.CurrentScore;
                titleText.color = Color.red;
                startButton.text = "Restart";
            }
        }
    }

    public void StartGame()
    {
        Debug.Log("(Re)Start/resume game button onclick - MenuUI:StartGame()");
        if(!GameManager.Instance.IsGameStarted)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            if (GameManager.Instance.IsGamePaused)
            {
                GameManager.Instance.ResumeGame();
            }
            if (GameManager.Instance.IsGameOver)
            {
                GameManager.Instance.RestartGame();
            }
        }
        
    }
    //Function to save the in-game data to JSON file when quiting the game
    public void Exit()
    {
        Debug.Log("Quit game button onclick - MenuUI:Exit()");

        DataManager.Instance.SavetoJson();
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                                Application.Quit(); // original code to quit Unity player
        #endif
    }

    //funtion to save name input to the in-game database
    public void SavePlayerName()
    {
        Debug.Log("Save button onclick - MenuUI:SavePlayerName()");

        if (playerNameInputField.text.Length > 0)
        {
            playerName = playerNameInputField.text;
        }
        else
        {
            playerNameInputField.text = "Anonymous player";
            playerName = "Anonymous player";
        }
        //update the name in the database
        DataManager.Instance.PlayerName = playerName;
        //update the name in the temp placeholder
        DataManager.Instance.SavePlayerName();
        //update the welcoming text
        titleText.text = "Hihi, " + playerName + "!";
    }

}
