using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 20f, timeToDestroy = 2f;
    int damage = 10;
    Vector2 direction;

    public void SetDetails(int damageSet, Vector2 directionSet)
    {
        damage = damageSet;
        direction = directionSet.normalized;

        gameObject.GetComponent<Rigidbody2D>().AddForce(direction * speed * 100, ForceMode2D.Force); 
    }
    
    void Update()
    {
        if (timeToDestroy > 0) timeToDestroy -= Time.unscaledDeltaTime;
        else Destroy(gameObject);
    }

    private void HitTarget(Character character)
    {
        if (character != null)
        {
            character.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && !col.isTrigger) HitTarget(col.gameObject.GetComponent<Character>());
    }
}