using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDetection : MonoBehaviour
{
    private bool _isColliding = false;
    [SerializeField] private AudioSource _alertSE;
    
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
                /*_alertSE.Pause();*/
                GameManager.Instance.IsGameOver = true;
                _isColliding = false;

            }
        }
    }

}
