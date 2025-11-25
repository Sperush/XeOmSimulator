using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public Button pauseButton;
    private bool isPaused = false;
    public GameObject pauseScene;
    public GameObject TutorialScene;
    public GameObject SoundScene;
    public GameObject SceneSettings;
    public Button tutorialButton;
    public Button soundButton;
    public GameObject noticePanel;
    public TextMeshProUGUI noticeText;
    public GameObject noticePanel2;
    public TextMeshProUGUI noticeText2;
    public TextMeshProUGUI textBTN;
    public GameObject Rates;
    public EnergyStatusController StatesEnergy;
    public StoreData storeData;
    public PlayerData playerData;
    public GameObject[] player;
    public GameObject StorePanel;
    public AudioSource audio;
    public Slider music;

    public static GameControl Instance;

    void Start()
    {
        music.value = audio.volume = playerData.valua_audio;
        audio.Play();
        if (Instance == null)
        {
            Instance = this;
        }
        if (!LobbyController.Instance.isLobby)
        {
            UpdatePlayerModel(storeData.currentCar != "");
            StorePanel.SetActive(false);
            noticePanel.SetActive(false);
            SceneSettings.SetActive(false);
            TutorialScene.SetActive(false);
            SoundScene.SetActive(false);
            pauseScene.SetActive(false);
            Time.timeScale = 1f;
        } else
        {
            LobbyController.Instance.indexMap = playerData.level_Winned != -1 ? playerData.level_Winned:0;
            if (playerData.isWinnedLevel)
            {
                LobbyController.Instance.indexMap++;
                LobbyController.Instance.StartGame();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseScene.SetActive(isPaused); // hiển thị hoặc ẩn giao diện pause
    }
    public void OpenTutorial()
    {
        SceneSettings.SetActive(true);
        TutorialScene.SetActive(true);
        SoundScene.SetActive(false);
        pauseScene.SetActive(false);
        ChangeButtonColor(tutorialButton, "#E76161");
        ChangeButtonColor(soundButton, "#000000");
    }
    public void ChangeButtonColor(Button btn, string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            if (hex.Equals("#000000"))
            {
                color.a = 0.5f;
            }
            else
            {
                color.a = 1f;
            }
            btn.image.color = color;
        }
    }

    public void OpenSound()
    {
        SceneSettings.SetActive(true);
        SoundScene.SetActive(true);
        TutorialScene.SetActive(false);
        pauseScene.SetActive(false);
        ChangeButtonColor(soundButton, "#E76161");
        ChangeButtonColor(tutorialButton, "#000000");
    }
    public void CloseSettings()
    {
        SceneSettings.SetActive(false);
        TutorialScene.SetActive(false);
        SoundScene.SetActive(false);
        pauseScene.SetActive(true);
        ChangeButtonColor(tutorialButton, "#E76161");
        ChangeButtonColor(soundButton, "#000000");
    }
    public void ShowNotice(string message, float duration = 3f)
    {
        noticeText.text = message;
        noticePanel.SetActive(true);
        StopAllCoroutines(); // Dừng nếu có Coroutine cũ đang chạy
        StartCoroutine(HideAfterSeconds(duration));
    }

    public void ShowNotice2(string message, string txtBTN)
    {
        noticeText2.text = message;
        textBTN.text = txtBTN;
        noticePanel2.SetActive(true);
    }

    private System.Collections.IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        noticePanel.SetActive(false);
    }
    public void UpdatePlayerModel(bool useCar)
    {
        playerData.isMotorcycle = PlayerController.Instance.isMotorbike = !useCar;
        if (!LobbyController.Instance.isLobby)
        {
            if (PlayerController.Instance.isMotorbike)
            {
                player[0].transform.localPosition = player[1].transform.localPosition;
                player[0].transform.localRotation = player[1].transform.localRotation;
            }
            else
            {
                player[1].transform.localPosition = player[0].transform.localPosition;
                player[1].transform.localRotation = player[0].transform.localRotation;
            }
            player[0].SetActive(!useCar);
            player[1].SetActive(useCar);
        }
    }
    public void ActivityBTNNoti()
    {
        if (textBTN.text.Equals("Chơi lại")) {
            ResetGame();
        } else if (textBTN.text.Equals("Next level"))
        {
            playerData.isMotorcycle = PlayerController.Instance.isMotorbike;
            playerData.Sum_Time_Finish = PlayerController.Instance.SumTimeFinish; // Lưu tổng thời gian hoàn thành vào PlayerData
            storeData.coins = PlayerController.Instance.coins;
            playerData.levelWinned.Add(SceneManager.GetActiveScene().name);
            playerData.isWinnedLevel = true;
            playerData.level_Winned = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""))-1;
            SceneManager.LoadScene("Lobby");
        }
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void HideUIStore()
    {
        // Ẩn UI của cửa hàng
        StorePanel.SetActive(false);
    }
    public void ShowUIStore()
    {
        // Hiển thị UI của cửa hàng
        StorePanel.SetActive(true);
    }

    public void BackToLobby()
    {
        playerData.isMotorcycle = PlayerController.Instance.isMotorbike; // Lưu trạng thái xe máy vào PlayerData
        // Quay về Lobby
        SceneManager.LoadScene("Lobby");
        Time.timeScale = 1f; // Đảm bảo game không bị pause khi trở về Lobby
    }

    private void OnApplicationQuit()
    {
        if(PlayerController.Instance == null)
        {
            Debug.Log("PlayerController.Instance == null");
            return;
        }
        playerData.isWinnedLevel = false;
        playerData.isMotorcycle = PlayerController.Instance.isMotorbike; // Lưu trạng thái xe máy vào PlayerData
        playerData.Sum_Time_Finish = PlayerController.Instance.SumTimeFinish; // Lưu tổng thời gian hoàn thành vào PlayerData
    }

    public void SetVolumeAudio()
    {
        audio.volume = music.value;
        playerData.valua_audio = audio.volume;
    }
}
