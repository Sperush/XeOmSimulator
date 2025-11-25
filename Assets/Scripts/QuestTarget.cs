using UnityEngine;

public class QuestTarget : MonoBehaviour
{
    public int questID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && QuestManager.Instance != null && QuestManager.Instance.IsQuestActive(questID))
        {
            QuestManager.Instance.CompleteQuest(questID);
        }
    }
}
