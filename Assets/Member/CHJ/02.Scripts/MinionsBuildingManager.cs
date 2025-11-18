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
        public Building GetAvailableHouse(Vector3 pos, BuildingSO type, float maxRange = 30f)
        {
            Building target = null;
            float bestDist = float.MaxValue;

            foreach (var building in Buildings)
            {
                if (building == null) continue;
                if (building.buildingSO != type)
                    continue;
                if (!building.CanReserve())
                    continue;

                float dist = Vector2.Distance(pos, building.transform.position);
                if (dist > maxRange) continue;

                if (dist < bestDist)
                {
                    bestDist = dist;
                    target = building;
                }
            }

            // 여기서 예약
            if (target != null)
            {
                Debug.Log($"MinionsBuildingManager {target}");
                target.TryReserve(); // 예약 성공/실패 다시 확인해도 되고
            }

            return target;
        }
        public Building GetAvailableHouseCheckOnly(Vector3 pos, BuildingSO type, float maxRange)
        {
            Building target = null;
            float minDist = float.MaxValue;

            foreach (var b in Buildings)
            {
                Debug.Log("BuildingSo is null" + b.buildingSO);
                if (b.buildingSO == null) continue;
                Debug.Log("BuildingSo isnt null");
                if (type == null) continue;
                Debug.Log("typs isnt null");

                if (b.buildingSO.name != type.name)
                    continue;
                Debug.Log("buildingSO.name == type name null");

                if (!b.CanReserve())
                    continue;
                Debug.Log("CanReserve");

                float dist = Vector3.Distance(pos, b.transform.position);
                if (dist < minDist && dist <= maxRange)
                {
                    minDist = dist;
                    target = b;
                }
            }
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