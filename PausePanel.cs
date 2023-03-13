using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;
    private CameraSounds cameraSoundsScript;
    // Update is called once per frame

    private void Start()
    {
        cameraSoundsScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraSounds>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            DisplayPauseOptions();     
        } 
    }

    public void ExitPause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void DisplayPauseOptions()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        
        if (pausePanel.activeSelf)
        { 
            Time.timeScale = 0;
            musicVolumeSlider.value = cameraSoundsScript.EffectsVolume();
            effectsVolumeSlider.value = cameraSoundsScript.MusicVolume();
        }
        else { Time.timeScale = 1; }
    }

    public void ChangeMusicVolume()
    {
        float value = musicVolumeSlider.value;
        cameraSoundsScript.MusicVolumeControl(value);
    }

    public void ChangeEffectsVolume()
    {
        float value = effectsVolumeSlider.value;
        cameraSoundsScript.EffectsVolumeControl(value);
    }

}
