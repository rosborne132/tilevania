using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float runSpeed = 5f;

    // State
    bool isAlive = true;

    // Cached component references
    Animator myAnimator;
    Collider2D myCollider2D;
    Rigidbody2D myRigidBody;
    float gravityScaleAtStart;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();

        gravityScaleAtStart = myRigidBody.gravityScale;
    }


    void Update()
    {
        ClimbLadder();
        FlipSprite();
        Jump();
        Run();
    }

    private bool HasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    private bool HasVerticalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
    }

    private void ClimbLadder()
    {
        // Check if our player is touching a climbable object.
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("Climbing", false);
            return;
        }

        // This is to prevent the player from falling down the climbable object.
        myRigidBody.gravityScale = 0;

        // If so then climb
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // Value is between -1 to +1
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;

        // Animate climbing
        bool playerHasVerticalSpeed = HasVerticalSpeed();
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // Value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = HasHorizontalSpeed();

        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = HasHorizontalSpeed();

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }

        return;
    }

    private void Jump()
    {
        // Check if our player is touching the ground.
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }
}
