using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionData", menuName = "So/Minion/MinionData")]
public class MinionData : ScriptableObject
{
    public List<Transform> patrolSite;
}
