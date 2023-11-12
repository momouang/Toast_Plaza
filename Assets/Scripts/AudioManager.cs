using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;

    //public GameManager gameManager;

    void Awake ()
    {
        if (instance == null) //Checks if AudioManager is already running
            instance = this;
        else
        {
            Destroy(gameObject); //If AudioManager running elsewhere, destroy instance.
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>(); //We should check this against Alan's method from Week 2.
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Play entry music here.
    void Start()
    {
        Play("BGM(pixel)");
        Play("PigeonPark");
        Play("Pigeon04");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found."); //Incase of typos.
            return;      
        }
        s.source.Play();
    }
}


////When you need to play sound - put this in GameObject
//FindObjectOfType<AudioManager>().Play("NameofSoundHere")