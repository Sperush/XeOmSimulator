using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class carspawnerscript : MonoBehaviour
{

    [Tooltip("A list of car models that will be spawned randomly. This script will simply copy these cars so they must have the CarAIController script setup and isCarControlledByAI=false.")]
    public List<GameObject> cars = new List<GameObject>();
    [Tooltip("The number of cars that will be spawned.")]
    public int numberOfCarsToSpawn = 1;
    [Tooltip("If false the spawner won't spawn cars.")]
    public bool canSpawn = true;
    [Tooltip("The first checkpoint that the car(s) will be redirected to.")]
    public Transform startingCheckpoint;
    [Tooltip("Time interval between cars in seconds.")]
    public float timeIntervalBetweenCarsInSeconds = 0f;
    [Header("This will randomly assign the distance that the cars will keep \n from other objects or cars from min to max.")]
    public float distanceKeptMin = 2f;
    public float distanceKeptMax = 2f;
    [Header("This will randomly assign the driving recklessness threshold \n from min to max.")]
    public int recklessnessMin = 0;
    public int recklessnessMax = 0;
    public static carspawnerscript Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(SpawnCycle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnCycle()
    {
        while (true) // chạy vô hạn
        {
            if (numberOfCarsToSpawn > 0 && canSpawn)
            {
                GameObject model = cars[Random.Range(0, cars.Count)];
                if (model == null)
                {
                    Debug.LogWarning("Car model is null! Skipping spawn.");
                    yield return null;
                    continue;
                }

                GameObject newCar = Instantiate(model);
                newCar.transform.position = transform.position;
                newCar.transform.rotation = transform.rotation * Quaternion.Euler(0f, 90f, 0f);

                CarAIController controller = newCar.GetComponent<CarAIController>();
                if (controller == null)
                {
                    Debug.LogError("Spawned car does not have CarAIController!");
                    Destroy(newCar);
                    yield return null;
                    continue;
                }

                controller.CheckPointSearch = true;
                controller.isCarControlledByAI = true;
                controller.distanceFromObjects = Random.Range(distanceKeptMin, distanceKeptMax);
                controller.recklessnessThreshold = Random.Range(recklessnessMin, recklessnessMax);
                controller.nextCheckpoint = startingCheckpoint;

                numberOfCarsToSpawn--;

                yield return new WaitForSeconds(timeIntervalBetweenCarsInSeconds);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<CarAIController>())
        {
            canSpawn = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<CarAIController>())
        {
            canSpawn = true;
        }
    }

    public void OnDestroy()
    {
        
    }
}
