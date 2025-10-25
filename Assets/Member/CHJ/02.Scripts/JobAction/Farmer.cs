using UnityEngine;

namespace Member.CHJ._02.Scripts.JobAction
{
    public class Farmer : WorkActionScr
    {
        public override void DoWork()
        {
            base.DoWork();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 6);
        }

        public override void ExitWork()
        {
            base.ExitWork();
        }
    }
}