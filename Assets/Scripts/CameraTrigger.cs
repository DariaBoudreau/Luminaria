using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    const int onPriority = 100;
    const int offPriority = 0;

    public CinemachineVirtualCamera newCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            newCamera.Priority = onPriority;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            newCamera.Priority = offPriority;
        }
        
    }

}
