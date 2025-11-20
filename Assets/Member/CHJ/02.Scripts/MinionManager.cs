using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Member.CHJ._02.Scripts
{
    public class MinionManager : MonoBehaviour
    {
        public static MinionManager Instance;
        public List<Minion> minionList = new List<Minion>();
        public int MinionMaxCount { get; private set; }
        [SerializeField] public BuildingSO houseSo;
        [SerializeField] public BuildingSO schoolSo;
        private Building _buildingTarget;
        public MinionsBuildingManager MinionsBuildingManager { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            MinionsBuildingManager = new MinionsBuildingManager();
        }

        private void Start()
        {
            TimeManager.Instance.OnOneSecond += UpdateTime;
        }

        public void RegisterMinion(Minion minion)
        {
            if (!minionList.Contains(minion))
            {
                minionList.Add(minion);
                if(MinionMaxCount <= minionList.Count)
                    MinionMaxCount = minionList.Count;
            }
        }
        public void UnRegisterMinion(Minion minion)
        {
            if(minionList.Contains(minion))
                minionList.Remove(minion);
        }
        private void UpdateTime(int time)
        {
            foreach (var minion in minionList)
            {
                AiStates newState = TimeCheck(minion,minion.TimeStruct,time);
                if (minion.currentState == newState || minion.isMating)
                    continue;
                minion.SetState(newState);
       
            }
        }
        #region StateSetting

            private AiStates TimeCheck(Minion minion,MinionTime minionTime, float time)
            {
                if (time < minionTime.FirstWork) return AiStates.Work;
                else if (time < minionTime.Patrol) return AiStates.Patrol;
                else if (time < minionTime.SecondWork) return AiStates.Work;
                else if (time < minionTime.Sleep)return AiStates.Sleep;
                return AiStates.None;
            }

            private void OnDisable()
            {
                TimeManager.Instance.OnOneSecond -= UpdateTime;
            }
        

        #endregion

    }
}