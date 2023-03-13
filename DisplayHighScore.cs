using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHighScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;


    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "High Score: " + Mathf.Round(CheckHighScore());
    }


    private float CheckHighScore()
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);
        return highScore;
    }
}
