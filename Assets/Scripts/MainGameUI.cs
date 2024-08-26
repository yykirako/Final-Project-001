using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _primaryControl;
    [SerializeField] private TextMeshProUGUI _dashModeControl;
    [SerializeField] private GameObject _forceVisualiser;

    //cursor settings
    [SerializeField] private Image _cursorNormal;
    [SerializeField] private Image _cursorFilled;
    [SerializeField] private float _cursorSize = 1.5f;
    private Vector2 _cursorPosition;

    void Awake()
    {
        _currentScoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        //show current score
        if(DataManager.Instance != null)
        {
            _currentScoreText.text = "Score: " + DataManager.Instance.CurrentScore;

        }

        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.IsDashMode && !GameManager.Instance.IsGamePaused)
            {
                Cursor.visible = false;
                _primaryControl.gameObject.SetActive(false);
                _forceVisualiser.SetActive(false);
                _dashModeControl.gameObject.SetActive(true);
                
            }
            else
            {
                
                Cursor.visible = true;
                _primaryControl.gameObject.SetActive(true);
                _forceVisualiser.SetActive(true);
                _dashModeControl.gameObject.SetActive(false);
            }
        }
        _cursorPosition = Input.mousePosition;

    }

    public void SetCursorNormal()
    {
        _cursorNormal.gameObject.SetActive(true);
        _cursorFilled.gameObject.SetActive(false);

        _cursorNormal.transform.position = _cursorPosition;
        _cursorNormal.rectTransform.localScale = new Vector2(_cursorSize, _cursorSize);

    }
    public void SetCursorFilled()
    {
        _cursorFilled.gameObject.SetActive(true);
        _cursorNormal.gameObject.SetActive(false);

        _cursorFilled.transform.position = _cursorPosition;
        _cursorFilled.rectTransform.localScale = new Vector2(_cursorSize, _cursorSize);

    }
    public void HideCustomCursor()
    {
        _cursorFilled.gameObject.SetActive(false);
        _cursorNormal.gameObject.SetActive(false);
    }

}
