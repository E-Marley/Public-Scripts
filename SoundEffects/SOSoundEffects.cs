using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Sounds List", menuName = "SoundsList")]

public class SOSoundEffects : ScriptableObject
{
    public Sounds[] sounds;
}
[System.Serializable]
public class Sounds
{
    public string description;
    public AudioClip clip;
}

