/*This script manage the most essential functionalities of the game, including Start/Restart/Pause/Quit the game, keep record of the game score/game status, etc.*/

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //important public accessible properties declared here
    public static GameManager Instance;
    public GameObject Container;
    public GameObject[] FruitPrefabs;
    public bool IsFruitMerging = false;

    public bool GameOver = false;
    
    //
    private GameObject _fruitMerged;

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
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


    private void ExitGame()
    {
        DataManager.Instance.SaveHighScore();
        DataManager.Instance.SavetoJson();
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                                Application.Quit(); // original code to quit Unity player
        #endif
    }
}
