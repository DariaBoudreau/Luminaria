using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Movement Settings")]
    public float movementSpeed = 3f;
    public float turnSpeed = 0.1f;
    private Vector3 movementDirection;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        movementDirection = newMovementDirection;
    }

    void FixedUpdate()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        Vector3 movement = movementDirection * movementSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }
}
