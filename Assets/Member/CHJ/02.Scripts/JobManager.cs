using System;
using System.Collections.Generic;
using Member.CHJ._02.Scripts.SO;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public enum JobType
{
    Miner,Baby,Farmer,Chef,WoodHarvester
}
public class JobManager : MonoSingleton<JobManager>
{
    public JobDataListSO jobDataListSo;
    public Dictionary<JobType, JobDataSO> JobDictionary= new();

    protected override void Awake()
    {
        base.Awake();
        foreach (var jobScr in jobDataListSo.list)
        { 
            JobDictionary.Add(jobScr.jobType, jobScr);
        }
    }

    public void AddJob(Minion minion, JobDataSO type) => minion.GetComponent<WorkActionScr>().jobData = type;
}
