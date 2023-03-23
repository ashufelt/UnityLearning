using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float timeBetweenFiring = 0.2f;

    [Header("AI")]
    [SerializeField] bool useAI = false;
    [SerializeField] float minimumTimeBetweenFiring = 0.1f;
    [SerializeField] float fireRateVarianceSec = 0f;
    
    [HideInInspector] public bool isFiring;

    Coroutine firingCoroutine;

    void Start()
    {
        isFiring = useAI;
    }

    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
        
    }

    private IEnumerator FireContinuously ()
    {
        while (isFiring)
        {
            Rigidbody2D tmpProjRB2D;
            GameObject tmpProjectile = Instantiate( projectilePrefab, 
                                                    new Vector2(transform.position.x, transform.position.y + 0.1f), 
                                                    Quaternion.identity);

            if (null != (tmpProjRB2D = tmpProjectile.GetComponent<Rigidbody2D>()))
            {
                tmpProjRB2D.velocity = transform.up * projectileSpeed;
            }
            Destroy(tmpProjectile, projectileLifetime);

            float fireTime = Random.Range(timeBetweenFiring - fireRateVarianceSec, timeBetweenFiring + fireRateVarianceSec);

            yield return new WaitForSeconds(Mathf.Clamp(fireTime, minimumTimeBetweenFiring, float.MaxValue));
        }
    }
}
