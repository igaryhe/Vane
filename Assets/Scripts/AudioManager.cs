using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public Sound[] sounds;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<AudioManager>();
            if (_instance == null)
            {
                var obj = new GameObject();
                obj.AddComponent<AudioManager>();
                _instance = obj.GetComponent<AudioManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
        }
    }

    public void Play(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
