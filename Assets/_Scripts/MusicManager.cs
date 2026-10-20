using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource feeling;
    public AudioSource place;
    public AudioSource distress;
    public AudioSource creepy;

    private void Awake()
    {
        if (instance == null)
        { instance = this; }

        this.feeling.volume = 1.0f;
        this.place.volume = 0.0f;
        this.distress.volume = 0.0f;
    }

    public void PlayFeeling()
    {
        this.feeling.volume = 1.0f;
        this.place.volume = 0.0f;
    }

    public void PlayPlace()
    {
        this.feeling.volume = 0.0f;
        this.place.volume = 1.0f;
    }

    public void AddDistress()
    {
        this.distress.volume = 1.0f;
    }

    public void RemoveDistress()
    {
        this.distress.volume = 0.0f;
    }

    public void PlayCreepy()
    {
        this.place.volume = 0.0f;
        this.feeling.volume = 0.0f;
        this.distress.volume = 0.0f;
        this.creepy.Play();
    }

    public void StopCreepy()
    {
        this.creepy.Stop();
    }

    public void StopAll()
    {
        this.feeling.volume = 0.0f;
        this.place.volume = 0.0f;
        this.distress.volume = 0.0f;
    }
}
