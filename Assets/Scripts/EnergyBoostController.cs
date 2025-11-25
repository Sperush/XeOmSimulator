using UnityEngine;
using UnityEngine.UI;

public class EnergyBoostController : MonoBehaviour
{
    public Slider energySlider; // Thanh năng lượng
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float boostCostPerSecond = 20f; // Tốn năng lượng khi boost
    private bool isBoosting = false;

    void Start()
    {
        if (energySlider == null)
        {
            Debug.LogError("Energy Slider not assigned!");
            enabled = false;
            return;
        }
        energySlider.maxValue = maxEnergy;
        energySlider.value = currentEnergy;
    }

    void Update()
    {
        if (isBoosting && currentEnergy > 0)
        {
            // Giảm năng lượng khi boost
            currentEnergy = Mathf.Max(0, currentEnergy - boostCostPerSecond * Time.deltaTime);
        }
        else if (currentEnergy < maxEnergy)
        {
            if(isBoosting && currentEnergy <= 0)
            {
                isBoosting = false;
            }
        }
        // Cập nhật Slider
        energySlider.value = currentEnergy;
    }

    public void addEnergy(float energy)
    {
        currentEnergy += energy;
    }

    public bool isFullEnergy()
    {
        return currentEnergy >= maxEnergy;
    }

    public void StartBoost()
    {
        if (currentEnergy > 0)
        {
            isBoosting = !isBoosting;
            Debug.Log("Boost "+ (isBoosting? "activated!":"disable!"));
        }
    }

    public void StopBoost()
    {
        isBoosting = false;
    }

    public bool IsBoosting()
    {
        return isBoosting && currentEnergy > 0;
    }
}