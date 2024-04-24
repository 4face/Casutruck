using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarShop : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    private int num;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private Button _Next;
    [SerializeField] private Button _Prev;
    [SerializeField] private Button _Buy;
    [SerializeField] private Button _Select;
    [SerializeField] private Button _Selected;
    [SerializeField] private Button _Back;
    [SerializeField] private Text wrenchScore;
    [SerializeField] private Text _Price;
    void Start()
    {
        bool anyCarPurchased = false;
        for (int i = 0; i < cars.Length; i++)
        {
            if (PlayerPrefs.GetInt("CarBought_" + i, 0) == 1)
            {
                cars[i].GetComponent<Car>().isPurchased = true;
                anyCarPurchased = true;
            }
            if (PlayerPrefs.GetInt("CarSelected_" + i, 0) == 1)
            {
                cars[i].GetComponent<Car>().isSelected = true;
                anyCarPurchased = true;
            }
        }
        if (!anyCarPurchased)
        {
            cars[0].GetComponent<Car>().isPurchased = true;
            cars[0].GetComponent<Car>().isSelected = true;
            PlayerPrefs.SetInt("CarBought_0", 1);
            PlayerPrefs.SetInt("CarSelected_0", 1);
        }
        num = PlayerPrefs.GetInt("CarNum", 0);
        UpdateButtonStates();
        UpdateButtonInteractivity();
        _Next.onClick.AddListener(NextCar);
        _Prev.onClick.AddListener(PrevCar);
        _Select.onClick.AddListener(SelectCar);
        _Buy.onClick.AddListener(BuyCar);
        _Back.onClick.AddListener(Back);
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
        if (Input.GetKeyDown(KeyCode.E))
        {
            int wrenchPoints = PlayerPrefs.GetInt("Wrench", 0);
            PlayerPrefs.SetInt("Wrench", wrenchPoints+150);
            UpdateButtonStates();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].GetComponent<Car>().isSelected = false;
                PlayerPrefs.SetInt("CarSelected_" + i, 0);
            }
            UpdateButtonStates();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].GetComponent<Car>().isPurchased = false;
                PlayerPrefs.SetInt("CarBought_" + i, 0);
            }
            UpdateButtonStates();
        }

    }
    private void UpdateButtonInteractivity()
    {
            if (num == cars.Length - 1) _Next.interactable = false;
            else _Next.interactable = true;
            if (num == 0) _Prev.interactable = false;
            else _Prev.interactable = true;
    }

    private void Back()
    {
        SceneManager.LoadScene(0);
    }
    private void NextCar()
    {
        cars[num].SetActive(false);
        num++;
        transform.rotation = Quaternion.identity;
        cars[num].SetActive(true);
        UpdateButtonInteractivity();
        UpdateButtonStates();
    }

    private void PrevCar()
    {
        cars[num].SetActive(false);
        num--;
        transform.rotation = Quaternion.identity;
        cars[num].SetActive(true);
        UpdateButtonInteractivity();
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        var car = cars[num].GetComponent<Car>();
        wrenchScore.text = PlayerPrefs.GetInt("Wrench", 0).ToString();
        _Buy.gameObject.SetActive(!car.isPurchased);
        _Price.text = "" + car.price;
        _Select.gameObject.SetActive(car.isPurchased && !car.isSelected);
        _Selected.gameObject.SetActive(car.isPurchased && car.isSelected);
    }

    private void BuyCar()
    {
        var car = cars[num].GetComponent<Car>();
        int wrenchPoints = PlayerPrefs.GetInt("Wrench", 0);
        if (wrenchPoints >= car.price)
        {
            wrenchPoints -= car.price;
            PlayerPrefs.SetInt("Wrench", wrenchPoints);
            car.isPurchased = true;
            PlayerPrefs.SetInt("CarBought_" + num, 1);
            UpdateButtonStates();
        }
    }

    private void SelectCar()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].GetComponent<Car>().isSelected = false;
            PlayerPrefs.SetInt("CarSelected_" + i, 0);
        }
        var car = cars[num].GetComponent<Car>();
        car.isSelected = true;
        PlayerPrefs.SetInt("CarSelected_" + num, 1);
        UpdateButtonStates();
    }
}

