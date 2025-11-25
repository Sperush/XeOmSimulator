using UnityEngine;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;

public class EnergySpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject[] energyPrefabs; // G?m 3 lo?i energy
    public Transform[] spawnPoints;

    private GameObject[] spawnedEnergies;

    void Start()
    {
        spawnedEnergies = new GameObject[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnEnergy(i);
        }
    }

    void SpawnEnergy(int index)
    {
        if (spawnedEnergies[index] == null)
        {
            int randomType = Random.Range(0, energyPrefabs.Length);
            GameObject energy = Instantiate(energyPrefabs[randomType], spawnPoints[index].position, Quaternion.identity);
            spawnedEnergies[index] = energy;

            // G?n callback
            Energy energyScript = energy.GetComponent<Energy>();
            if (energyScript != null)
            {
                energyScript.Setup(this, index);
            }
        }
    }

    public void OnEnergyDestroyed(int index)
    {
        spawnedEnergies[index] = null;
        StartCoroutine(RespawnAfterDelay(index, 5f));
    }

    IEnumerator RespawnAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnergy(index);
    }
}
