using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI EnemyNumber;

    public TextMeshProUGUI WaveNumber;
    public GameObject WaveStartObject;
    public TextMeshProUGUI waveStartText;

    public TextMeshProUGUI Mag;


    private void Awake()
    {
        Instance = this;
    }
    public void UpdateEnemyNumber(int number)
    {
        EnemyNumber.text = number.ToString();
    }
    public void UpdateWaveNumber(int number)
    {
        WaveNumber.text = number.ToString();
    }
    public void UpdateMagNumber(int number)
    {
        Mag.text = number.ToString();
    }
    public void WaveStartText(int wave)
    {
        waveStartText.text = wave.ToString();
        WaveStartObject.GetComponent<DOTweenAnimation>().RecreateTweenAndPlay();
        UpdateWaveNumber(wave);
    }
}
