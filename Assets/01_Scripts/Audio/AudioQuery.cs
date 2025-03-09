using UnityEngine;

[CreateAssetMenu(fileName = "AudioQuery", menuName = "Scriptable Objects/Audio/Audio Query")]
public class AudioQuery : ScriptableObject
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 3f)] public float minPitch = 1f;
    [Range(0f, 3f)] public float maxPitch = 1f;
    [Range(0f, 1f)] public float spatialBlend = 0f;
    [Range(0f, 500f)] public float minDistance = 1f;
    [Range(0f, 500f)] public float maxDistance = 500f;
}
