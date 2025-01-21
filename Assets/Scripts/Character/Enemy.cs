using UnityEngine;

public class Enemy : Character
{
    //lastAttacker for kill count
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        // Player player = FindObjectOfType<Player>();
        // if (Vector3.Distance(transform.position, player.transform.position) < 5f)
        // {
        //     Attack(player);
        // }
    }
}