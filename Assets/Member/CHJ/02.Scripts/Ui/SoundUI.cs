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
        bgmSlider.onValueChanged.AddListener((value)=>mixer.SetFloat("BGSound",  Mathf.Log10(value) * 20));
        sfxSlider.onValueChanged.AddListener((value)=>mixer.SetFloat("SFX", Mathf.Log10(value) * 20));
        masterSlider.onValueChanged.AddListener((value)=>mixer.SetFloat("Master", Mathf.Log10(value) * 20));
    }
}
