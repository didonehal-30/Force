using UnityEngine;

/// <summary>
/// Grab‐to‐move platform (Celeste-style).
/// • Dashes from Start → End when the player touches it, then glides back.
/// • Use Rigidbody2D.MovePosition inside FixedUpdate so the platform moves within the physics step and other rigidbodies collide with it correctly, whereas setting transform.position would teleport the collider and miss fast-moving interactions
/// • We cache the start/end points in world space during Awake because if they stayed as child transforms they’d follow the platform, causing the target to drift and the platform to “chase” a moving goal.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(LineRenderer))]
public class StandMovingPlatform : MonoBehaviour
{
    [Header("Options")]
    public bool alwaysMove = false;
    [Header("Visuals / Debug")]
    public bool hideLineInPlay = false; 
    [Header("Path (child objects)")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Speed")]
    public float moveSpeed   = 30f;   // forward dash
    public float returnSpeed =  2f;   // back glide

    // cached world-space targets
    private Vector2 startWorld, endWorld;

    private bool triggered;   // moving to End
    private bool returning;   // moving back

    private Rigidbody2D  rb;
    private LineRenderer line;

    // player stick-on logic
    private Vector2   lastPos;
    private bool      playerIsGrabbing;
    private GameObject player;
    

    
    void Awake()
    {
        // ----- cache path -----
        startWorld = startPoint.position;
        endWorld   = endPoint.position;

        // hide helper points in play mode
        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);

        rb   = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        line.enabled = !hideLineInPlay;  

        // draw path once
        line.positionCount = 2;
        line.startWidth = line.endWidth = 0.08f;
        line.useWorldSpace = true;
        line.SetPositions(new Vector3[] { startWorld, endWorld });

        lastPos = rb.position;
    }

    // physics step
    void FixedUpdate()
    {
        if ((triggered||alwaysMove) && !returning)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, endWorld, moveSpeed * Time.fixedDeltaTime));

            if (Vector2.SqrMagnitude(rb.position - endWorld) < 0.04f)
            {
                rb.position = endWorld;
                triggered   = false;
                returning   = true;
            }
        }
        else if (returning)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, startWorld, returnSpeed * Time.fixedDeltaTime));

            if (Vector2.SqrMagnitude(rb.position-startWorld) < 0.04f)
            {
                rb.position = startWorld;
                returning   = false;
            }
        }
    }

    // stick player to platform
    void LateUpdate()
    {
        if (playerIsGrabbing && player)
        {
            Vector2 delta = rb.position - lastPos;
            player.transform.position += (Vector3)delta;
        }
        lastPos = rb.position;
    }

    private const float TOP_NORMAL_THRESHOLD = -0.2f;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
            return;
        bool cameFromTop = false;
        foreach (var c in col.contacts)         
        {
            if (c.normal.y <= TOP_NORMAL_THRESHOLD)
            {
                cameFromTop = true;              
                break;                           
            }
        }
        if (!cameFromTop) return; 

        if ((!triggered && !returning) || alwaysMove)
        {
            triggered = true;
            playerIsGrabbing = true;
            player = col.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerIsGrabbing = false;
            player           = null;
        }
    }
}
