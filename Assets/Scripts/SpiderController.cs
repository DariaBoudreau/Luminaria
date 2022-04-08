using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField]
    Transform aspen;

    [SerializeField]
    [Tooltip("The accuracy of the spider when it approaches the player.")]
    float marginOfError = -10f;

    float speed = 10f;

    private void Update()
    {
        Vector3 positionToMoveToward = new Vector3(aspen.position.x + marginOfError, transform.position.y, transform.position.z);
        if (Mathf.Abs(transform.position.x - positionToMoveToward.x) < 2.5f && Mathf.Abs(transform.position.x - positionToMoveToward.x) > 2f)
        {
            speed = 2f;
        }
        if (transform.position == positionToMoveToward)
        {
            marginOfError = -marginOfError;
            speed = 5f;
        }
        transform.position = Vector3.MoveTowards(transform.position, positionToMoveToward, speed * Time.deltaTime);
    }
}
