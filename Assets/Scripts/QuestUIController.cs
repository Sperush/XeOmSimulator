using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIController : MonoBehaviour
{
    public static QuestUIController Instance;
    public GameObject UIAcceptQuest;
    public GameObject UICompleteQuest;
    public TMP_Text textInfoQuest;
    public TMP_Text txtDifficulty;
    public string[] Quests;
    public string[] Difficulties;
    public Button acceptButton;
    public int currentQuestID = -1;
    public int checkQuestID = -1;

    public bool isHideUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UIAcceptQuest.SetActive(false);
        UICompleteQuest.SetActive(false);
        acceptButton.onClick.AddListener(AcceptQuest);
    }

    public void ShowAcceptQuest(int questID)
    {
        checkQuestID = questID;
        txtDifficulty.SetText("Độ khó: "+Difficulties[checkQuestID]);
        textInfoQuest.SetText(Quests[checkQuestID]);
        UIAcceptQuest.SetActive(true);
        StartCoroutine(QuestManager.CountdownAndPause());
    }

    public void HideAcceptQuest()
    {
        UIAcceptQuest.SetActive(false);
        isHideUI = true;
    }

    private void AcceptQuest()
    {
        if (checkQuestID != -1)
        {
            HideAcceptQuest();
            currentQuestID = checkQuestID;
            QuestManager.Instance.StartQuest(currentQuestID);
        }
    }
}
