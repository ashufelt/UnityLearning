using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidBody;


    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(moveSpeed, 0);
    }
    /*
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit2D, enemy" + collision.GetType());
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }
    */
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit2D, enemy" + collision.GetType());
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
