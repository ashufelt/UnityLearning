using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    [SerializeField] private bool isLeftPaddle = true;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float rotationSpeed = 1000f;

    private Rigidbody2D rBody2D;
    private HingeJoint2D hingeJoint2D;

    // Start is called before the first frame update
    void Start()
    {
        rBody2D = GetComponent<Rigidbody2D>();
        hingeJoint2D = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeftPaddle && playerInput.actions["ButtonPressLeftPaddle"].IsPressed())
        {
            //Debug.Log("Polling: Paddle button Pressed : Left");
            rBody2D.MoveRotation(rBody2D.rotation + (rotationSpeed * Time.deltaTime));
        }
        else if (isLeftPaddle)
        {
            //Debug.Log("Polling: Paddle button Not Pressed : Left");
            rBody2D.MoveRotation(rBody2D.rotation + ((-1) * rotationSpeed * Time.deltaTime));
        }
        else if (!isLeftPaddle && playerInput.actions["ButtonPressRightPaddle"].IsPressed())
        {
            //Debug.Log("Polling: Paddle button Pressed : Right");
            rBody2D.MoveRotation(rBody2D.rotation + ((-1) * rotationSpeed * Time.deltaTime));
        }
        else if (!isLeftPaddle)
        {
            //Debug.Log("Polling: Paddle button Not Pressed : Left");
            rBody2D.MoveRotation(rBody2D.rotation + (rotationSpeed * Time.deltaTime));
        }
    }
}
