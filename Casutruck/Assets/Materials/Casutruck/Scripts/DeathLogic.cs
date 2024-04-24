using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    [SerializeField] private UIController UIC;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            UIC.GameOver();
        }
    }
}
