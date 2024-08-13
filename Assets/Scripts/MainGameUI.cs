using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currentScoreText;
    //cursor settings
    [SerializeField] private Image _cursorNormal;
    [SerializeField] private Image _cursorFilled;
    [SerializeField] private float _cursorSize = 1.5f;
    private Vector2 _cursorPosition;

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
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.IsDashMode)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
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
