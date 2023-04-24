using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        bool isDead = false;

        [SerializeField] float healthPoints = 100f;

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            print(healthPoints);
            if (healthPoints <= 0 && !isDead)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            Destroy(gameObject, 2f);
        }

        public bool IsDead()
        {
            return healthPoints <= 0;
        }
    }
}