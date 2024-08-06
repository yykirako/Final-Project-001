/*This script manage the most essential functionalities of the game, including Start/Restart/Pause/Quit the game, keep record of the game score/game status, etc.*/

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //important public accessible properties declared here
    public static GameManager Instance;

    //fruits status
    public GameObject Container;
    public GameObject[] FruitPrefabs;
    public bool IsFruitMerging = false;
    private GameObject _fruitMerged;

    //game status
    public bool IsGameOver = false;
    public bool IsGameStarted = false;
    public bool IsGamePaused = false;
    /*[SerializeField] private GameObject darkBackground;*/
    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject menuUI;

    private int _currentScore;


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
        
    }

    private void Update()
    {
        //Esc key to pause the game
        if(IsGameStarted &&
            !IsGamePaused &&
            !IsGameOver &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            /*ExitGame();*/
            IsGamePaused = true;
        }

        if (!IsGameStarted)
        {
            mainGame.SetActive(false);
            menuUI.SetActive(true);
            
        }else{       
            if (!IsGamePaused && !IsGameOver)
            {
                mainGame.SetActive(true);
                menuUI.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    IsGamePaused = true;
                }
            }
            if (IsGamePaused || IsGameOver)
            {
                mainGame.SetActive(false);
                menuUI.SetActive(true);
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
            }

        //make sure the new fruit object and children have the correct layer setting
        _fruitMerged.layer = 6;
            foreach (Transform child in _fruitMerged.transform)
            {
                child.gameObject.layer = 6;
            }

    }


    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        IsGamePaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        IsGamePaused = false;
    }

    public void RestartGame()
    {
        DataManager.Instance.SaveHighScore();
        DataManager.Instance.SavetoJson();


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
        IsGameOver = false;
    }
    public void ExitGame()
    {
        if(DataManager.Instance != null)
        {
            DataManager.Instance.SaveHighScore();
            DataManager.Instance.SavetoJson();
        }
        
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                                Application.Quit(); // original code to quit Unity player
        #endif
    }
}
