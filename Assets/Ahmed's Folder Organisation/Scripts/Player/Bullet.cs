using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public ParticleSystem _hitParticle;
    [SerializeField] private GameObject _explosion;

    public void Shooted(Vector3 TargetPos, float Speed )
    {
        transform.DOMove(TargetPos, Speed).SetEase(Ease.Linear).OnComplete(() => FinishUp());
    }
    void FinishUp()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Instantiate(_hitParticle, transform.position, Quaternion.identity);;
        Destroy(gameObject);
    }
}
