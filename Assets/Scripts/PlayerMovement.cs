// PlayerMovement.cs
using System;
using UnityEngine;
// No longer needed if Universal RP is not directly impacting this script's functionality
// using UnityEngine.Rendering.Universal.Internal;

public class PlayerMovement : MonoBehaviour
{
    public bool externalLockMovement = false;
    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    private int grounded;
    private int maxJumps = 1; //no of jumps possible in air
    private float horizontalInput; // Used for both keyboard and mobile combined input

    private float objectScale = 1.0f;
    private bool onWall;
    [SerializeField] float wallJumpGravity = 6.0f;
    [SerializeField] private float wallCooldownMax;
    private float wallCooldown;
    [SerializeField] float jumpOffset; // This is used for wall jump horizontal impulse
    [SerializeField] private float rayLength = 1.5f;
    [SerializeField] private Transform rayPosition;

    // New: Reference to the MobileInputManager
    private MobileInputManager mobileInputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grounded = 0;
        onWall = false;

        // Find the MobileInputManager in the scene
        mobileInputManager = FindObjectOfType<MobileInputManager>();
        if (mobileInputManager == null)
        {
            Debug.LogError("MobileInputManager not found in scene! Please ensure it's on a GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        wallCooldown = Math.Clamp(wallCooldown - Time.deltaTime, 0, wallCooldownMax);

        // Keyboard Jump Input (W) - remains active
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && grounded < maxJumps && !onWall)
        {
            Jump();
        }

        // Wall disengage from keyboard (S) - remains active
        if (onWall && Input.GetKeyDown(KeyCode.S))
        {
            rb.gravityScale = wallJumpGravity;
            onWall = false;
        }

        // Wall jump from keyboard (A/D) - Modified to use current keyboard horizontal input
        // If you want specific A/D for wall jump, you would add separate Input.GetKeyDown checks here
        if (onWall && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            // Determine wall jump direction based on current keyboard input
            float keyboardWallJumpDirection = Input.GetAxisRaw("Horizontal");
            if (keyboardWallJumpDirection != 0)
            {
                jumpOffset = Math.Abs(jumpOffset) * keyboardWallJumpDirection;
                rb.gravityScale = wallJumpGravity;
                rb.linearVelocity = new Vector2(speed * jumpOffset, jumpSpeed);
                onWall = false;
            }
        }

        //Method to check when to reset the grounded variable
        ResetJumpCount();
    }

    void FixedUpdate()
    {
        if (!externalLockMovement)
        {
            if (!onWall)
            {
                // Start with keyboard horizontal input
                horizontalInput = Input.GetAxis("Horizontal");

                // If mobile input manager exists, combine or override horizontal input
                if (mobileInputManager != null)
                {
                    float mobileHorizontalInput = 0f;
                    if (mobileInputManager.moveLeftPressed)
                    {
                        mobileHorizontalInput = -1f;
                    }
                    else if (mobileInputManager.moveRightPressed)
                    {
                        mobileHorizontalInput = 1f;
                    }

                    // Prioritize mobile input if any mobile directional button is pressed
                    // This means if you're pressing a mobile button, it overrides keyboard
                    if (mobileHorizontalInput != 0f)
                    {
                        horizontalInput = mobileHorizontalInput;
                    }
                    // If no mobile directional button is pressed, horizontalInput remains from keyboard.
                }

                if (wallCooldown == 0) // Only apply horizontal movement if not in wall jump cooldown
                {
                    movement = new Vector2(horizontalInput * speed, rb.linearVelocityY);
                    rb.linearVelocity = movement;
                }
                else
                {
                    // During wall cooldown, only apply the jumpOffset for horizontal movement
                    // This ensures the player is propelled horizontally after a wall jump
                    movement = new Vector2(jumpOffset * speed, rb.linearVelocityY); // Note: using jumpOffset for horizontal speed during cooldown
                    rb.linearVelocity = movement;
                }


                //flip player
                if (horizontalInput > 0.01f)
                    transform.localScale = Vector3.one * objectScale;
                else if (horizontalInput < -0.01f)
                    transform.localScale = new Vector3(-1, 1, 1) * objectScale;
            }
            else
            {
                // When on wall, ensure gravity is reset and velocity is zero
                rb.gravityScale = 0;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void Jump()
    {
        if(rb.linearVelocityY > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY + jumpSpeed);
        else
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpSpeed);
        grounded += 1;
    }

    // This method is now primarily a state manager for being on a wall,
    // actual wall jumping or disengaging is handled by inputs in Update or button methods.
    private void WallJump()
    {
        wallCooldown = wallCooldownMax; //refresh wall cooldown for as long as the player is on the wall
        // The actual wall jump and disengage logic is now in OnJumpButtonPressed and Update for keyboard
        // No direct movement or gravity changes here, just cooldown management
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Use CompareTag for efficiency
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            onWall = true;
            grounded = maxJumps; //no more jumps once on the wall
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (onWall) // Only reset if we were actually on the wall
            {
                rb.gravityScale = wallJumpGravity; // Reset gravity when leaving wall
                onWall = false;
            }
        }
    }

    void ResetJumpCount()
    {
        // Use a small offset for the raycast origin to avoid hitting the player's own collider
        Vector2 rayOrigin = new Vector2(rayPosition.position.x, rayPosition.position.y - 0.1f); // Small offset down

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, LayerMask.GetMask("Default", "KineticObjects", "Ground")); // Ensure "Ground" layer is included

        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);

        if (hit.collider != null) // Check if hit.collider is not null first
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
                grounded = 0;
        }
    }


    // --- Public Methods for UI Buttons ---

    public void OnJumpButtonPressed()
    {
        if (onWall)
        {
            // Determine wall jump direction based on current mobile horizontal input
            float mobileWallJumpDirection = 0;
            if (mobileInputManager != null)
            {
                if (mobileInputManager.moveRightPressed) mobileWallJumpDirection = 1f;
                else if (mobileInputManager.moveLeftPressed) mobileWallJumpDirection = -1f;
            }

            // If no directional input from mobile, jump away from the wall (based on player's facing direction)
            if (mobileWallJumpDirection == 0)
            {
                mobileWallJumpDirection = -Mathf.Sign(transform.localScale.x); // Jump opposite to facing direction
            }

            jumpOffset = Math.Abs(jumpOffset) * mobileWallJumpDirection; // Set jumpOffset for horizontal propulsion
            rb.gravityScale = wallJumpGravity;
            rb.linearVelocity = new Vector2(speed * jumpOffset, jumpSpeed); // Apply horizontal and vertical speed
            onWall = false;
            wallCooldown = wallCooldownMax; // Reset cooldown for wall jump
            grounded = maxJumps; // Allow another jump after wall jump
        }
        else if (grounded < maxJumps)
        {
            Jump(); // Regular jump
        }
    }

    // Optional: Add a button for specific wall disengage (like 'S' key)
    public void OnDisengageWallButtonPressed()
    {
        if (onWall)
        {
            rb.gravityScale = wallJumpGravity;
            onWall = false;
        }
    }
}