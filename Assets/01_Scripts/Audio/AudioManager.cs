using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioShot;
    [Range(0f, 1f)] public float globalVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Play a specific AudioQuery with global volume applied
    public void PlayAudioQuery(AudioQuery audioQuery, Transform audioTransform)
    {
        AudioSource audioSource = Instantiate(audioShot, audioTransform.position, Quaternion.identity);

        audioSource.clip = audioQuery.clip;
        audioSource.volume = audioQuery.volume * globalVolume;
        audioSource.pitch = Random.Range(audioQuery.minPitch, audioQuery.maxPitch);
        audioSource.spatialBlend = audioQuery.spatialBlend;
        audioSource.minDistance = audioQuery.minDistance;
        audioSource.maxDistance = audioQuery.maxDistance;

        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length / audioSource.pitch);
    }

    // Play a random AudioQuery from an AudioQuerySet with global volume applied
    public void PlayRandomAudioQuery(AudioQuerySet audioQuerySet, Transform audioTransform)
    {
        if (audioQuerySet.audioQueries.Count == 0) return;

        int rand = Random.Range(0, audioQuerySet.audioQueries.Count);
        AudioQuery audioQuery = audioQuerySet.audioQueries[rand];

        PlayAudioQuery(audioQuery, audioTransform);
    }

    public void StopAllQueries()
    {
        GameObject[] audioQuery = GameObject.FindGameObjectsWithTag("AudioQuery");
        foreach (GameObject go in audioQuery)
        {
            Destroy(go);
        }
    }
}
