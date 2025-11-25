using UnityEngine;
using UnityEngine.AI;

public class DogController : MonoBehaviour
{
    public Transform[] player;
    private NavMeshAgent agent;
    private Animator m_Animator;
    public GameObject panelDangerous;

    private Vector3 oldPosition;
    public enum DogStatus
    {
        Normal = 1,
        Angry = 2
    }
    public DogStatus status;
    public static DogController Instance;
    public int State;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        status = DogStatus.Normal;
        oldPosition = transform.position;
        panelDangerous.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        State = 0;
    }

    void Update()
    {
        float sqrDistance = (transform.position - player[PlayerController.Instance.isMotorbike ? 0 : 1].position).sqrMagnitude;
        float threshold = 5f * 5f; // 25

        bool shouldShow = sqrDistance < threshold;
        if (status == DogStatus.Angry && player != null)
        {
            State = 1;
            float threshold1 = 20f * 20f; // 25
            if (sqrDistance < threshold1)
            {
                agent.SetDestination(player[PlayerController.Instance.isMotorbike ? 0 : 1].position);
            } else
            {
                status = DogStatus.Normal;
            }
        } else if(status == DogStatus.Normal)
        {
            State = 0;
            if (shouldShow)
            {
                status = DogStatus.Angry;
            }
        }
        if (m_Animator.GetFloat("State") != State)
        {
            m_Animator.SetFloat("State", State);
        }
        if (panelDangerous.activeSelf != shouldShow)
        {
            panelDangerous.SetActive(shouldShow);
        }
    }
    public void ResetDog()
    {
        status = DogStatus.Normal;
        transform.position = oldPosition;
        agent.ResetPath();
        panelDangerous.SetActive(false);
    }
}
