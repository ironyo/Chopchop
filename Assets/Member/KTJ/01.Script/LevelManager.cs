using System;
using Member.CHJ._02.Scripts;
using System.Collections;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private TextMeshProUGUI LevelTxt;
    [SerializeField] private TextMeshProUGUI ShadowLevelTxt;
    [SerializeField] private Slider expSlider;
    [SerializeField] private WeaponDataListSO weaponDataList;

    private int _weaponIndex = 0;
    private int _level;
    public int Level { get { return _level; } private set { _level = Mathf.Clamp(value, 1, 5); } }

    public void IncreseLevel(int amount)
    {
        expSlider.value += amount;

        if (expSlider.value >= 100)
        {
            _weaponIndex++;
            MinionSetWeapon();
            
            int leftAmount = ((int)expSlider.value + amount) - 100;
            Level += 1;
            expSlider.value = leftAmount;
            UpdateUI();
            return;
        }
    }

    public void MinionSetWeapon()
    {
        foreach (var m in MinionManager.Instance.minionList)
        {
            WeaponHolder wh = m.GetComponent<TestMinion>().weaponHolder;
            wh.weaponData = weaponDataList.list[_weaponIndex];
            wh.SetWeapon();
        }
        Debug.Log("Minion Set");
    }

    private void UpdateUI()
    {
        LevelTxt.text = Level.ToString();
        ShadowLevelTxt.text = Level.ToString();
        NotifictionManager.Instance.NotifictionEvent.Invoke(Level.ToString() + " ������!", "�����ϼ̳׿�!");
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