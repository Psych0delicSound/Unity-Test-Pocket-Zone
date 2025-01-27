using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    Transform targetT;
    NavMeshAgent navMeshAgent;
    protected override void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = movementSpeed;
        base.Start();
    }

    void Update()
    {
        if (targetT == null) return;

        navMeshAgent.SetDestination(targetT.position);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") targetT = col.gameObject.transform;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            targetT = null;
            navMeshAgent.ResetPath();
        }
    }

}