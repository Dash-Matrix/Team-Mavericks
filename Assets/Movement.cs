using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{ 
    public NavMeshAgent enemy;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        enemy.SetDestination(player.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
