using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleEffect;

    public void Shooted(Vector3 TargetPos, float Speed )
    {
        Instantiate(_particleEffect, transform.position, Quaternion.identity);
        transform.DOMove(TargetPos, Speed).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collider Hat");
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "BG")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Movement>().Hit();
            Destroy(gameObject);
        }
    }
}
