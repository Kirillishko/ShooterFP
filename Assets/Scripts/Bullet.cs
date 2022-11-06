using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int Damage;

    public void SetDamage(int damage)
    {
        Destroy(gameObject, 5f);
        Damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("EnemyAmmo"))
        {
            //Debug.Log($"IsPlayer : {Damage} = damage");
            collision.gameObject.GetComponent<PlayerHP>().TakeDamage(Damage);
        }
        else if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerAmmo"))
        {
            //Debug.Log("b");
            collision.gameObject.GetComponent<EnemyHP>().TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
