using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lifeTime = ProceduralGeneration.SecondsPerWrench;

    private float timer;
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);

        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
