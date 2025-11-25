using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public int level;
    public GameObject[] Task;
    public GameObject[] Finish_Task;
    public GameObject UIreward;
    public Image[] stars;
    public Sprite starFull;
    public Sprite starEmpty;
    public Slider energySlider;
    public TMP_Text textRewardcoins;
    public int[][] RewardCoinsLevels = new int[][]
    {
       new int[] { 0, 50, 100, 150, 200, 250 },
       new int[] { 0, 70, 140, 210, 280, 350 }
    };
    public int activeQuestID = -1;
    [HideInInspector]
    public bool isEnd = false;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartQuest(int id)
    {
        if (activeQuestID != -1) return;
        Task[id].SetActive(false);
        activeQuestID = id;
        Debug.Log("Quest Started: " + id);
        GameControl.Instance.ShowNotice("Đã đón khách thành công!");
        // TODO: Hiện minimap marker, chỉ đường  
        MinimapTargetIcon.Instance.target = Finish_Task[id].transform;
        MinimapTargetIcon.Instance.transform.position = MinimapTargetIcon.Instance.target.position;
        CountdownTimer.Instance.StartCountdownNV(activeQuestID);
        Time.timeScale = 1f;
    }

    public bool IsQuestActive(int id)
    {
        return activeQuestID == id;
    }

    public void CompleteQuest(int id)
    {
        if (activeQuestID != id) return;
        Finish_Task[id].SetActive(false);
        Debug.Log("Quest Completed: " + id);
        activeQuestID = -1;
        FindObjectOfType<CountdownTimer>().CompleteMission();
        EvaluateAndShowStar(level, Finish_Task[id].layer == LayerMask.NameToLayer("vip"));
        // TODO: Nhận thưởng, ẩn marker  
        if (Task.Length > id + 1)
        {
            MinimapTargetIcon.Instance.target = Task[id + 1].transform;
            MinimapTargetIcon.Instance.transform.position = MinimapTargetIcon.Instance.target.position;
        } else
        {
            isEnd = true;
        }
            StartCoroutine(CountdownAndPause());
    }
    public void HideUIReward()
    {
        Time.timeScale = 1f;
        UIreward.SetActive(false);
        if (isEnd)
        {
            GameControl.Instance.ShowNotice2("Chúc mừng vượt qua màn chơi!", "Next level");
            Time.timeScale = 0f;
        }
    }

    public static IEnumerator CountdownAndPause()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }

    public void Update()
    {
        if ((activeQuestID != -1 || QuestUIController.Instance.isHideUI) && Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            if (QuestUIController.Instance.isHideUI) QuestUIController.Instance.isHideUI = false;
        }
    }
    public void EvaluateAndShowStar(int level, bool isVip)
    {
        float energyPercent = energySlider.value / energySlider.maxValue;

        int starCount = 0;

        if (energyPercent >= 1f)
            starCount = 5;
        else if (energyPercent >= 0.8f)
            starCount = 4;
        else if (energyPercent >= 0.6f)
            starCount = 3;
        else if (energyPercent >= 0.4f)
            starCount = 2;
        else if (energyPercent >= 0.2f)
            starCount = 1;
        else
            starCount = 0;

        int coin = RewardCoinsLevels[level-1][starCount];
        int tip = Random.Range(50, 200);
        UIreward.SetActive(true);
        ShowStarRating(starCount);
        textRewardcoins.SetText("Thưởng: " + coin.ToString() + "$" + (isVip || starCount == 5 ? " + Tip: "+ tip : "")+"$");
        PlayerController.Instance.AddCoins(coin+ tip);
    }
    public void ShowStarRating(int count)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = i < count ? starFull : starEmpty;
        }
    }
}
