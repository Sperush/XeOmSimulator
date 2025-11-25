using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isMotorbike; // Biến để xác định đây có phải là xe máy không
    public int coins = 0; // Số tiền hiện tại
    public TMP_Text textcoins;
    public StoreData storeData;
    public PlayerData playerData;
    public static PlayerController Instance;
    public int SumTimeFinish = 0; // Tổng thời gian hoàn thành các nhiệm vụ

    void Start()
    {
        Instance = this;
        isMotorbike = playerData.isMotorcycle; // Lấy trạng thái xe máy từ PlayerData
        SumTimeFinish = playerData.Sum_Time_Finish; // Lấy số tiền từ StoreData
        coins = storeData.coins;
        textcoins.text = coins.ToString(); // Hiển thị số tiền ban đầu
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        textcoins.text = coins.ToString();

    }

    public void SubCoins(int amount)
    {
        if(coins - amount < 0)
        {
            GameControl.Instance.ShowNotice2("Bạn đã thua cuộc!", "Chơi lại");
            Time.timeScale = 0f;
            return;
        }
        coins = Mathf.Max(0, coins - amount);
        storeData.coins = coins;
        textcoins.text = coins.ToString();
    }
}
