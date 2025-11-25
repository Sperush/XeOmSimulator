using UnityEngine;

public class Energy : MonoBehaviour
{
    private EnergySpawner spawner;
    private int spawnIndex;

    public void Setup(EnergySpawner spawner, int index)
    {
        this.spawner = spawner;
        spawnIndex = index;
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnergyDestroyed(spawnIndex);
        }
    }
}
