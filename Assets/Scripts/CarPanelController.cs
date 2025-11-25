using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CarPanelController : MonoBehaviour
{
    public RawImage carImage;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    public GameObject modelInstance;
    public RenderTexture renderTexture;
    public int price;

    public string carID;
    public StoreData storeData;

    private TextMeshProUGUI txtBTN;
    StoreSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<StoreSpawner>();

        if (priceText != null)
            priceText.text = "Giá: " + price + "$";

        if (carImage != null && renderTexture != null)
            carImage.texture = renderTexture;

        txtBTN = buyButton.GetComponentInChildren<TextMeshProUGUI>();

        UpdateButton();
    }

    void UpdateButton()
    {
        if (storeData.IsUsing(carID))
            txtBTN.text = "Gỡ";
        else if (storeData.IsPurchased(carID))
            txtBTN.text = "Sử dụng";
        else
            txtBTN.text = "Mua";
    }

    public void OnBuyButtonClicked()
    {
        if (!storeData.IsPurchased(carID))
        {
            if (storeData.coins < price)
            {
                GameControl.Instance.ShowNotice("Không đủ tiền để mua xe!");
                return;
            }

            PlayerController.Instance.SubCoins(price);
            if (!storeData.purchasedCars.Contains(carID))
            {
                storeData.purchasedCars.Add(carID);
            }
            GameControl.Instance.ShowNotice("Mua xe thành công!");
        }
        else if (!storeData.IsUsing(carID))
        {
            storeData.currentCar = carID;
            GameControl.Instance.ShowNotice("Đã chọn xe!");
        }
        else
        {
            storeData.currentCar = "";
            GameControl.Instance.ShowNotice("Đã gỡ xe!");
        }
        if (spawner != null)
        {
            foreach (var ctrl in spawner.allControllers)
            {
                ctrl.UpdateButton();
            }
        }
        if(GameControl.Instance != null)
        {
            GameControl.Instance.UpdatePlayerModel(storeData.currentCar != "");
            Debug.Log("Cập nhật mô hình người chơi với xe: " + storeData.currentCar);
        }
    }
}
