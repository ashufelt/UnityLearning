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

        Transform target;
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

            if (target != null)
            {
                if (GetIsInRange())
                {
                    mover.Cancel();
                    AttackBehavior();
                }
                else
                {
                    mover.MoveTo(target.position);
                }
            }
        }

        private void AttackBehavior()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                timeSinceLastAttack = 0;
                GetComponent<Animator>().SetTrigger("attack");
            }
            
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

        //called from animator
        private void Hit()
        {
            if (null == target)
            {
                print("No target to hit");
            }
            else if (null == target.GetComponent<Health>())
            {
                print("Target does not have health component to take damage");
            }
            else
            {
                target.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
}
