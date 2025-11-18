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
        bgmSlider.onValueChanged.AddListener((value)=>mixer.SetFloat("BGSound", bgmSlider.value));;
        sfxSlider.onValueChanged.AddListener((value)=>mixer.SetFloat("SFX", bgmSlider.value));;
        masterSlider.onValueChanged.AddListener((value)=>mixer.SetFloat("Master", bgmSlider.value));;
    }
}
