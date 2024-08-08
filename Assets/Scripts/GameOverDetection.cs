using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameOverDetection : MonoBehaviour
{
    private bool _isColliding = false;

    [SerializeField] private float _timer;
    //seconds to save oneself before game over
    [SerializeField] float gameOverBuffer = 5f;

    [SerializeField] private AudioSource _alertSE;

    private void Awake()
    {
        _timer = 0f;
    }
    private void Update()
    {
        if (_isColliding)
        {
            _timer += Time.deltaTime;

        }

        if (_timer > gameOverBuffer)
        {
            GameManager.Instance.IsGameOver = true;
            _alertSE.Stop();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ContainerTop: triggerEnter");
        if (other.gameObject.CompareTag("Watermelon")
            || other.gameObject.CompareTag("Pineapple")
            || other.gameObject.CompareTag("Melon")
            || other.gameObject.CompareTag("Apple")
            || other.gameObject.CompareTag("Orange")
            || other.gameObject.CompareTag("Lemon")
            || other.gameObject.CompareTag("Lime"))
        {
            if (!_isColliding && !GameManager.Instance.IsGameOver)
            {
                _isColliding = true;
                _alertSE.Play();

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ContainerTop: triggerExit");

        if (other.gameObject.CompareTag("Watermelon")
            || other.gameObject.CompareTag("Pineapple")
            || other.gameObject.CompareTag("Melon")
            || other.gameObject.CompareTag("Apple")
            || other.gameObject.CompareTag("Orange")
            || other.gameObject.CompareTag("Lemon")
            || other.gameObject.CompareTag("Lime"))
        {
            if (_isColliding && !GameManager.Instance.IsGameOver)
            {
                _alertSE.Pause();
                _isColliding = false;
                _timer = 0f;

            }
        }
    }

}
