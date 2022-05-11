using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] Transform endingPosition;
    [SerializeField] float speed;
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            transform.position = Vector3.MoveTowards(transform.position, endingPosition.position, speed);
        }
    }
}
