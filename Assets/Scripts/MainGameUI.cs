using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currentScoreText;
    // Start is called before the first frame update
    void Awake()
    {
        _currentScoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if(DataManager.Instance != null)
        {
            _currentScoreText.text = "Score: " + DataManager.Instance.CurrentScore;

        }

    }
}
