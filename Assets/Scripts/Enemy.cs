using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 100f;

    public void TakeDamage(float hit)
    {
        health -= hit;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
