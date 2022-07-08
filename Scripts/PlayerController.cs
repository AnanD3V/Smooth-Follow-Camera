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

    [SerializeField] Transform player2; //OPTIONAL, REMOVE IF YOU DON'T HAVE A SECOND CHARACTER

    FollowTarget cameraFollowTarget;

    private void Start()
    {
        cameraFollowTarget = Camera.main.GetComponent<FollowTarget>();
        cameraFollowTarget.SwitchTartget(transform, true);
        cameraFollowTarget.PauseFollow(false);

        Transform exitGate = FindObjectOfType<Exit>().transform;
        StartCoroutine(ShowExitAtStart(exitGate));
    }

    IEnumerator ShowExitAtStart(Transform exit)
    {
        yield return new WaitForSeconds(1f);
        cameraFollowTarget.SwitchTartget(exit, false);
        yield return new WaitForSeconds(2f);
        cameraFollowTarget.SwitchTartget(transform,false);
        yield return new WaitForSeconds(1f);
        GetComponent<PlayerInput>().enabled = true;
    }

    private void OnJump()
    {
        cameraFollowTarget.SwitchTartget(player2, false);
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
