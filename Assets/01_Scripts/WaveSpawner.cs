using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;

    public TextMeshProUGUI waveDisplay;
    public List<SpawnEnemy> enemies = new List<SpawnEnemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    public int maxWaveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    private float maxWaveTime;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public float initialCooldown = 5f; // Initial cooldown before the first wave
    private float initialCooldownTimer;

    public float waveCooldown = 10f; // Cooldown between waves
    private float waveCooldownTimer;
    private bool isInCooldown;


    // Start is called before the first frame update
    void Start()
    {
        initialCooldownTimer = initialCooldown;
        waveCooldownTimer = waveCooldown;
        isInCooldown = true;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInCooldown)
        {
            if (initialCooldownTimer > 0)
            {
                initialCooldownTimer -= Time.fixedDeltaTime;
                waveDisplay.text = "Wave 1 in: " + Mathf.Ceil(initialCooldownTimer).ToString();
                return;
            }
            else if (waveCooldownTimer > 0)
            {
                waveCooldownTimer -= Time.fixedDeltaTime;
                waveDisplay.text = "Next wave in: " + Mathf.Ceil(waveCooldownTimer).ToString();
                return;
            }
            else
            {
                isInCooldown = false;
                GenerateWave();
            }
        }
        else
        {
            waveDisplay.text = "Wave " + currWave.ToString();
        }


        if (spawnTimer <= 0)
        {
            // Spawn an enemy
            if (enemiesToSpawn.Count > 0)
            {
                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity); // Spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // And remove it
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;

                if (spawnIndex + 1 <= spawnLocation.Length - 1)
                {
                    spawnIndex++;
                }
                else
                {
                    spawnIndex = 0;
                }
            }
            else
            {
                waveTimer = 0; // If no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
            maxWaveTime -= Time.fixedDeltaTime;
        }

        if ((waveTimer <= 0 && spawnedEnemies.Count <= 0) || maxWaveTime <= 0)
        {
            currWave++;
            isInCooldown = true;
            waveCooldownTimer = waveCooldown; // Reset the cooldown timer for the next wave
        }
    }

    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
        maxWaveTime = maxWaveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        float totalWeight = 0f;

        // Filter enemies based on the current wave number
        List<SpawnEnemy> availableEnemies = new List<SpawnEnemy>();
        foreach (var enemy in enemies)
        {
            if (currWave >= enemy.unlockWave)
            {
                availableEnemies.Add(enemy);
            }
        }

        // Safety check to avoid infinite loop
        if (availableEnemies.Count == 0)
        {
            Debug.LogError("No enemies available to spawn for this wave!");
            return;
        }

        // Calculate total weight with dynamic scaling
        foreach (var enemy in availableEnemies)
        {
            // Calculate the dynamic weight
            float dynamicWeight = enemy.baseWeight + (enemy.weightScale * (currWave - enemy.unlockWave));
            if (dynamicWeight < 0) dynamicWeight = 0; // Ensure weight doesn't become negative

            totalWeight += dynamicWeight;
        }

        int attempts = 0;
        while (waveValue > 0 && generatedEnemies.Count < 50)
        {
            attempts++;
            if (attempts > 1000) // Safety break to prevent infinite loop
            {
                Debug.LogError("Too many attempts to generate enemies, breaking to prevent infinite loop.");
                break;
            }

            float randomValue = Random.Range(0, totalWeight);
            float cumulativeWeight = 0f;
            SpawnEnemy selectedEnemy = null;

            // Select an enemy based on the weighted random value
            foreach (var enemy in availableEnemies)
            {
                // Calculate the dynamic weight
                float dynamicWeight = enemy.baseWeight + (enemy.weightScale * (currWave - enemy.unlockWave));
                if (dynamicWeight < 0) dynamicWeight = 0; // Ensure weight doesn't become negative

                cumulativeWeight += dynamicWeight;
                if (randomValue <= cumulativeWeight)
                {
                    selectedEnemy = enemy;
                    break;
                }
            }

            if (selectedEnemy != null && waveValue - selectedEnemy.cost >= 0)
            {
                generatedEnemies.Add(selectedEnemy.enemyPrefab);
                waveValue -= selectedEnemy.cost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }

        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

}

[System.Serializable]
public class SpawnEnemy
{
    public GameObject enemyPrefab; // Prefab reference
    public int cost; // Points cost of picking this enemy
    public float baseWeight; // Base weight attribute
    public int unlockWave; // Wave at which this enemy gets unlocked
    public float weightScale; // Weight scaling factor per wave
}

