using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidDataList", menuName = "Asteroids/New Asteroid Data List")]
public class AsteroidDataList : ScriptableObject
{
    public List<AsteroidData> asteroids; // All asteroids of a certain size
}
