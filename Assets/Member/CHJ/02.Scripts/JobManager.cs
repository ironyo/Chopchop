using System;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public enum JobType
{
    Miner,Soldier,Baby
}
public class JobManager : MonoBehaviour
{
    [SerializeField] private List<WorkActionScr> jobList = new List<WorkActionScr>();
    public Dictionary<JobType, Type> JobDictionary= new Dictionary<JobType, Type>();
    public static JobManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != null)
            Destroy(gameObject);
        foreach (var jobScr in jobList)
        { 
            JobDictionary.Add(jobScr.Type, jobScr.GetType());
        }
    }

    public void AddJob(Minion minion, JobType type)
    {
        if (JobDictionary.TryGetValue(type, out Type jobType))
        {
            if (minion.gameObject.TryGetComponent<Baby>(out Baby baby))
            {
                Destroy(baby);
            }
            var job = (WorkActionScr)minion.gameObject.AddComponent(jobType);
            minion.behaviorGraph.SetVariableValue("Work Action Scr", job);
        }
    }
}
