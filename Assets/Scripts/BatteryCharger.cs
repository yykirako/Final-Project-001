using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryCharger : MonoBehaviour
{
    [SerializeField] private Image batteryFiller;
    [SerializeField] private Image batteryShiner;
    [SerializeField] private float chargingThreshold = 50f; //scores this amount to fully charge the battery
    private int _chargingStartingScore = 0;
    [SerializeField] private float _dashModeTime = 10f;
    private float _countDownSeconds;

    [SerializeField] private PlayerController _playerController;

    void Awake()
    {
        batteryShiner.gameObject.SetActive(false);

        if (batteryFiller == null)
        {
            Debug.LogError("Battery image not assigned.");
            return;
        }
        batteryFiller.fillAmount = 0f;
        _countDownSeconds = _dashModeTime;
    }

    void Update()
    {
        if (GameManager.Instance.IsBatteryCharging)
        {
            if (!GameManager.Instance.IsDashMode)
            {
                batteryFiller.fillAmount = Mathf.Clamp01((DataManager.Instance.CurrentScore - _chargingStartingScore)/ chargingThreshold);

                if (batteryFiller.fillAmount >= 1f)
                {
                    batteryShiner.gameObject.SetActive(true);
                    GameManager.Instance.IsBatteryCharging = false;
                    GameManager.Instance.IsBatteryCharged = true;
                    Debug.Log("Battery fully charged!");
                }
            }  
        }
        if(GameManager.Instance.IsBatteryCharged && GameManager.Instance.IsDashMode)
        {
            GameManager.Instance.IsBatteryCharging = true;
            //battery dies out 
            _countDownSeconds -= Time.deltaTime;
            batteryFiller.fillAmount = Mathf.Clamp01(_countDownSeconds / _dashModeTime);

            if ( _countDownSeconds <= 0f)
            {
                batteryFiller.fillAmount = 0;
                batteryShiner.gameObject.SetActive(false);

                GameManager.Instance.IsDashMode = false;
                GameManager.Instance.IsBatteryCharged = false;
                //reset screen
                _playerController.DashModeOut();
                _countDownSeconds = _dashModeTime;
                _chargingStartingScore = DataManager.Instance.CurrentScore;
                
            }
        }

    }
}
