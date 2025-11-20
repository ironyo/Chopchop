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
    }

    private void ChangeVolume(string id, float value)
    {
        if(value <= 0.001f)
            mixer.SetFloat(id, -80f);
        else
            mixer.SetFloat(id, Mathf.Log10(value) * 20);
    }
}
