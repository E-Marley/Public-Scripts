using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Slider boostSlider;
    [SerializeField] GameObject boostTip;
    public float maxNumberofBoosts;
    public float boostsAvailable;
    Transform playerTransform;
    float initialPos;
    float score;



    private void OnEnable()
    {
        GameEvents.OnPlayerStop += ShowGameOverUI;
        GameEvents.OnGameStart += CloseGameOverPanel;
        GameEvents.OnGameStart += ResetScore;
        GameEvents.OnGameStart += ResetBoosts;
        GameEvents.OnPlayerBoost += BoostSliderUI;
        GameEvents.OnPlayerUseBoost += HideBoostTip;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerStop -= ShowGameOverUI;
        GameEvents.OnGameStart -= CloseGameOverPanel;
        GameEvents.OnGameStart -= ResetScore;
        GameEvents.OnPlayerBoost -= BoostSliderUI;
        GameEvents.OnGameStart -= ResetBoosts;
        GameEvents.OnPlayerUseBoost -= HideBoostTip;
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        initialPos = playerTransform.position.x;
        ResetScore();
        ResetBoosts();

#if UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_STANDALONE
        ChangeTipForGamePlatform("Click on");
#endif
#if UNITY_ANDROID 
        ChangeTipForGamePlatform("Tap on");
#endif
    }

    void Update()
    {
        UpdateScore();
    }

    private void ShowGameOverUI()
    {
        OpenGameOverPanel();
        SaveHighScore(score);
    }

    private void UpdateScore()
    {
        if (GameEvents.playerFlightBegun)
        {
            score = playerTransform.position.x - initialPos;
            scoreText.text = "Score: "+ Mathf.Round(Mathf.Clamp(score, 0, 10000)).ToString();
        }

    }

    private void ChangeTipForGamePlatform(string verb)
    {
        boostTip.GetComponent<TextMeshProUGUI>().text = verb + " screen to use boost!";
    }

    private void ResetScore()
    {
        scoreText.text = "Score: 0";
    }

    private void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void CloseGameOverPanel()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }


    private void SaveHighScore(float newScore)
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);
        bool gotNewHighScore = newScore > highScore;

        if (gotNewHighScore)
        {
            PlayerPrefs.SetFloat("HighScore", newScore);
            PlayerPrefs.Save();
            highScoreText.text = "New High Score!";
        }
        else
        {
            highScoreText.text = "High Score: " + Mathf.Round(highScore);
        }
    }

    private void HideBoostTip()
    {
        boostTip.SetActive(false);
    }
    private void BoostSliderUI(float boostAmount)

    {
        float previousBoostAvailable = boostsAvailable;
        float newBoostAvailable = Mathf.Clamp(boostsAvailable + boostAmount, boostSlider.minValue, boostSlider.maxValue);

        for(int i = 0; i < maxNumberofBoosts; i++)
        {
            if (previousBoostAvailable < maxNumberofBoosts - i && previousBoostAvailable >= maxNumberofBoosts -i -1 && newBoostAvailable >= maxNumberofBoosts - i)
            {
                GameEvents.PlaySound(1);
            }
        }
        boostsAvailable = newBoostAvailable;

        boostSlider.value = boostsAvailable;
    }

    private void ResetBoosts()
    {
        GameEvents.PlayerBoosted(-maxNumberofBoosts);
    }
}
