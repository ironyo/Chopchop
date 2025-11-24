using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataListSO", menuName = "SO/EnemyDataListSO")]
public class EnemyDataListSO : ScriptableObject
{
    public List<EnemyDataSO> list;
}
