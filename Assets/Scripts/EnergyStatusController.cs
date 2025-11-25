using UnityEngine.UI;
using UnityEngine;
using System;

public class EnergyStatusController : MonoBehaviour
{
    public Slider energySlider; // Thanh năng lượng
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public Image iconStatus;
    public Sprite[] energySprites;

    // Start is called before the first frame update
    void Start()
    {
        if (energySlider == null)
        {
            Debug.LogError("Energy Slider not assigned!");
            enabled = false;
            return;
        }
        if (iconStatus == null)
        {
            Debug.LogError("Icon Status not assigned!");
            enabled = false;
            return;
        }
        energySlider.maxValue = maxEnergy;
        energySlider.value = currentEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        float percent = currentEnergy / energySlider.maxValue;
        int level = 0;
        if (percent < 0.2f)
        {
            GameControl.Instance.ShowNotice2("Bạn đã thua cuộc vì điểm cảm xúc dưới 20!", "Chơi lại");
            Time.timeScale = 0f;
            return;
        }
        if (percent < 0.4f) level = 4;
        else if (percent < 0.6f) level = 3;
        else if (percent < 0.8f) level = 2;
        else if (percent < 1f) level = 1;
        else level = 0;
        if (iconStatus.sprite != energySprites[level])
        {
            SetImageLevel(level);
        }
        UpdateEnergy(currentEnergy);
    }
    public void SetImageLevel(int index)
    {
        if (energySprites != null && iconStatus != null && index >= 0 && index < energySprites.Length)
        {
            iconStatus.sprite = energySprites[index];
        }
    }

    public void UpdateEnergy(float energy)
    {
        currentEnergy = Mathf.Clamp(energy, 0, maxEnergy);
        energySlider.value = currentEnergy;
    }
    public void SubEnergy(float energy)
    {
        currentEnergy = Mathf.Clamp(currentEnergy - energy, 0, maxEnergy);
        energySlider.value = currentEnergy;
    }
}
