using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpSpeed = 5.0f;
    [SerializeField] float climbSpeed = 5.0f;
    [SerializeField] float rollSpeedFactor = 3.0f;

    float baseRunSpeed;

    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowSpawnLocation;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    BoxCollider2D myBodyCollider;
    CapsuleCollider2D myFeetCollider;

    List<Collider2D> results;
    List<Chest> allChests;

    ContactFilter2D chestContactFilter;
    Chest nearbyChest;

    bool isAlive;
    bool isFacingRight;
    bool isRolling;

    float startingGravity;

    Vector2 moveInput;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        myFeetCollider = GetComponent<CapsuleCollider2D>();

        startingGravity = myRigidbody.gravityScale;
        isAlive = true;
        isFacingRight = true;
        isRolling = false;
        baseRunSpeed = runSpeed;

        chestContactFilter.SetLayerMask(LayerMask.GetMask("Chest"));
        allChests = new List<Chest>(Object.FindObjectsOfType<Chest>());
    }

    void Update()
    {
        if (!isAlive) { return; }

        if (!isRolling) //while rolling, player is locked in on the roll, is immune to death, ladders, etc.
        {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }
    }

    void OnMove (InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump (InputValue value)
    {
        bool isOnGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isAlive) { return; }
        if (value.isPressed && isOnGround)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire (InputValue value)
    {
        if (!isAlive) { return; }

        Instantiate(arrow, arrowSpawnLocation.position, transform.rotation);
    }

    void OnRoll (InputValue value)
    {
        if (!isAlive) { return; }

        isRolling = true;
        myAnimator.SetBool("isRolling", true);
        myRigidbody.gravityScale = 0;
        Vector2 rollVelocity = new Vector2(myRigidbody.velocity.x * rollSpeedFactor, 0f);
        myRigidbody.velocity = rollVelocity;

        //end roll after 0.4 seconds
        StartCoroutine("EndRollAfterDelay", 0.4f);
    }

    IEnumerator EndRollAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isRolling = false;
        myAnimator.SetBool("isRolling", false);
        //runSpeed = baseRunSpeed;
        myRigidbody.gravityScale = startingGravity;
    }

    void OnInteract (InputValue value)
    {
        if (!isAlive) { return; }

        // Be sure there are chests before finding closest one
        if (null != (nearbyChest = GetClosestChest(allChests)))
        {
            if (nearbyChest.GetComponent<Collider2D>().IsTouching(myBodyCollider))
            {
                nearbyChest.ChestInteract();
                Debug.Log("Chest interacted (Player log)");
            }
        }
    }

    Chest GetClosestChest (List<Chest> chests)
    {
        Chest closestChest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (Chest chest in chests)
        {
            Vector2 directionToTarget = ((Vector2) chest.transform.position) - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestChest = chest;
            }
        }
        return closestChest;
    }

    void Run()
    {
        if (!isAlive) { return; }
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        if (!isAlive) { return; }
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
            isFacingRight = !isFacingRight;
        }
    }

    void ClimbLadder()
    {
        if (!isAlive) { return; }
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        bool isOnLadder = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (isOnLadder)
        {
            myRigidbody.velocity = climbVelocity;
            myRigidbody.gravityScale = 0f;
            myAnimator.SetBool("isClimbing", Mathf.Abs(climbVelocity.y) > Mathf.Epsilon);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = startingGravity;
        }
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Spikes")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = new Vector2(0f, 0f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    public int GetDirection()
    {
        return isFacingRight ? 1 : -1;
    }
}
