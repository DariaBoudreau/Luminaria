using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollower : MonoBehaviour
{
    [SerializeField]
    Transform objectToFollow;

    [SerializeField]
    float moveSpeed = 50;

    float xOffset = 3;

    Vector3 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the desired amount of offset at the start
        xOffset = transform.position.x - objectToFollow.position.x;

        previousPosition = objectToFollow.position;
        transform.position = new Vector3(previousPosition.x + xOffset, previousPosition.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (previousPosition != objectToFollow.position)
        {
            StartCoroutine(ApproachCo(previousPosition.x, previousPosition.y, objectToFollow.position.x));
            previousPosition = objectToFollow.position;
        }
    }

    IEnumerator ApproachCo(float xPos, float yPos, float xPosFollowObject)
    {
        yield return new WaitForSeconds(.25f);

        if (xPos > xPosFollowObject && xOffset < 0 || xPos < xPosFollowObject && xOffset > 0)
        {
            xOffset *= (-1);
        }

        xPos += xOffset;
        Vector3 desiredPosition = new Vector3(xPos, yPos, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * moveSpeed);
    }
}
