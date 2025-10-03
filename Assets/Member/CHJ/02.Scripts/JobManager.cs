using System;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    [SerializeField] private List<WorkActionScr> jobList = new List<WorkActionScr>();
    public Dictionary<String, WorkActionScr> JobDictionary= new Dictionary<String, WorkActionScr>();

    private void Awake()
    {
        foreach (var jobScr in jobList)
        {
            if (!JobDictionary.ContainsKey(jobScr.Name))
            {
                JobDictionary.Add(jobScr.Name, jobScr);
            }
        }
    }

    public void AddJob(MinionSetting minion, string jobName)
    {
        minion.GetJob(JobDictionary[jobName]);
    }
}
