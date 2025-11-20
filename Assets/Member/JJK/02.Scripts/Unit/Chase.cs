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
