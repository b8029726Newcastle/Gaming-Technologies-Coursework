using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//make it serializable so I can use this CUSTOM class in inspector
[System.Serializable]
public class Sound
{
    //AUDIO: Custom Sound Class

    public string name;

    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource audioSource; //don't want it to show up in inspector because it's populated in "AudioManager" Class' Start() method

}
