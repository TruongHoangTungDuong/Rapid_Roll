using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    [SerializeField] private Text Heart;
    [SerializeField] private Text Score;
    public float volume=1;
    public AudioSource audioBackground;
    public List<AudioSource> audioList = new List<AudioSource>();
    public GameObject settingMenuPanel;
    public GameObject gameoverPanel;
    public Button settingButton;
    public Button pauseGame;
    public Button continueGame;
    public Button hide;
    public Slider volumeSlider;
    private bool isFullscreen = true;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("UIManager");
                    instance = singletonObject.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    public void ShowSettingMenu()
    {
        settingMenuPanel.SetActive(true);
    }
    public void ShowGameOverPanel()
    {
        gameoverPanel.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1f;
        audioBackground.Play();
    }
    public void HideSettingMenu()
    {
        settingMenuPanel.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void setVolume(float volume)
    {
        audioBackground.volume= volume;
        foreach (AudioSource audio in audioList){
            audio.volume = volume;
        }
    }
    public void setHeartText(string txt)
    {
        if (Heart)
        {
            Heart.text = txt;
        }
    }
    public void setScoreText(string txt)
    {
        if (Score)
        {
            Score.text = txt;
        }
    }
    public void setFullScreen(bool isFullScreen)
    {
        isFullscreen = !isFullscreen;

        if (isFullscreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else
        {
            Screen.SetResolution(960, 540, false); 
        }
    }

    void Start()
    {
        setHeartText(Convert.ToString(3));
        setScoreText(Convert.ToString(0));
        volumeSlider.onValueChanged.AddListener(setVolume);
        audioBackground.Play();
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }
}
