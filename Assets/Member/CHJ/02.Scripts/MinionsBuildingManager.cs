using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Member.CHJ._02.Scripts
{
    [Serializable]
    public class MinionsBuildingManager
    {
        public List<Building> Buildings = new List<Building>();
        private Building _buildingTarget;
        private WaitForSeconds _waitT = new WaitForSeconds(0.1f);
            

        public void AddBuilding(Building building)
        {
            if(!Buildings.Contains(building))
                Buildings.Add(building);
        }

        public bool IsBuilding(BuildingSO buildingSo)
        {
            foreach (var building in Buildings)
            {
                if(building.buildingSO == buildingSo)
                    return true;
            }
            return false;
        }
        public void RemoveBuilding(Building building)
        {
            if(Buildings.Contains(building))
                Buildings.Remove(building);
        }
        
        public Building GetAvailableHouseCheckOnly(Vector3 pos, BuildingSO type, float maxRange)
        {
            Building target = null;
            float minDist = float.MaxValue;

            Debug.Log($"[GetAvailableHouseCheckOnly] Target type = {(type == null ? "null" : type.name)}");

            foreach (var b in Buildings)
            {
                if (b == null)
                {
                    Debug.Log("[GetAvailableHouseCheckOnly] null building in list");
                    continue;
                }

                if (b.buildingSO == null)
                {
                    Debug.Log($"[GetAvailableHouseCheckOnly] {b.name} has NULL buildingSO");
                    continue;
                }

                Debug.Log($"[GetAvailableHouseCheckOnly] candidate {b.name}, so={b.buildingSO.name}");

                if (type == null)
                    continue;

                // ★ 이름 말고 참조로 비교 (더 안전)
                if (b.buildingSO != type)
                {
                    Debug.Log($"[GetAvailableHouseCheckOnly] skip {b.name} : {b.buildingSO.name} != {type.name}");
                    continue;
                }

                Debug.Log($"[GetAvailableHouseCheckOnly] TYPE MATCH {b.name}");

                if (!b.CanReserve())
                {
                    Debug.Log($"[GetAvailableHouseCheckOnly] {b.name} CanReserve == false");
                    continue;
                }

                Debug.Log($"[GetAvailableHouseCheckOnly] {b.name} CanReserve == true");

                float dist = Vector3.Distance(pos, b.transform.position);
                if (dist < minDist && dist <= maxRange)
                {
                    minDist = dist;
                    target = b;
                }
            }

            Debug.Log(target != null
                ? $"[GetAvailableHouseCheckOnly] FOUND {target.name}"
                : "[GetAvailableHouseCheckOnly] NOT FOUND");

            return target;
        }

        public bool TryEnterBuilding(Building building)
        {
            if (building == null) return false;
            return building.TryReserve();
        }
        public void LeaveBuilding(Building building)
        {
            if (building == null) return;
            building.Release();
        }
        public Building GetNearBuilding(BuildingSO type, Vector2 pos)
        {
            Building buildingTarget = null;
            float bestDist = float.MaxValue;

            foreach (var building in Buildings)
            {
                if (building.buildingSO != type)
                    continue;

                float dist = Vector2.Distance(pos, building.transform.position);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    buildingTarget = building;
                }
            }

            return buildingTarget; // 없으면 null
        }
    }
}