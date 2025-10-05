using System;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    [SerializeField] private List<WorkActionScr> jobList = new List<WorkActionScr>();
    public Dictionary<string, Type> JobDictionary= new Dictionary<String, Type>();

    private void Awake()
    {
        foreach (var jobScr in jobList)
        { 
            JobDictionary.Add("Miner", typeof(Miner));
        }
    }

    public void AddJob(MinionSetting minion, string jobName)
    {
        if (JobDictionary.TryGetValue(jobName, out Type jobType))
        {
            // Baby action 지우기
            var job = minion.gameObject.AddComponent(jobType);
            minion.behaviorGraph.SetVariableValue("Work Action Scr", jobType);
        }
    }
}
