using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryCharger : MonoBehaviour
{
    [SerializeField] private Image batteryImage;
    [SerializeField] private float chargingThreshold = 50f; //scores this amount to fully charge the battery

    void Start()
    {
        if (batteryImage == null)
        {
            Debug.LogError("Battery image not assigned.");
            return;
        }

        batteryImage.fillAmount = 0f;
    }

    void Update()
    {
        if (GameManager.Instance.IsBatteryCharging)
        {
            if (!GameManager.Instance.IsDashMode)
            {
                batteryImage.fillAmount = Mathf.Clamp01(DataManager.Instance.CurrentScore / chargingThreshold);

                if (batteryImage.fillAmount >= 1f)
                {
                    GameManager.Instance.IsBatteryCharging = false;
                    GameManager.Instance.IsBatteryCharged = true;
                    Debug.Log("Battery fully charged!");
                }
            }
            
        }
    }
}
