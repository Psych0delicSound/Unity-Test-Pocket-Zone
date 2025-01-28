using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [NonSerialized] public int id;
    Transform targetT;
    float distanceToTarget;
    Character targetCharacter;
    NavMeshAgent navMeshAgent;

    protected override void Start()
    {
        base.Start();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = movementSpeed;

        hpBar = GetComponentInChildren<HPBar>();
    }

    protected override void Update()
    {
        if (targetT == null || isDead) return;

        navMeshAgent.SetDestination(targetT.position);

        distanceToTarget = (targetT.position - transform.position).magnitude;

        if (distanceToTarget < 2) AttackUnarmed(targetCharacter);

        base.Update();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            targetT = col.gameObject.transform;
            targetCharacter = col.gameObject.GetComponent<Character>();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            targetT = null;
            navMeshAgent.ResetPath();
            targetCharacter = null;
        }
    }

    protected override void Die()
    {
        base.Die();

        targetT = null;
        navMeshAgent.ResetPath();
        targetCharacter = null;

        gameController.Spawn(gameController.GetItemPrefab(1), transform.position);
        Destroy(gameObject);
    }

}