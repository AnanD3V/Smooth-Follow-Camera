using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2D => GetComponent<Rigidbody2D>();

    [SerializeField] float jumpForce;
    [SerializeField] Vector2 groundCheckDimensions;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] float movementSpeed;

    bool isGrounded;
    float horizontalInput;

    FollowTarget cameraFollowTarget;

    private void Start()
    {
        cameraFollowTarget = Camera.main.GetComponent<FollowTarget>();
        cameraFollowTarget.SwitchTartget(transform, true);
        cameraFollowTarget.PauseFollow(false);

        Transform exitGate = FindObjectOfType<Exit>().transform;
        StartCoroutine(ShowExitAtStart(exitGate));          //PLAY INTRO SEQUENCE
    }

    IEnumerator ShowExitAtStart(Transform exit)
    {
        yield return new WaitForSeconds(1f);
        cameraFollowTarget.SwitchTartget(exit, false);  //change camera target to exit
        yield return new WaitForSeconds(2f);            
        cameraFollowTarget.SwitchTartget(transform,false);  //change camera target back to player 
        yield return new WaitForSeconds(1f);
        GetComponent<PlayerInput>().enabled = true;     //enable player input
    }

    private void OnJump()
    {
        if(isGrounded)
            rb2D.velocity += Vector2.up * jumpForce;
    }

    private void OnHorizontalMovement(InputValue axis)
    {
        horizontalInput = axis.Get<float>();
    }

    private void Update()
    {
        CheckForGround();
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(horizontalInput * movementSpeed * Time.fixedDeltaTime, rb2D.velocity.y);
    }
    private void CheckForGround()
    {
        isGrounded = Physics2D.BoxCast(transform.position, groundCheckDimensions, 0f,
                     -transform.up, 0.1f, platformLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, (Vector3)groundCheckDimensions);
    }
}
