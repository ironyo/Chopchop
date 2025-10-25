using System;
using System.Collections.Generic;
using Member.CHJ._02.Scripts.SO;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public enum JobType
{
    Miner,Soldier,Baby,Farmer,
}
public class JobManager : MonoBehaviour
{
    [SerializeField] private JobDataListSO _jobDataListSo;
    public Dictionary<JobType, JobDataSO> JobDictionary= new();
    public static JobManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != null)
            Destroy(gameObject);
        foreach (var jobScr in _jobDataListSo.list)
        { 
            JobDictionary.Add(jobScr.JobType, jobScr);
        }
    }

    public void AddJob(Minion minion, JobType type)
    {
        if (JobDictionary.TryGetValue(type, out JobDataSO jobSO))
        {
            minion.GetComponent<WorkActionScr>().jobData = jobSO;
        }
    }
}
