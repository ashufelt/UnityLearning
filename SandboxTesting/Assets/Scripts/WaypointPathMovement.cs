using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPathMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float waitTimeSeconds = 0.5f;

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    enum State
    { 
        Moving,
        Waiting
    }
    private State currentState = State.Moving;

    private void Update()
    {
        switch (currentState)
        {
            case State.Moving:
                if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.01f)
                {
                    currentWaypointIndex += 1;
                    currentWaypointIndex %= waypoints.Length;
                    currentState = State.Waiting;
                    StartCoroutine(PausePlatform());
                }
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, movementSpeed * Time.deltaTime);
                break;
            case State.Waiting:
                
                break;

        }
    }

    private IEnumerator PausePlatform()
    {
        yield return new WaitForSeconds(waitTimeSeconds);
        currentState = State.Moving;
    }
}
