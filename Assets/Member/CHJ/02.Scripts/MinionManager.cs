using System;
using System.Collections;
using System.Collections.Generic;
using Member.CHJ._02.Scripts.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Member.CHJ._02.Scripts
{
    public class MinionManager : MonoSingleton<MinionManager>
    {
        public List<Minion> minionList = new List<Minion>();
        [SerializeField] public BuildingSO houseSo;

        [SerializeField] private TextMeshProUGUI hungryTxt;
        [SerializeField] private TextMeshProUGUI thirstyTxt;
        [SerializeField] private TextMeshProUGUI dirtyTxt;

        private float currentTime = 0;
        private float duration = 2f;


        private Building _buildingTarget;
        public MinionsBuildingManager MinionsBuildingManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            MinionsBuildingManager = new MinionsBuildingManager();
        }

        private void Start()
        {
            TimeManager.Instance.OnOneSecond += UpdateTime;
        }

        private void Update()
        {
            currentTime += Time.deltaTime;
            if (duration <= currentTime)
            {
                List<TestMinion> r_list = new List<TestMinion>();

                foreach(Minion minion in minionList)
                {
                    if (minion.TryGetComponent<TestMinion>(out var tm))
                    {
                        tm.Hungry--;
                        tm.Thirsty--;
                        tm.Dirty--;

                        if (tm.Hungry <= 0 || tm.Thirsty <= 0 || tm.Dirty <= 0)
                        {
                            r_list.Add(tm);
                        }
                    }
                }

                r_list.ForEach(x => x.Die("주인이 날 관리하지 않았어.."));

                SetMinionAverageState();
                currentTime = 0;
            }

        }

        private void SetMinionAverageState()
        {
            if (SceneManager.GetActiveScene().buildIndex == 2) return;
            int hAdd = 0;
            int tAdd = 0;
            int dAdd = 0;

            foreach(Minion minion in minionList)
            {
                TestMinion tm = minion.gameObject.GetComponent<TestMinion>();
                hAdd += tm.Hungry;
                tAdd += tm.Thirsty;
                dAdd += tm.Dirty;
            }

            if (minionList.Count == 0) return;

            hAdd /= minionList.Count;
            tAdd /= minionList.Count;
            dAdd /= minionList.Count;

            hungryTxt.text = "에너지: "+hAdd.ToString();
            thirstyTxt.text = "수분: " + tAdd.ToString();
            dirtyTxt.text = "청결: " + dAdd.ToString();
        }

        public void RegisterMinion(Minion minion)
        {
            if (!minionList.Contains(minion))
            {
                minionList.Add(minion);
                if (minionList.Count >= 1000)
                {
                    Debug.Log("GameClear");
                    GameFinishManager.Instance.onGameClear?.Invoke();
                }
            }
        }
        public void UnRegisterMinion(Minion minion)
        {
            if (!minionList.Contains(minion)) return;
            
            minionList.Remove(minion);
            if(minionList.Count == 0)
                GameFinishManager.Instance.onGameOver?.Invoke();
            
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
        private AiStates TimeCheck(Minion minion,MinionTime minionTime, float time)
        {
            if (time < minionTime.FirstWork) return AiStates.Work;
            else if (time < minionTime.Patrol) return AiStates.Patrol;
            else if (time < minionTime.SecondWork) return AiStates.Work;
            else if (time < minionTime.Sleep)return AiStates.Sleep; 
            return AiStates.None;
        }
        #region StateSetting


            private void OnDisable()
            {
                TimeManager.Instance.OnOneSecond -= UpdateTime;
            }
        

        #endregion
    }
}