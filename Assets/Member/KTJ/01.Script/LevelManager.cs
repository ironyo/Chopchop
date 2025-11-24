using Member.CHJ._02.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private TextMeshProUGUI LevelTxt;
    [SerializeField] private TextMeshProUGUI ShadowLevelTxt;
    [SerializeField] private Slider expSlider;

    private int _level;
    public int Level { get { return _level; } private set { _level = Mathf.Clamp(value, 1, 5); } }

    public void IncreseLevel(int amount)
    {
        expSlider.value += amount;

        if (expSlider.value >= 100) // 슬라이더 꽉차면 레벨증가
        {
            int leftAmount = ((int)expSlider.value + amount) - 100;
            Level += 1;
            expSlider.value = leftAmount;
            UpdateUI();
            return;
        }
    }

    private void UpdateUI()
    {
        LevelTxt.text = Level.ToString();
        ShadowLevelTxt.text = Level.ToString();
        NotifictionManager.Instance.NotifictionEvent.Invoke(Level.ToString() + " 레벨업!", "성장하셨네요!");
    }


    //private void Start()
    //{
    //    StartCoroutine(AA());
    //}
    //IEnumerator AA()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    IncreseLevel(2);
    //    StartCoroutine(AA());
    //}
}
