using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CarData
{
    public string carID;
    public GameObject modelPrefab; // hoặc RenderTexture / hình đại diện
    public int price;
}

public class StoreSpawner : MonoBehaviour
{
    public StoreData storeData;
    [Header("Prefabs")]
    public GameObject carPanelPrefab; // UI panel prefab
    public GameObject rowPrefab;      // Row chứa 2 panel

    [Header("Parents")]
    public Transform contentParent;

    [Header("Car Data")]
    public List<CarData> carDataList; // Danh sách xe
    public List<CarPanelController> allControllers = new List<CarPanelController>();


    void Start()
    {
        SpawnCarPanels();
    }

    public void SpawnCarPanels()
    {
        for (int i = 0; i < carDataList.Count; i += 2)
        {
            GameObject newRow = Instantiate(rowPrefab, contentParent);

            for (int j = 0; j < 2; j++)
            {
                int index = i + j;
                if (index >= carDataList.Count) break;

                CarData data = carDataList[index];
                GameObject carItem = Instantiate(carPanelPrefab, newRow.transform);
                CarPanelController controller = carItem.GetComponent<CarPanelController>();
                if (controller != null)
                {
                    controller.price = data.price;
                    controller.modelInstance = data.modelPrefab;
                    controller.carID = "Car" + index;
                    allControllers.Add(controller);
                }
            }
        }
    }
}
