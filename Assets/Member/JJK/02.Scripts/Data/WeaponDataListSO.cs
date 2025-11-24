using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataListSO", menuName = "SO/WeaponDataListSO")]
public class WeaponDataListSO : ScriptableObject
{
    public List<WeaponDataSO> list;
}
