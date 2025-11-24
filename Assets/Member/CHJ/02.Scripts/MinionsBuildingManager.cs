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


            foreach (var b in Buildings)
            {
                if (b == null)
                    continue;
                if (b.buildingSO == null)
                    continue;
                if (type == null)
                    continue;
                if (b.buildingSO != type)
                    continue;
                if (!b.CanReserve())
                    continue;

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