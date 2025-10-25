using UnityEngine;

namespace Member.CHJ._02.Scripts.SO
{
    [CreateAssetMenu(fileName = "JobDataSO", menuName = "SO/JobDataSO", order = 0)]
    public class JobDataSO : ScriptableObject
    {
        public string JobPosName;
        public JobType JobType;
        public BuildingSO BuildingData;
    }
}