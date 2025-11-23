using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider bgmSlider;
    
    [SerializeField] private Slider sfxSlider;
    
    [SerializeField] private Slider masterSlider;
    
    private void Start()
    {
        bgmSlider.onValueChanged.AddListener((value)=>ChangeVolume("BGM", value));
        sfxSlider.onValueChanged.AddListener((value)=>ChangeVolume("SFX", value));
        masterSlider.onValueChanged.AddListener((value)=>ChangeVolume("Master", value));
        
        mixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM"));
        mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
        mixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
    }

    private void ChangeVolume(string id, float value)
    {
        float setValue = 0;
        if (value <= 0.001f)
            setValue = -80f;
        else
            setValue = Mathf.Log10(value) * 20;
        
        mixer.SetFloat(id, setValue);
        PlayerPrefs.SetFloat(id, setValue);
    }
}
