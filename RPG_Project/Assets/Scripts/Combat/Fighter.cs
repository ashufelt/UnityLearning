using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float damage = 5f;

        Health targetHealth;
        Mover mover;
        float timeSinceLastAttack;

        private void Start()
        {
            timeSinceLastAttack = 0;
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (targetHealth != null)
            {
                if (targetHealth.IsDead())
                {
                    return;
                }
                else if (GetIsInRange())
                {
                    mover.Cancel();
                    AttackBehavior();
                }
                else
                {
                    mover.MoveTo(targetHealth.transform.position);
                }
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(targetHealth.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                timeSinceLastAttack = 0;
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
                print("Player animator trigger: attack");
            }
            
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetHealth.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            targetHealth = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            targetHealth = null;
        }

        //called from animator
        private void Hit()
        {
            if (null == targetHealth)
            {
                print("No target to hit");
            }
            else
            {
                targetHealth.TakeDamage(damage);
            }
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            return (!combatTarget.GetComponent<Health>().IsDead());
        }
    }
}
