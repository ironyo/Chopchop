using System;
using System.Collections.Generic;
using UnityEngine;

namespace Member.CHJ._02.Scripts
{
    public class MinionManager : MonoBehaviour
    {
        public static MinionManager Instance;
        public List<Minion> minions = new List<Minion>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            Debug.Log(Instance);
        }

        private void Start()
        {
            TimeManager.Instance.OnOneSecond += UpdateTime;
        }

        private void UpdateTime(int time)
        {
            foreach (var minion in minions)
            {
                AiStates newState = TimeCheck(minion.TimeStruct,time);
                if (minion.currentState == newState)
                    continue;
                
                minion.SetState(newState);
       
            }
        }
        private AiStates TimeCheck(MinionTime minionTime, float time)
        {
            if (time < minionTime.FirstWork) return AiStates.Work;
            else if (time < minionTime.Patrol) return AiStates.Patrol;
            else if (time < minionTime.SecondWork) return AiStates.Work;
            else return AiStates.Sleep;
            
        }
        public void StartMate()
        {
            foreach (var minion in minions)
            {
                if (minion.Stats.Age >= 5)
                {
                    minion.SetState(AiStates.Mate);
                    Debug.Log("[Mate] mate start");
                }
            }
        }
        public void AddMinion(Minion minion)
        {
            minions.Add(minion);
        }

        private void OnDisable()
        {
            TimeManager.Instance.OnOneSecond -= UpdateTime;
        }
    }
}