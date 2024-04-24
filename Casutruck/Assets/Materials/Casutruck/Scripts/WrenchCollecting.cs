using UnityEngine;

public class WrenchCollecting : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIController.wrenchCount++;
            PlayerPrefs.SetInt("Wrench", UIController.wrenchCount);
            GameObject.Destroy(gameObject);
        }
    }
}
