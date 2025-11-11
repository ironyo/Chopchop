using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Member.CHJ._02.Scripts
{
    public class MinionManager : MonoBehaviour
    {
        public static MinionManager Instance;
        public List<Minion> minionList = new List<Minion>();
        public Queue<Minion> minonQueue = new Queue<Minion>();
        private Building _buildingTarget;
        private BuildingManager _buildingManager;
        [SerializeField] private BuildingSO houseSo;
        private WaitForSeconds _waitT = new WaitForSeconds(0.5f);

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _buildingManager = new BuildingManager();
        }

        private void Start()
        {
            TimeManager.Instance.OnOneSecond += UpdateTime;
        }

        private void UpdateTime(int time)
        {
            foreach (var minion in minionList)
            {
                AiStates newState = TimeCheck(minion,minion.TimeStruct,time);
                if (minion.currentState == newState || minion.isMating)
                    continue;
                Debug.Log("CAN CHANGE MATE");
                minion.SetState(newState);
       
            }
        }
        #region StateSetting

            private AiStates TimeCheck(Minion minion,MinionTime minionTime, float time)
            {
                if (minion.isMating)
                    return AiStates.Mate;
                if (time < minionTime.FirstWork) return AiStates.Work;
                else if (time < minionTime.Patrol) return AiStates.Patrol;
                else if (time < minionTime.SecondWork) return AiStates.Work;
                else return AiStates.Sleep;
                
            }
            public void StartMate()
            {
                foreach (var minion in minionList)
                {
                    if (minion.Stats.Age >= 5)
                    {
                        minion.isMating = true;
                        minion.SetState(AiStates.Mate);
                        Debug.Log("[Mate] mate start");
                    }
                }
            }
            public void AddMinion(Minion minion)
            {
                minionList.Add(minion);
                if(!minonQueue.Contains(minion))
                    minonQueue.Enqueue(minion);
            }

            private void OnDisable()
            {
                TimeManager.Instance.OnOneSecond -= UpdateTime;
            }
        

        #endregion
        public IEnumerator MatchMinion()
        {
            if (minonQueue.Count >= 2)
            { 
                _buildingTarget = _buildingManager.GetNearBuilding(houseSo, minonQueue.Dequeue().transform.position);
                if (_buildingTarget != null)
                {
                    var m1 = minonQueue.Dequeue();
                    var m2 = minonQueue.Dequeue();
                    m1.StartMate();
                    m2.StartMate();
                }
            }

            yield return _waitT;
        }
    }
}