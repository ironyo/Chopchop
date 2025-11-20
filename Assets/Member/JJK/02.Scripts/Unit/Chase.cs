using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = false;
        agent.stoppingDistance = 0.5f;
    }
    
    public Transform GetNearestTarget(string targetTag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        if (targets.Length == 0)
            return null;

        Transform closest = null;
        float closestSqrDist = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (var obj in targets)
        {
            Vector3 dir = obj.transform.position - myPos;
            float sqrDist = dir.sqrMagnitude;

            if (sqrDist < closestSqrDist)
            {
                closestSqrDist = sqrDist;
                closest = obj.transform;
            }
        }

        return closest;
    }

    public void MoveTo(Transform target)
    {
        if (agent == null) return;
        agent.SetDestination(target.position);
    }

    public void Stop()
    {
        if (agent == null) return;
        agent.ResetPath();
    }

    public float GetDistance(Transform targetPos)
    {
        return Vector3.Distance(transform.position, targetPos.position);
    }
}
