using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviours")]
    public PlayerMovement playerMovement;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    float horizontalMovement;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    bool isGrounded = false;

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        horizontalMovement = value.ReadValue<Vector2>().x;
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

        Debug.Log(rawInputMovement);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            playerMovement.UpdateJump();
            isGrounded = false;
        }
    }

    void Update()
    {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
    }

    //Input's Axes values are raw


    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void UpdatePlayerMovement()
    {
        playerMovement.UpdateMovementData(smoothInputMovement, horizontalMovement);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }
}
