using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistAudio : MonoBehaviour
{
    private static PersistAudio instance;

    public static PersistAudio Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);    
    }
}