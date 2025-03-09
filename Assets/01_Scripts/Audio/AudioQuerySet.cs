using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioQuerySet", menuName = "Scriptable Objects/Audio/Audio Query Set")]
public class AudioQuerySet : ScriptableObject
{
    public List<AudioQuery> audioQueries = new List<AudioQuery>();
}
