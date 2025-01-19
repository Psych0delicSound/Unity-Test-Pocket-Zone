using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 20f;
    int damage = 10;
    private Transform target;

    public void SetTarget(Transform newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                HitTarget();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HitTarget()
    {
        Character character = target.GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}