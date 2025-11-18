using System;
using UnityEngine;

namespace Member.CHJ._02.Scripts.SO
{
    [CreateAssetMenu(fileName = "JobDataSO", menuName = "SO/JobDataSO", order = 0)]
    public abstract class JobDataSO : ScriptableObject
    {
        public string jobName;
        public JobType jobType;
        public BuildingSO buildingData;

        private void OnValidate()
        {
            if(jobName != null)
                name = jobName;
        }
    }
}