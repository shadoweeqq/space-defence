using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> musicClips;
    private int lastPlayedIndex = -1;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        PlayRandomTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    void PlayRandomTrack()
    {
        if (musicClips.Count == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, musicClips.Count);
        } while (newIndex == lastPlayedIndex);

        lastPlayedIndex = newIndex;
        audioSource.clip = musicClips[newIndex];
        audioSource.Play();
    }
}