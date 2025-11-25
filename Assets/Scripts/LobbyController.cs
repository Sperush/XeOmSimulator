using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public GameObject SceneGray;
    public GameObject SceneSettings;
    public GameObject TutorialScene;
    public GameObject LevelScene;
    public GameObject HubScene;
    public GameObject NextRight;
    public GameObject NextLeft;
    public GameObject NoteText;
    public GameObject BlackPanel;
    public GameObject StorePanel;
    public Image ReviewMap;
    public TextMeshProUGUI txtNoteMap;
    public Sprite[] ListReviewMap;
    public string[] ListNoteMap;
    [HideInInspector]
    public int indexMap = 0;
    public static LobbyController Instance;
    public bool isLobby = true;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        ReviewMap.sprite = ListReviewMap[indexMap];
        SceneGray.SetActive(false);
        SceneSettings.SetActive(false);
        TutorialScene.SetActive(false);
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

    public void OpenNoteText()
    {
        NoteText.SetActive(true);
    }
    public void CloseNoteText()
    {
        NoteText.SetActive(false);
    }

    public void OpenSettings()
    {
        SceneGray.SetActive(true);
        SceneSettings.SetActive(true);
        TutorialScene.SetActive(false);
    }

    public void CloseSettings()
    {
        SceneGray.SetActive(false);
        SceneSettings.SetActive(false);
        TutorialScene.SetActive(false);
    }

    public void OpenTutorial()
    {
        SceneGray.SetActive(true);
        SceneSettings.SetActive(false);
        TutorialScene.SetActive(true);
    }

    public void CloseTutorial()
    {
        SceneGray.SetActive(false);
        SceneSettings.SetActive(false);
        TutorialScene.SetActive(false);
    }

    public void StartGame()
    {
        LevelScene.SetActive(true);
        BlackPanel.SetActive(true);
        ReviewMap.sprite = ListReviewMap[indexMap];
        txtNoteMap.SetText(ListNoteMap[indexMap]);
        if (indexMap == 0)
        {
            NextLeft.SetActive(false);
            NextRight.SetActive(true);
        }
        else if (indexMap == ListReviewMap.Length - 1)
        {
            NextRight.SetActive(false);
            NextLeft.SetActive(true);
        }
        else if (indexMap > 0 && indexMap < ListReviewMap.Length - 1)
        {
            NextLeft.SetActive(true);
            NextRight.SetActive(true);
        }
    }

    public void NextMap(bool isRight)
    {
        indexMap += isRight ? 1 : -1;
        ReviewMap.sprite = ListReviewMap[indexMap];
        txtNoteMap.SetText(ListNoteMap[indexMap]);
        if (indexMap == 0)
        {
            NextLeft.SetActive(false);
            NextRight.SetActive(true);
        } else if (isRight && indexMap == ListReviewMap.Length - 1)
        {
            NextRight.SetActive(false);
            NextLeft.SetActive(true);
        }
    }

    public void PlayGame()
    {
        if (indexMap != 0) return;
        isLobby = false;
        SceneManager.LoadScene("Level"+(indexMap+1));
    }

    public void BackToHub()
    {
        HubScene.SetActive(true);
        BlackPanel.SetActive(false);
        SceneGray.SetActive(false);
        NextLeft.SetActive(false);
        NextRight.SetActive(false);
        LevelScene.SetActive(false);
    }

    
}
