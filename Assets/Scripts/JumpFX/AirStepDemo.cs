using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AirStepDemo : MonoBehaviour
{
    [Header("Refs")]
    public Transform footAnchor;          
    public GameObject airStepPrefab;      
    public GameObject groundStepPrefab;   

    [Header("Colors (共用 prefab 时生效)")]
    public Color groundStepColor = new Color(0.9f, 0.9f, 0.9f, 1f); // 白色
    public Color airStepColor    = new Color(0.3f, 0.8f, 1f, 1f);    // 蓝色

    [Header("Ground Check")]
    public LayerMask groundMask;          
    public float groundRayLength = 0.15f; 
    public Vector2 groundOffset = new(0f, -0.05f);

    [Header("Double Jump Control")]
    public int maxAirSteps = 1;           
    public int airStepCount = 0;          
    public float pressCooldown = 0.08f;   
    public float airFxCooldown = 0.25f;   

    private Rigidbody2D rb;
    private float lastPressTime = -999f;
    private float lastAirFxTime = -999f;
    private bool wasGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!footAnchor) footAnchor = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            lastPressTime = Time.time;

        bool isGrounded = CheckGrounded();

        // 落地 → 清空计数
        if (!wasGrounded && isGrounded)
        {
            airStepCount = 0;
            lastPressTime = -999f;
        }

        bool recentlyPressed = (Time.time - lastPressTime) <= pressCooldown;

        // 地面起跳
        if (wasGrounded && !isGrounded && recentlyPressed)
        {
            SpawnGroundStep(footAnchor.position);
            lastPressTime = -999f;
        }

        // 空中二段跳
        if (!isGrounded && recentlyPressed && airStepCount < maxAirSteps && (Time.time - lastAirFxTime) >= airFxCooldown)
        {
            SpawnAirStep(footAnchor.position);
            airStepCount++;
            lastAirFxTime = Time.time;
            lastPressTime = -999f;
        }

        wasGrounded = isGrounded;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || InGroundLayer(collision.collider.gameObject.layer))
        {
            airStepCount = 0;
            lastPressTime = -999f;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || InGroundLayer(collision.collider.gameObject.layer))
        {
            if (airStepCount != 0) airStepCount = 0;
        }
    }

    bool InGroundLayer(int layer) => ((1 << layer) & groundMask) != 0;

    bool CheckGrounded()
    {
        Vector2 from = (Vector2)(footAnchor ? footAnchor.position : transform.position) + groundOffset;
        var hit = Physics2D.Raycast(from, Vector2.down, groundRayLength, groundMask);
        Debug.DrawRay(from, Vector2.down * groundRayLength, Color.yellow);
        return hit.collider != null;
    }

    void SpawnGroundStep(Vector3 pos)
    {
        GameObject prefab = groundStepPrefab ? groundStepPrefab : airStepPrefab;
        if (!prefab) return;
        var go = Instantiate(prefab, pos, Quaternion.identity);
        TintIfPossible(go, groundStepColor);
    }

    void SpawnAirStep(Vector3 pos)
    {
        if (!airStepPrefab) return;
        var go = Instantiate(airStepPrefab, pos, Quaternion.identity);
        TintIfPossible(go, airStepColor);
    }

    void TintIfPossible(GameObject go, Color c)
    {
        var sr = go.GetComponentInChildren<SpriteRenderer>();
        if (sr) sr.color = c;

        var ps = go.GetComponentInChildren<ParticleSystem>();
        if (ps)
        {
            var main = ps.main;
            main.startColor = c;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!footAnchor) return;
        Gizmos.color = Color.yellow;
        Vector2 from = (Vector2)footAnchor.position + groundOffset;
        Gizmos.DrawLine(from, from + Vector2.down * groundRayLength);
    }
#endif
}