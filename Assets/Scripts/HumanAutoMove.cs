using UnityEngine;
using UnityEngine.AI;

public class HumanAutoMove : MonoBehaviour
{
    public Transform targetPoint; // Điểm đến
    private NavMeshAgent agent;
    private Animator animator;

    public RuntimeAnimatorController RunController;
    private Vector3 oldPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        oldPosition = transform.position;

        if (agent == null || animator == null)
        {
            Debug.LogError("Missing NavMeshAgent or Animator!");
            return;
        }

        animator.runtimeAnimatorController = RunController;

        if (targetPoint != null)
        {
            agent.SetDestination(targetPoint.position);
        }
    }

    void Update()
    {
        if (targetPoint == null || agent == null) return;
            // Kiểm tra đã đến nơi chưa
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
            {
                agent.SetDestination(targetPoint.position);
                if ((transform.position - targetPoint.position).sqrMagnitude < 0.01f)
                {
                    transform.position = oldPosition;
                }
            }
        }
    }
}
