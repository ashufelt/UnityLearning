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

    void FixedUpdate()
    {
        //RaycastHit2D hit;
        Vector2 rayOrigin = new Vector2(transform.position.x + ((moveSpeed >= 0) ? 0.4f : -0.4f), transform.position.y);
        //Vector2 rayOrigin = transform.position;
        int layerMask = LayerMask.GetMask("Ground");
        //layerMask = ~layerMask;

        if (Physics2D.Raycast(rayOrigin, Vector2.down, 1f, layerMask))
        {
            //Debug.DrawRay(rayOrigin, Vector2.down * 1f, Color.yellow);
        }
        else
        {
            //Debug.DrawRay(rayOrigin, Vector2.down * 1f, Color.red);
            //Debug.Log("Not detecting ground");
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
        /*
        if(Physics2D.Raycast(rayOrigin, Vector2.right, 1f))
        {
            Debug.DrawRay(rayOrigin, Vector2.right * 1f, Color.yellow);
        }
        else
        {
            Debug.DrawRay(rayOrigin, Vector2.right * 1f, Color.red);
        }
        */
    }
    
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
