using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 targetOffset;

    float smoothTime = 0.5f;
    float maxSpeed = 15f;

    [SerializeField] float followSmoothTime = 0.5f;
    [SerializeField] float followMaxSpeed = 15f;
    Vector3 velocity;

    [SerializeField] float switchSmoothTime = 0.2f;
    [SerializeField] float switchMaxSpeed = 25f;
    [SerializeField] float switchSpeedThreshold = 0.5f;     //Lower value gives better results

    bool isPaused;
    private void Start()
    {
        if (target != null)
            targetOffset = transform.position - target.position;
        else
            isPaused = true;
    }
    private void LateUpdate()
    {
        if (!isPaused)
        {
            //transform.position = target.position + targetOffset;
            transform.position = Vector3.SmoothDamp(transform.position, target.position + targetOffset,
                                    ref velocity, smoothTime, maxSpeed);
        }

    }

    public void SwitchTartget(Transform newTarget, bool changeOffset)
    {
        target = newTarget;
        if(changeOffset)
            targetOffset = transform.position - target.position;


        StartCoroutine(SwitchTransition());
    }

    IEnumerator SwitchTransition()
    {
        smoothTime = switchSmoothTime;
        maxSpeed = switchMaxSpeed;              //Switching speed

        while (velocity.sqrMagnitude <= switchSpeedThreshold)   
        {
            yield return new WaitForEndOfFrame();
        }

        //AT THIS POINT CAMERA ACCELERATES ABOVE THRESHOLD

        while(velocity.sqrMagnitude > switchSpeedThreshold)
        {
            yield return new WaitForEndOfFrame();
        }

        //AT THIS POINT CAMERA DECELERATES BELOW THRESHOLD
        //THIS MEANS IT IS ABOUT TO REACH DESTINATION

        smoothTime = followSmoothTime;
        maxSpeed = followMaxSpeed;          //Follow Speed
           

        //YOU CAN USE A DELEGATE CALL HERE, WHEN THE TRANSITION IS COMPLETE
    }

    public void PauseFollow(bool value)
    {
        isPaused = value;
    }
}
