using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public static WaveSystem instance;

    [SerializeField] private GameObject[] waves;
    int currentWave = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        NextWave();
    }

    public void NextWave()
    {
        if(currentWave < waves.Length)
        {
            if(currentWave != 0)
            {
                waves[currentWave - 1].gameObject.SetActive(false);
                PlayerController.instance.Mag += 5;
                PlayerController.instance.mageStorage += 5;
            }
            waves[currentWave].gameObject.SetActive(true);
            UIManager.Instance.WaveStartText(currentWave + 1);
            currentWave++;
        }
        else
        {
            PlayerController.instance.PlayerDieWin(true);
        }
    }
}
