using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ProceduralGeneration PG;
    [SerializeField] private AudioController AC;
    [SerializeField] private CinemachineVirtualCamera _VirtualCamera;
    [SerializeField] private CinemachineVirtualCamera _VirtualCamera2;
    

    [Header("Menu UI Panels")]
    [SerializeField] private GameObject _MenuUI;
    [SerializeField] private Button _Play;
    [SerializeField] private Button _Settings;
    [SerializeField] private Button _Shop;
    [SerializeField] private Text wrenchcoinText1;
    [SerializeField] private Text recordText1;

    [Header("Gameplay UI Panels")]
    [SerializeField] private GameObject _GameplayUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text wrenchcoinText;
    [SerializeField] private Button _Pause;

    [Header("GameOver UI Panels")]
    [SerializeField] private GameObject _GameOverUI;
    [SerializeField] private Text wrenchcoinText2;
    [SerializeField] private Button _Restart;
    [SerializeField] private Text recordText2;
    [SerializeField] private Button _Menu;

    [Header("Settings UI Panels")]
    [SerializeField] private GameObject _SettingsUI;
    [SerializeField] private Slider sliderVolumeMusic;
    [SerializeField] private Scrollbar scrollbarDifficulty;
    [SerializeField] private Button _Back;

    [Header("Pause UI Panels")]
    [SerializeField] private GameObject _PauseUI;
    [SerializeField] private Slider sliderVolumeMusic1;
    [SerializeField] private Button _Menu1;
    [SerializeField] private Button _Back1;


    [Header("Gameplay Variables")]
    [SerializeField][Range(0f, 5f)] private float pointsPerSecond;

    public static int wrenchCount;
    private float scoreCount;
    private float recordCount; // ReCoRd WTF OOMG LMAO !@#!@#!@# 
    public static bool played = false; // Make it static, for this variable will not be able to destroyed, when we restart scene

    private void Awake()
    {
        if (played) // if the game is started, we activate the first camera, which is responsible for the gameplay
        {
            _VirtualCamera.gameObject.SetActive(true);
        }
        else // if the game is not started, activate the second camera, which is responsible for the menu
        {
            _VirtualCamera2.gameObject.SetActive(true);
        }
    }
    private void Start()
    {
        _Play.onClick.AddListener(StartGame);
        _Menu.onClick.AddListener(OpenMenu);
        _Restart.onClick.AddListener(RestartGame);
        _Shop.onClick.AddListener(OpenShop);
        
        _Pause.onClick.AddListener(OpenPause);
        _Menu1.onClick.AddListener(OpenMenu);
        _Back1.onClick.AddListener(ReturnBackGame);
        sliderVolumeMusic1.onValueChanged.AddListener(value => AC.VolumeController(sliderVolumeMusic1));

        _Settings.onClick.AddListener(OpenSettings);
        sliderVolumeMusic.onValueChanged.AddListener(value => AC.VolumeController(sliderVolumeMusic));
        scrollbarDifficulty.onValueChanged.AddListener(SetDifficultyLevel);
        _Back.onClick.AddListener(ReturnBackMenu);
    }

    private void Update()
    {
        if (played &&  !_PauseUI.activeSelf)
        {
            UpdateGameplayUI();
        }
        else if(!_MenuUI.activeSelf && !played && !_SettingsUI.activeSelf) 
        {
            UpdateMenuUI();
        }
    }

    private void UpdateGameplayUI() 
    {
        if (!_GameplayUI.activeSelf && !_GameOverUI.activeSelf)
        {
            _GameplayUI.SetActive(true);
        }

        scoreCount += (pointsPerSecond + PG.passedLocations)/ 5 * Time.deltaTime; // Scores have a progression that depends on the number of locations passed
        scoreText.text = Mathf.Round(scoreCount).ToString();
        wrenchCount = PlayerPrefs.GetInt("Wrench");
        wrenchcoinText.text = wrenchCount.ToString();
    }

    private void UpdateMenuUI()
    {
        _MenuUI.SetActive(true);
        recordCount = PlayerPrefs.GetFloat("Record");
        recordText1.text = Mathf.Round(recordCount).ToString();
        wrenchCount = PlayerPrefs.GetInt("Wrench");
        wrenchcoinText1.text = wrenchCount.ToString();
    }

    private void StartGame()
    {
        switch (PlayerPrefs.GetFloat("difficultyLevel")) // Getting difficulty, placing it in our game
        {
            case 0:
                PG.SecondsPerCar = 13;
                ProceduralGeneration.SecondsPerWrench = 60;
                break;
            case 0.5f:
                PG.SecondsPerCar = 9;
                ProceduralGeneration.SecondsPerWrench = 45;
                break;
            case 1f:
                PG.SecondsPerCar = 5;
                ProceduralGeneration.SecondsPerWrench = 30  ;
                break;


        }
        played = true;
        _MenuUI.SetActive(false);
        _GameplayUI.SetActive(true);
        _VirtualCamera.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        scoreCount = 0;
        Time.timeScale = 1;
        _GameOverUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        if (PlayerPrefs.GetFloat("Record") < scoreCount)
        {
            PlayerPrefs.SetFloat("Record", scoreCount);
        }

        _GameOverUI.SetActive(true);
        _GameplayUI.SetActive(false);

        recordCount = PlayerPrefs.GetFloat("Record");
        recordText2.text = Mathf.Round(recordCount).ToString();
        PlayerPrefs.SetInt("Wrench", wrenchCount);
        wrenchCount = PlayerPrefs.GetInt("Wrench");
        wrenchcoinText2.text = wrenchCount.ToString();

        Time.timeScale = 0;
    }
    private void OpenMenu()
    {
        scoreCount = 0;
        Time.timeScale = 1;
        _GameOverUI.SetActive(false);
        _PauseUI.SetActive(false);
        played = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OpenSettings()
    {
        _SettingsUI.SetActive(true);
        _MenuUI.SetActive(false);
        sliderVolumeMusic.value = PlayerPrefs.GetFloat("Volume");
        scrollbarDifficulty.value = PlayerPrefs.GetFloat("difficultyLevel");
    }
    private void OpenPause()
    {
        Time.timeScale = 0;
        sliderVolumeMusic1.value = PlayerPrefs.GetFloat("Volume");
        _PauseUI.SetActive(true);
        _GameplayUI.SetActive(false);
    }
    private void ReturnBackMenu()
    {
        PlayerPrefs.SetFloat("Volume", sliderVolumeMusic.value);
        _SettingsUI.SetActive(false);
        _MenuUI.SetActive(true);
    }
    private void ReturnBackGame()
    {
        PlayerPrefs.SetFloat("Volume", sliderVolumeMusic1.value);
        Time.timeScale = 1;
        _PauseUI.SetActive(false);
        _GameplayUI.SetActive(true);
    }
    private void SetDifficultyLevel(float value)
    {
        PlayerPrefs.SetFloat("difficultyLevel", value);
    }
    private void OpenShop()
    {
        SceneManager.LoadScene(1);
    }
}