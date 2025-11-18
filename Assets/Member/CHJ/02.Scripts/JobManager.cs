using System;
using System.Collections.Generic;
using Member.CHJ._02.Scripts.SO;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public enum JobType
{
    Miner,Baby,Farmer,Chef,WoodHarvester
}
public class JobManager : MonoBehaviour
{
    public JobDataListSO jobDataListSo;
    public Dictionary<JobType, JobDataSO> JobDictionary= new();
    public static JobManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != null)
            Destroy(gameObject);
        foreach (var jobScr in jobDataListSo.list)
        { 
            JobDictionary.Add(jobScr.jobType, jobScr);
        }
    }

    public void AddJob(Minion minion, JobType type)
    {
        if (JobDictionary.TryGetValue(type, out JobDataSO jobSO))
        {
            minion.GetComponent<WorkActionScr>().jobData = jobSO;
            minion.behaviorGraph.BlackboardReference.SetVariableValue("SO", jobSO);
        }
    }
}
