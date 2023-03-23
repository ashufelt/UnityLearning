using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] bool dieOnHit = true;

    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        if (dieOnHit)
        {
            Destroy(gameObject);
        }
        //Debug.Log(gameObject + "hit something for " + damage + " damage");
    }
}
