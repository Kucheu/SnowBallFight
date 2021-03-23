using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] tracks;
    AudioSource audioSource;


    private int lastTrack;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Tracks numbers");
        Debug.Log(tracks.Length);
        Debug.Log("--------------------");
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            if(tracks.Length > 0)
            {
                int trackNumber = Random.Range(0, tracks.Length);
                if (trackNumber != lastTrack || tracks.Length == 1)
                {
                    audioSource.PlayOneShot(tracks[trackNumber]);
                    lastTrack = trackNumber;
                }
                    
            }
        }
    }
}
