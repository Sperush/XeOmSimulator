using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 90f;
    public TMP_Text countdownText;

    private bool isMissionComplete = false;
    private float delayTime = 0f;

    public static CountdownTimer Instance;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public void CompleteMission()
    {
        isMissionComplete = true;
    }

    System.Collections.IEnumerator StartCountdown()
    {
        float currentTime = countdownTime;

        while (currentTime > 0 && !isMissionComplete)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            if (minutes == 0 && seconds <= 5)
            {
                countdownText.color = Color.red;
            }

            countdownText.text = $"{minutes:D2}:{seconds:D2}";
            currentTime -= Time.deltaTime;
            yield return null;
        }

        // Khi hết thời gian mà chưa hoàn thành nhiệm vụ
        if (!isMissionComplete)
        {
            countdownText.text = "00:00";
            countdownText.color = Color.yellow;
            delayTime = 0f;

            while (!isMissionComplete)
            {
                delayTime += Time.deltaTime;

                int delaySeconds = Mathf.FloorToInt(delayTime);
                countdownText.text = $"+00:{delaySeconds:D2}";
                GameControl.Instance.StatesEnergy.SubEnergy(15);
                GameControl.Instance.ShowNotice("-1 điểm cảm xúc/s vì trễ giờ trả khách!");
                yield return null;
            }
        }
        PlayerController.Instance.SumTimeFinish += (Mathf.FloorToInt(delayTime) + Mathf.FloorToInt(currentTime));
        // Nhiệm vụ hoàn thành → reset màu
        countdownText.text = "00:00";
        countdownText.color = Color.white;
        isMissionComplete = false;
    }
    public void StartCountdownNV(int index)
    {
        switch (index)
        {
            case 0:
                countdownTime = 40f;
                break;
            case 1:
                countdownTime = 30f;
                break;
            case 2:
                countdownTime = 20f;
                break;
            default:
                countdownTime = 0f;
                break;
        }
        StartCoroutine(StartCountdown());
    }
}
