using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public int questID;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && QuestUIController.Instance != null && QuestUIController.Instance.currentQuestID + 1 == questID)
        {
            QuestUIController.Instance.ShowAcceptQuest(questID);
        }
    }
}
