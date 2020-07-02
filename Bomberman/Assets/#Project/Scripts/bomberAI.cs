using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class bomberAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] targets;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // On commence par lui donner une destination au hasard !
        agent.destination = GiveDestination();
    }

  

    void Update()
    {
        // target s'il n'a plus de target //
        if (IsArrived())
        {
            agent.destination = GiveDestination();
        }
        
    }

    // fonction qui donne un ptn au hasard
    // un point du tableau de "targets" défini plus haut
    private Vector3 GiveDestination()
    {
        int index = Random.Range(0, targets.Length);
        return targets[index].position;
    }

    private bool IsArrived()
    {
        // est-ce que la distance entre la target et toi est <= à la distance à parcourir //
        print(agent.remainingDistance - agent.stoppingDistance);
        return agent.remainingDistance <= agent.stoppingDistance;
    }
}
