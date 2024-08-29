/*This script manage the most essential functionalities of the game, including Start/Restart/Pause/Quit the game, keep record of the game score/game status, etc.*/

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    //important public accessible properties
    //fruits status
    public GameObject Container;
    public GameObject[] FruitPrefabs;
    public bool IsFruitMerging = false;
    private GameObject _fruitMerged;
    [SerializeField] private AudioSource _SE_fruitMerge;
    [SerializeField] private AudioSource _SE_fruitDrop;

    //game status
    public bool IsGameOver = false;
    public bool IsGameOverCountingDown = false;
    public bool IsGameStarted = false;
    public bool IsGamePaused = false;
    public bool IsBatteryCharging = true;
    public bool IsBatteryCharged = false; //=is dash mode ready to launch
    public bool IsDashMode = false;
    [SerializeField] public bool IsDarkMode = false;

    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button restartButton;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject cameraPosition;



    // Start is called before the first frame update
    void Awake()
    {
        //dont create multiple instances
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //display menu but not the main game at the start
        mainGame.SetActive(false);
        menuUI.SetActive(true);

    }

    private void Update()
    {
        //Esc key to pause/resume the game
        if (IsGameStarted &&
            !IsGameOver &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void MergeFruit(int fruitIndex, Vector3 mergePosition)
    {
     
            IsFruitMerging = true;
            /*Debug.Log("fruit merging");*/
            _fruitMerged = Instantiate(FruitPrefabs[fruitIndex + 1], mergePosition, FruitPrefabs[fruitIndex + 1].transform.rotation, Container.transform);

            if(DataManager.Instance != null)
            {
                DataManager.Instance.CurrentScore += (fruitIndex + 1);

            if (_SE_fruitMerge != null)
            {
                _SE_fruitMerge.Play();

            }
            StartCoroutine(ScoreColorGreen());
            }

        //make sure the new fruit object and children have the correct layer setting
        _fruitMerged.layer = 6;
            foreach (Transform child in _fruitMerged.transform)
            {
                child.gameObject.layer = 6;
            }
    }

    public void DropFruit()//losing score and play an SE when a fruit is dropped
    {
        if(_SE_fruitDrop != null)
        {
            _SE_fruitDrop.Play();

        }

        StartCoroutine(GameManager.Instance.ScoreColorRed());

        DataManager.Instance.CurrentScore -= 5;
    }

    public void StartGame()
    {
        Debug.Log("GameManager: game is started.");

        IsGameStarted = true;
        mainGame.SetActive(true);
        menuUI.SetActive(false);
        restartButton.gameObject.SetActive(false);


    }
    private void PauseGame()
    {
        Debug.Log("GameManager: game is paused.");

        Time.timeScale = 0.0f;
        IsGamePaused = true;
        mainGame.SetActive(false);
        menuUI.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Debug.Log("GameManager: game is resumed.");
        
        Time.timeScale = 1.0f;
        IsGamePaused = false;
        mainGame.SetActive(true);
        menuUI.SetActive(false);
        restartButton.gameObject.SetActive(false);

    }

    public void GameOver()
    {
        Debug.Log("GameManager: game over.");

        IsGameOver = true;
        Time.timeScale = 0.0f;

        mainGame.SetActive(false);
        menuUI.SetActive(true);
        restartButton.gameObject.SetActive(false);


        //note the game scores
        DataManager.Instance.SaveHighScore();
        DataManager.Instance.SavetoJson();

    }
    public void RestartGame()
    {
        Debug.Log("GameManager: game is restarted.");
        //reset everything
        DataManager.Instance.CurrentScore = 0;
        foreach (Transform child in Container.transform)
        {
            if (!child.gameObject.CompareTag("Container"))
            {
                Destroy(child.gameObject);
            }
        }

        Time.timeScale = 1.0f;
        mainGame.SetActive(true);
        menuUI.SetActive(false);
        restartButton.gameObject.SetActive(false);


        mainCamera.transform.position = cameraPosition.transform.position;

        IsGameOver = false;
        IsGamePaused = false;
        IsBatteryCharging = true;
        IsBatteryCharged = false; //=is dash mode ready to launch
        IsDashMode = false;

}
    public void ExitGame()
    {
        if(DataManager.Instance != null)
        {
            DataManager.Instance.SaveHighScore();
            DataManager.Instance.SavetoJson();
        }
        Debug.Log("GameManager: game is quited.");

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBGL
        // Do nothing in WebGL builds
        Debug.Log("Quit button pressed in WebGL mode. No action taken.");
#elif UNITY_STANDALONE
            Application.Quit(); 

#endif
    }

    private IEnumerator ScoreColorRed()//score turn red when losing points
    {
        if (scoreText != null)
        {
            Debug.Log("Score dropped");
            
            scoreText.color = Color.red;
            yield return new WaitForSeconds(1f);
            scoreText.color = Color.white;
        }
    }
    private IEnumerator ScoreColorGreen()//score turn green when gaining points
    {
        if (scoreText != null)
        {
            Debug.Log("Score gained");

            scoreText.color = Color.green;
            yield return new WaitForSeconds(1f);
            scoreText.color = Color.white;
        }
    }


}
