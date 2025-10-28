using System;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Member.CHJ._02.Scripts.Action
{
    public class MinionMovementManager
    {
        public Vector2 RandomPatrol()
        {
            Vector2 pos= PatrolSiteManager.Instance.patrolSite
                [Random.Range(0, PatrolSiteManager.Instance.patrolSite.Count)].transform.position;
            return pos + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)); // Patrol
        }

        public bool IsCanEnterBuilding(Building building)
        {
            return building.NowMinion <= building.buildingSO.maxLevel;
            // 현재 미니언 수 <= 최대 미니언 수
            // 즉, true 때 가능.
        }
        
    }
}