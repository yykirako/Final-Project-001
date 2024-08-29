using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkModeManager : MonoBehaviour
{
    [SerializeField] private Light _main_Light;
    [SerializeField] private Light _sky_Reflection;
    [SerializeField] private GameObject _day_Sky;
    [SerializeField] private GameObject _night_Sky;


    public void DarkModeSwitch()
    {
        if (GameManager.Instance.IsDarkMode)
        {
            //set to standard mode
            _main_Light.intensity = 1;
            _sky_Reflection.gameObject.SetActive(true);
            _day_Sky.gameObject.SetActive(true);
            _night_Sky.gameObject.SetActive(false);
            GameManager.Instance.IsDarkMode = false;
        }
        else
        {
            //set to dark mode 

            _main_Light.intensity = 0.5f;
            _sky_Reflection.gameObject.SetActive(false);
            _day_Sky.gameObject.SetActive(false);
            _night_Sky.gameObject.SetActive(true);
            GameManager.Instance.IsDarkMode = true;

        }

    }
}
