using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI welcomeText;

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
        welcomeText.text = "Hihi, " + playerName + "!";
    }
    
    //function to start a new game
    public void StartNew()
    {
        SceneManager.LoadScene("Main Game");
    }

    //Function to save the in-game data to JSON file when quiting the game
    public void Exit()
    {
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
        welcomeText.text = "Hihi, " + playerName + "!";
    }

   
   /* public void LoadPlayerName()
    {
        DataManager.Instance.LoadPlayerName();
    }*/

    public void GotoHighScoresScene()
    {
        SceneManager.LoadScene("High Scores");
    }
}
