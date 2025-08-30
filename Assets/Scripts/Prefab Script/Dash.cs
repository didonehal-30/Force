using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] float dashSpeed = 18f;
    [SerializeField] float dashDuration = 0.18f;
    [SerializeField] float dashCooldown = 0.5f;
    public KeyCode dashKey = KeyCode.LeftShift;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private int dashDirection = 1;
    private PlayerMovement pm; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {

        if (Input.GetKeyDown(dashKey) && !isDashing && dashCooldownTimer <= 0)
        {

            float input = Input.GetAxisRaw("Horizontal");
            dashDirection = input != 0 ? (int)Mathf.Sign(input) : 1;
            isDashing = true;
            dashTimer = dashDuration;
            pm.externalLockMovement = true;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
                pm.externalLockMovement = false;
            }
        }
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.fixedDeltaTime;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
}
