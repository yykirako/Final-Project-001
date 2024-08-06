using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoresDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoresText;
    
    //temp placeholder for the high scores
    private List<DataManager.HighScore> highScores;
    //maximum high score records
    private readonly int _highScoresCount = 10;

    private bool _isHighScoresDisplayed = false;

    // Start is called before the first frame update
    void Awake()
    {
        DisplayHighScores();
    }

    private void Update()
    {
        if (!_isHighScoresDisplayed) //in case DataManager didn't instantiate before this component
        {
            DisplayHighScores();
            _isHighScoresDisplayed=true;
        }
        if(GameManager.Instance.IsGamePaused || GameManager.Instance.IsGameOver)
        {
            DisplayHighScores();
            _isHighScoresDisplayed = true;

        }
    }
    private void DisplayHighScores()
    {
        if(DataManager.Instance != null)
        {
            highScores = DataManager.Instance.HighScores;
            highScoresText.text = "";
            for (int i = 1; i <= _highScoresCount; i++)
            {
                highScoresText.text += i + ". " + highScores[i - 1].Name + " " + highScores[i - 1].Score + "\n";
                /*Debug.Log(i + ". " + highScores[i - 1].Name + " " + highScores[i - 1].Score + "\n");*/
            }
            _isHighScoresDisplayed = true;

        }

    }
}
