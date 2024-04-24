using Cinemachine;
using UnityEngine;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private CinemachineVirtualCamera _VirtualCamera;
    private void Awake()
    {
        CinemachineTransposer transposer = _VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        bool anyCarSelected = false;
        for (int i = 0; i < cars.Length; i++) // checking the entire array of cars, finding one of the selected ones,
                                              // activating them and setting each camera position specifically for the car
        {
            if (PlayerPrefs.GetInt("CarSelected_" + i, 0) == 1)
            {
                cars[i].GetComponent<Car>().isSelected = true;
                anyCarSelected = true;
                switch (cars[i].GetComponent<Car>().number)
                {
                    case 0:
                        cars[i].SetActive(true);
                        _VirtualCamera.Follow = cars[i].transform;
                        _VirtualCamera.LookAt = cars[i].transform;
                        transposer.m_FollowOffset.y = 3.65f;
                        transposer.m_FollowOffset.z = -8f;
                        break;
                    case 1:
                        cars[i].SetActive(true);
                        _VirtualCamera.Follow = cars[i].transform;
                        _VirtualCamera.LookAt = cars[i].transform;
                        transposer.m_FollowOffset.y = 4.5f;
                        transposer.m_FollowOffset.z = -8f;
                        break;
                    default:
                        cars[0].SetActive(true);
                        _VirtualCamera.Follow = cars[0].transform;
                        _VirtualCamera.LookAt = cars[0].transform;
                        transposer.m_FollowOffset.y = 3.65f;
                        transposer.m_FollowOffset.z = -8f;
                        break;
                }
            }
        }
        if (!anyCarSelected) // if no one car is selected activate first one
        {
            cars[0].SetActive(true);
            _VirtualCamera.Follow = cars[0].transform;
            _VirtualCamera.LookAt = cars[0].transform;
            transposer.m_FollowOffset.y = 3.65f;
            transposer.m_FollowOffset.z = -8f;
        }

    }
}
