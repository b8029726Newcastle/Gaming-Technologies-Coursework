using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System; //used for Array

public class AudioManager : MonoBehaviour
{
    public Sound[] soundArray;

    // Start is called before the first frame update
    void Start()
    {
        //initialise audio source
        foreach (Sound sound in soundArray)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;

            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;

            sound.audioSource.loop = sound.loop;
        }
        //AUDIO: Play ambient music clips for each scene or level
        Play("Background Theme");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play(string audioName)
    {
        //loop through audioClips and find the one with appropriate name
        //lambda expression - find sound in soundArray where Sound name = audio name
        Sound sound = Array.Find(soundArray, Sound => Sound.name == audioName);

        //check if there's a missing audio clip
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + audioName + " not found!");
            return;
        }
        //AUDIO: Play audio clip
        sound.audioSource.Play();

    }
}
