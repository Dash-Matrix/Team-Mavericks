using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip Lose;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void _Win()
    {
        m_AudioSource.clip = Win;
        m_AudioSource.Play();
    }
    public void _Lose()
    {
        m_AudioSource.clip = Lose;
        m_AudioSource.Play();
    }
}
