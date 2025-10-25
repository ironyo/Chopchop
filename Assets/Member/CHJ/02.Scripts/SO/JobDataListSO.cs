using System.Collections.Generic;
using UnityEngine;

namespace Member.CHJ._02.Scripts.SO
{
    [CreateAssetMenu(fileName = "JobDataListSO", menuName = "SO/JobDataListSO", order = 0)]
    public class JobDataListSO : ScriptableObject
    {
        public List<JobDataSO> list;
    }
}