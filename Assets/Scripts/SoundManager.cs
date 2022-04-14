using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip bgClip, shootSound;
    private AudioSource source;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        PlayBGMusic();
    }

    private void PlayBGMusic()
    {
        source.Play();
    }

    public void ShootSound()
    {
        source.PlayOneShot(shootSound);
    }
}
