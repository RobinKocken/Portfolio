using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    public float health;

    public void Damagable(float damage)
    {
        health -= health;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
