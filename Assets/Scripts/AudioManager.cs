using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    //audio setting
    [SerializeField] Slider _BGMVolumeSlider;
    [SerializeField] Slider _SEVolumeSlider;
    [SerializeField] GameObject _BGMPlayer;
    private AudioSource[] _BGM_List;
    private  AudioSource _BGM_NormalMode;
    private AudioSource _BGM_DashMode;

    public AudioMixer AudioMixer;

    private void Awake()
    {
        //dont create multiple instances
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (_BGMPlayer != null)
        {
            _BGM_List = _BGMPlayer.GetComponentsInChildren<AudioSource>();
            Debug.Log("AudioManager BGM list: "+ _BGM_List);
            _BGM_NormalMode = _BGM_List[0];
            _BGM_DashMode = _BGM_List[1];
        }
    }
    private void Update()
    {
        
        
    }
    public IEnumerator PlayNormalBGM()
    {
        if(_BGM_NormalMode != null)
        {
            Debug.Log("AudioManager: play normal bgm");
            yield return new WaitForSeconds(0.5f);

            _BGM_DashMode.Stop();
            yield return new WaitForSeconds(1.0f);
            _BGM_NormalMode.Play();
        }
    }
    public IEnumerator PlayDashModeBGM()
    {
        if(_BGM_DashMode != null)
        {
            Debug.Log("AudioManager: play normal bgm");
            yield return new WaitForSeconds(0.5f);

            _BGM_NormalMode.Stop();
            yield return new WaitForSeconds(1.0f);
            _BGM_DashMode.Play();
        }
    }
    public void ChangeBGM_Vol(float vol)
    {
        AudioMixer.SetFloat("BGM_Vol", vol);
    }
    public void ChangeSE_Vol(float vol)
    {
        AudioMixer.SetFloat("SE_Vol", vol);

    }
}
