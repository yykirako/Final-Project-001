using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryCharger : MonoBehaviour
{
    [SerializeField] private Image batteryImage;
    [SerializeField] private float chargingThreshold = 50f;
    private bool _isCharging = true;

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
        if (_isCharging)
        {
            batteryImage.fillAmount = Mathf.Clamp01(DataManager.Instance.CurrentScore / chargingThreshold);

            if (batteryImage.fillAmount >= 1f)
            {
                _isCharging = false;
                Debug.Log("Battery fully charged!");
            }
        }
    }

    public void StartCharging()
    {
        _isCharging = true;
        batteryImage.fillAmount = 0f;
    }

    public void StopCharging()
    {
        _isCharging = false;
    }
}
