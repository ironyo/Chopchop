using System;
using System.Collections.Generic;
using UnityEngine;

namespace Member.CHJ._02.Scripts
{
    [Serializable]
    public class BuildingManager
    {
        public List<Building> Buildings = new List<Building>();

        public void AddBuilding(Building building)
        {
            if(!Buildings.Contains(building))
                Buildings.Add(building);
        }

        public List<Building> GetBuildingList(BuildingSO type)
        {
            List<Building> temp = new List<Building>();
            foreach (var building in Buildings)
            {
                if (building.buildingSO == type)
                    temp.Add(building);
            }

            return temp;
        }
        public Building GetNearBuilding(BuildingSO type, Vector2 pos)
        {
            Building buildingTarget = GetBuildingList(type)[0]; // GC 1
        
            foreach (var building in GetBuildingList(type))
            {
                if (Vector2.Distance(pos, building.gameObject.transform.position) <
                    Vector2.Distance(pos, buildingTarget.gameObject.transform.position))
                {
                    buildingTarget = building;
                }
            }

            return buildingTarget;
        }
    }
}