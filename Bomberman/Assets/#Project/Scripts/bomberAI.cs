using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class bomberAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] targets;

    private GameObject player;
    public float viewRange = 4f;
    public float time = 1f;
    public float actualTimer = 0f;

    private bool playerFound;

    public GameObject bomb;
  
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // On commence par lui donner une destination au hasard !
        agent.destination = GiveDestination();
        player = GameObject.FindGameObjectWithTag("Player");
    }

  

    void Update()
    {
        

        RaycastHit hit;

        if(actualTimer > 0)
        {
            actualTimer -= Time.deltaTime;
        }

        // un vecteur normalisé est réduit à son expression unitaire
        // donc on a la direction du vecteur mais en (1,1)
        Vector3 direction = (player.transform.position - transform.position).normalized;

        playerFound = false;

        if (Physics.Raycast(transform.position, direction, out hit, viewRange))
        {
            // si j'ai touché le player ?
            if(hit.transform.gameObject == player)
            {
                playerFound = true;
                // la destination de notre agent est là où est l'objet
                agent.destination = player.transform.position;
                if(hit.distance <= 5f)
                {
                    if(actualTimer <= 5f)
                    {
                        ThrowBomb();
                        actualTimer = time;
                    }
                    
                }
            }
        }


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

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        if (playerFound)
        {
            color = Color.red;
        }
        color.a = 0.5f;
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position, viewRange);
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }

    private void ThrowBomb()
    {
        GameObject bombInstance = Instantiate(bomb, transform.position, transform.rotation);
        bombInstance.GetComponent<Bomb>().player = player;
    }
}
