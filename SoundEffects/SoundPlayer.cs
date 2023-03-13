using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] SoundsList[] soundsLists;

    private void OnEnable()
    {
        GameEvents.OnPlaySound += PlaySound;
    }

    private void OnDisable()
    {
        GameEvents.OnPlaySound -= PlaySound;
    }

    private void PlaySound(int soundType, int soundIndex, bool isLooped)
    {
        AudioSource audioSource = soundsLists[soundType].audioSource;
        AudioClip clip = soundsLists[soundType].soundsSO.sounds[soundIndex].clip;
        audioSource.loop = isLooped;
        if (isLooped)
        {
            audioSource.clip = clip;
            audioSource.Play();    
        }
        else
        {
	    audioSource.PlayOneShot(clip);
        }
       // Debug.Log("Played " + soundsLists[soundType].soundsSO.sounds[soundIndex].description + " sound from " + soundsLists[soundType].soundsListType + " audio source. Looped: " + isLooped);
    }
}

[System.Serializable]
public class SoundsList
{
    public string soundsListType;
    public SOSoundEffects soundsSO;
    public AudioSource audioSource;
}

