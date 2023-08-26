using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    [Header("Enemy Target"), Tooltip("Spawned Enemies will go to there")]
    public Transform enemyTarget;
    [SerializeField, Header("Enemy Storage"), Tooltip("GameObject that should store Instantiated enemies to be clean")]
    GameObject enemyStorage;
    [Space(30), Header("Spawn System")]
    // for now the game have only one enemy variation
    [SerializeField, Header("Enemy to Spawn"), Tooltip("Add Enemy Prefab that it will spawn on spawn points")] 
    private GameObject enemyPrefab;
    [SerializeField, Header("Spawn Points"), Tooltip("Add Spawn Points' Transforms (gameobjects) and Enemies will spawn there")]
    private Transform[] spawnPoints;
    [SerializeField, Header("Spawn Number"), Tooltip("Add Number enemies each spawn point should spawn in order of the array of spawn positions")]
    private int[] spawnNumber;
    [SerializeField, Header("Spawn Delay"), Tooltip("Add Enemy Spawn Delay in seconds in order of spawn position's and spawn number's array")]
    private float[] spawnDelay;
    [SerializeField, Header("Start Spawn Delay"), Tooltip("Add Delay before starting to spawn enemies, should be in order of other arrays")]
    private float[] spawnStartDelay;
    [Space(30), Header("Wave System")]
    [SerializeField, Header("Wave System"), Tooltip("Check it true to make wave system activate and it will automatically turn existing to wave system")]
    private bool waveSystem;
    [Header("Number of Waves"), Tooltip("How many waves should the level have, this will divide number of enemies into waves")]
    public int waves;
    [Header("Current Wave"), Tooltip("What current wave is it and it should be under or equal to Number of Waves Number")]
    public int currentWave;
    [SerializeField, Header("Delay of Waves"), Tooltip("Add Delay in-between next wave, must be same order and length of Number of waves")]
    private float[] wavesDelay;
    // Private variables for storing wave stuff
    [HideInInspector] public bool waveRunning;
    [HideInInspector] public int totalEnemies;
    private int[] wavesNumberDivided;
    private int[] wavesEnemyPointsDivided;

    private void Awake()
    {
        instance = this;

        if (wavesDelay.Length != waves)
        {
            Debug.LogWarning("Delay of Waves is not equal to Number of Waves, please make sure is it equal to Number of Waves");
            Debug.Break();
        }
        if (currentWave > waves || currentWave < 0)
        {
            Debug.LogWarning("Current Wave is set higher or in Minus than Number of Waves, please make sure is it under Number of Waves or in positives");
            Debug.Break();
        }
        if (spawnStartDelay.Length <= 1 && waveSystem)
        {
            Debug.LogWarning("Spawn Delay don't have first value of delay before starting spawn, please add value"); ;
            Debug.Break();
        }
        if (spawnPoints.Length != spawnNumber.Length || spawnPoints.Length != spawnDelay.Length || spawnNumber.Length != spawnDelay.Length || spawnStartDelay.Length != spawnPoints.Length)
        {
            Debug.LogWarning("The spawning arraries in Spawner Script are not same length so please check them");
            Debug.Break();
        }
        if (waveSystem)
        {
            wavesNumberDivided = new int[waves];
            wavesEnemyPointsDivided = new int[spawnPoints.Length];
            int numRound;
            for(int i = 0; i < spawnNumber.Length; i++)
            {
                numRound = Mathf.RoundToInt(spawnNumber[i]/waves);
                wavesEnemyPointsDivided[i] = numRound;
                for (int e = 0; e < waves; e++)
                {
                    wavesNumberDivided[e] = wavesNumberDivided[e] + numRound;
                    totalEnemies += numRound;
                }
            }
            StartSpawn();
        }
        else
        {
            for(int i = 0; i < spawnNumber.Length; i++)
            {
                totalEnemies += spawnNumber[i];
            }
            StartSpawn();
        }
    }
    public void StartSpawn()
    {
        if (waveSystem && waves >= currentWave)
        {
            StartCoroutine(waveSpawner(currentWave));
        }
        else if(waves < currentWave)
        {
            Debug.Log("Waves ended");
            // When all waves finished
        }
        else if(!waveSystem)
        {
            for (int num = 0; num < spawnPoints.Length; num++)
            {
                StartCoroutine(normalSpawner(spawnStartDelay[num], num));
            }
        }
    }
    private IEnumerator normalSpawner(float startDelay, int i)
    {
        yield return new WaitForSeconds(startDelay);
        for (int num = 0; num < spawnNumber[i]; num++)
        {
            if(enemyStorage != null)
            {
                Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity, enemyStorage.transform);
            }
            else
            {
                Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity, spawnPoints[i]);
            }
            yield return new WaitForSeconds(spawnDelay[i]);
        }
    }
    private IEnumerator waveSpawner(int i)
    {
        waveRunning = true;

        yield return new WaitForSeconds(wavesDelay[i]);

        for (int num = 0; num < wavesEnemyPointsDivided.Length; num++)
        {
            for (int e = 0; e < wavesEnemyPointsDivided[e]; e++)
            {
                if (enemyStorage != null)
                {
                    Instantiate(enemyPrefab, spawnPoints[num].position, Quaternion.identity, enemyStorage.transform);
                }
                else
                {
                    Instantiate(enemyPrefab, spawnPoints[num].position, Quaternion.identity, spawnPoints[num]);
                }
                yield return new WaitForSeconds(spawnDelay[num]);
            }
        }

        currentWave++;
        waveRunning = false;
    }

    public void EnemyDied()
    {
        totalEnemies--;
        if(totalEnemies <= 0)
        {
            PlayerController.instance.PlayerDieWin(true);
        }
    }
}
