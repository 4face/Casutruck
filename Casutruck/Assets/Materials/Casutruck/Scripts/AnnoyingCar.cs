using UnityEngine;

public class AnnoyingCar : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed;
    private float distanceTraveled = 0f;

    void Update()
    {
        transform.Translate(0, 0, _speed * Time.deltaTime);
        distanceTraveled += _speed * Time.deltaTime;
        if (distanceTraveled >= 300f)
        {
            Destroy(gameObject);
        }
    }
}
