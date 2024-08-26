using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameOverDetection : MonoBehaviour
{

    [SerializeField] private float _timer; //timer to record colliding time

    //seconds to save oneself before game over
    [SerializeField] float gameOverBuffer = 5f;

    [SerializeField] private AudioSource _alertSE;

    private void Awake()
    {
        _timer = 0f;
    }
    private void Update()
    {
        if (GameManager.Instance.IsGameOverCountingDown && !GameManager.Instance.IsDashMode)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _alertSE.Stop();
        }
        //game over when the fruits colliding with the detector for over [gameOverBuffer] seconds
        if (_timer > gameOverBuffer)
        {
            GameManager.Instance.GameOver();
            _alertSE.Stop();
            _timer = 0f;
            GameManager.Instance.IsGameOverCountingDown=false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*Debug.Log("ContainerTop: triggerEnter");*/
        if (other.gameObject.CompareTag("Watermelon")
            || other.gameObject.CompareTag("Pineapple")
            || other.gameObject.CompareTag("Melon")
            || other.gameObject.CompareTag("Apple")
            || other.gameObject.CompareTag("Orange")
            || other.gameObject.CompareTag("Lemon")
            || other.gameObject.CompareTag("Lime"))
        {
            if (!GameManager.Instance.IsGameOverCountingDown && !GameManager.Instance.IsGameOver)
            {
                GameManager.Instance.IsGameOverCountingDown = true;
                _alertSE.Play();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        /*Debug.Log("ContainerTop: triggerExit");*/

        if (other.gameObject.CompareTag("Watermelon")
            || other.gameObject.CompareTag("Pineapple")
            || other.gameObject.CompareTag("Melon")
            || other.gameObject.CompareTag("Apple")
            || other.gameObject.CompareTag("Orange")
            || other.gameObject.CompareTag("Lemon")
            || other.gameObject.CompareTag("Lime")
            || other.gameObject.CompareTag("Grape")
            || other.gameObject.CompareTag("Cherry"))
        {
            if (GameManager.Instance.IsGameOverCountingDown && !GameManager.Instance.IsGameOver)
            {
                _alertSE.Pause();
                GameManager.Instance.IsGameOverCountingDown = false;
                _timer = 0f;

            }
        }
    }

}
