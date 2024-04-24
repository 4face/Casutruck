using System.Collections;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [Header("Location Settings")]
    [SerializeField] private GameObject[] Locations;
    public float passedLocations = 0;
    private GameObject previousLocation;

    [Header("Car Settings")]
    [SerializeField] private GameObject[] Cars;
    [SerializeField] private GameObject MainCar;
    private int previousRow = -1;

    [Header("Wrench Settings")]
    [SerializeField] private GameObject Wrench;

    [Header("Generation Settings")]
    [SerializeField, Range(0.0f, 30.0f)] public float SecondsPerCar = 5.0f;
    [SerializeField, Range(0.0f, 60.0f)] public static float SecondsPerWrench = 45.0f;

    private System.Random random = new System.Random();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Generation Trigger"))
        {
            GenerateLocation(other);
            if(UIController.played)
            {
                StartCoroutine(SpawnWrench());
            }
            if (passedLocations >= 2)
            {
                StartCoroutine(SpawnCar());
            }
        }
        else if (other.gameObject.CompareTag("DestroyTrigger"))
        {
            Destroy(previousLocation);
        }
    }

    private IEnumerator SpawnCar()
    {
        while (true)
        {
            GenerateCar();
            yield return new WaitForSeconds(SecondsPerCar);
        }
    }
    private IEnumerator SpawnWrench()
    {
        while (true)
        {
            GenerateWrench();
            yield return new WaitForSeconds(SecondsPerWrench);
        }
    }

    private void GenerateLocation(Collider other) /* Getting the trigger's parent position is our location,
                                                   then add 227,9f along the Z axis = the length of our location
                                                   then getting Random Location of all array of Locations and placing it
                                                   add the previous location to the variable allocated for it, for its subsequent removal
                                                  */
    {
        Vector3 parentPosition = other.gameObject.transform.parent.position;
        parentPosition += new Vector3(0, 0, 227.9f);
        Instantiate(Locations[random.Next(0, Locations.Length)], parentPosition, Quaternion.identity);
        previousLocation = other.transform.parent.gameObject;
        passedLocations++;
    }

    private void GenerateCar() // generating prefab of random car from array 
    {
        Instantiate(Cars[random.Next(0, Cars.Length)], GetRowPositionCar(), Quaternion.Euler(0f, 180f, 0f));
    }

    private void GenerateWrench() // generating prefab of wrench 
    {
        Instantiate(Wrench, GetRowPositionWrench(), Quaternion.Euler(90f, 0f, 0f));
    }
    private Vector3 GetRowPositionCar() // getting position of car and change coordinates for generated car
    {
        Vector3 carPosition = MainCar.transform.position;
        int row = GetRandomRow();

        switch (row)
        {
            case 0:
                carPosition.x = 48.5f;
                carPosition.y = 2.35f;
                carPosition.z += 227.9f;
                break;
            case 1:
                carPosition.x = 43.5f;
                carPosition.y = 2.35f;
                carPosition.z += 227.9f;
                break;
            case 2:
                carPosition.x = 38.5f;
                carPosition.y = 2.35f;
                carPosition.z += 227.9f;
                break;
        }
        return carPosition;
    }
    private Vector3 GetRowPositionWrench() // getting position of car and change coordinates for generated wrench
    {
        Vector3 carPosition = MainCar.transform.position;
        int row = GetRandomRow();

        switch (row)
        {
            case 0:
                carPosition.x = 48.5f;
                carPosition.y = 3.65f;
                carPosition.z += 227.9f;
                break;
            case 1:
                carPosition.x = 43.5f;
                carPosition.y = 3.65f;
                carPosition.z += 227.9f;
                break;
            case 2:
                carPosition.x = 38.5f;
                carPosition.y = 3.65f;
                carPosition.z += 227.9f;
                break;
        }
        return carPosition;
    }

    private int GetRandomRow() // getting a random number between 0 and 3 while the row will not be the same as the previousRow
    {
        int row = random.Next(0, 3);
        while (row == previousRow)
        {
            row = random.Next(0, 3);
        }
        previousRow = row;
        return row;
    }
}