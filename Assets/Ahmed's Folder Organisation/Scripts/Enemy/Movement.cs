using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{ 
    public NavMeshAgent enemy;
    Animator anim;

    [SerializeField] int Health;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        enemy.SetDestination(GameObject.FindWithTag("Player").transform.position);
    }
    public void Hit()
    {
        Debug.Log("Enemy Hit");

        Health--;
        if (Health <= 0)
        {
            // Play Dead Animation
            Spawner.instance.EnemyDied();
            enemy.Stop();
            enemy.enabled = false;
            GetComponent<Collider>().enabled = false;
            anim.SetTrigger("Died");
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().enabled)
            {
                collision.gameObject.GetComponent<PlayerController>().PlayerDieWin(false);
            }
            enemy.SetDestination(Spawner.instance.enemyTarget.position);
        }
    }
}
