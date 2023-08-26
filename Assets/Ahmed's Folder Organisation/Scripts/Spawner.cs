using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Space, Header("Spawn System")]
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
    [Space, Header("Wave System")]
    [SerializeField, Header("Wave System"), Tooltip("Check it true to make wave system activate and it will automatically turn existing to wave system")]
    private bool waveSystem;
    [Header("Number of Waves"), Tooltip("How many waves should the level have, this will divide number of enemies into waves")]
    public int waves;
    [Header("Current Wave"), Tooltip("What current wave is it and it should be under or equal to Number of Waves Number")]
    public int currentWave;
    [SerializeField, Header("Delay of Waves"), Tooltip("Add Delay in-between next wave, must be same order and length of Number of waves")]
    private float[] wavesDelay;
    // Private variables for storing wave stuff
    private int[] wavesNumberDivided;
    private int[] wavesEnemyPointsDivided;

    private void Awake()
    {
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
                }
            }
            StartCoroutine(spawner(spawnStartDelay[0], waves, true));
        }
        else
        {
            for (int num = 0; num < spawnPoints.Length; num++)
            {
                StartCoroutine(spawner(spawnStartDelay[num], num, false));
            }
        }
    }
    private IEnumerator spawner(float startDelay, int i, bool _waves)
    {
        if(_waves && i >= currentWave)
        {
            yield return new WaitForSeconds(wavesDelay[currentWave]);
            currentWave++;

            for (int num = 0; num < spawnNumber[i]; num++)
            {
                Instantiate(enemyPrefab, spawnPoints[i], false);
                yield return new WaitForSeconds(spawnDelay[i]);
            }
        }
        else
        {
            yield return new WaitForSeconds(startDelay);
            for (int num = 0; num < spawnNumber[i]; num++)
            {
                Instantiate(enemyPrefab, spawnPoints[i], false);
                yield return new WaitForSeconds(spawnDelay[i]);
            }
        }
    }
}
