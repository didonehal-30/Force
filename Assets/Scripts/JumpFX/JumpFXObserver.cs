using UnityEngine;

/// <summary>
/// 监听玩家刚体状态来触发两类跳跃特效：
/// 1) 地面起跳：从接地到离地的第一帧；落点采用对地 Raycast 的命中点，防止在空中生成。
/// 2) 空中二段跳：空中出现明显的向上速度突增（例如二段跳或墙跳）。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class JumpFXObserver : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform footAnchor;         // 脚底定位点（Player 下新建一个空物体）
    [SerializeField] private LayerMask groundMask;         // 地面层（务必把 Ground 所在层加入）
    [SerializeField] private float rayLength = 1.2f;       // 射线长度
    [SerializeField] private float groundOffsetUp = 0.05f; // 生成时向上抬一点，避免埋进地面

    [Header("Jump Detect")]
    [SerializeField] private float takeoffVyThreshold = 3.0f;   // 起跳瞬间Y速度阈值
    [SerializeField] private float suddenBoostThreshold = 2.0f; // 空中二段跳“速度突增”阈值
    [SerializeField] private float fxMinInterval = 0.08f;       // 两次 FX 之间的最小间隔，防抖

    private Rigidbody2D rb;
    private bool wasGrounded;
    private float prevVy;
    private float lastFxTime;
    private JumpFXManager fx; // 场景里的 FX 管理器

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fx = FindFirstObjectByType<JumpFXManager>();
        if (!fx) Debug.LogWarning("JumpFXManager not found in scene.");
        if (!footAnchor) footAnchor = transform; // 兜底
    }

    void Update()
    {
        bool isGrounded;
        Vector3 groundPos;
        CastGround(out isGrounded, out groundPos);

        float vy = rb.linearVelocity.y; // 使用标准 API 以提高兼容性

        // 情形1：从“接地”到“离地”的第一帧，并且向上速度足够（地面起跳）
        if (wasGrounded && !isGrounded && vy > takeoffVyThreshold)
        {
            if (Time.time - lastFxTime > fxMinInterval)
            {
                // 优先使用 Ray 命中点；若无效则用脚底点
                Vector3 spawn = (groundPos.sqrMagnitude > 0f) ? groundPos + Vector3.up * groundOffsetUp : footAnchor.position;
                fx?.PlayGroundJumpFX(spawn);
                lastFxTime = Time.time;
            }
        }

        // 情形2：不接地时，出现明显的Y向上速度突增（空中二段跳/墙跳）
        float dv = vy - prevVy;
        if (!isGrounded && dv > suddenBoostThreshold)
        {
            if (Time.time - lastFxTime > fxMinInterval)
            {
                fx?.PlayAirJumpFX(footAnchor.position);
                lastFxTime = Time.time;
            }
        }

        wasGrounded = isGrounded;
        prevVy = vy;
    }

    /// <summary>
    /// 对脚底发出向下 Raycast，返回是否接地及命中点。
    /// </summary>
    private void CastGround(out bool grounded, out Vector3 hitPoint)
    {
        Vector3 from = footAnchor.position;
        var hit = Physics2D.Raycast(from, Vector2.down, rayLength, groundMask);
        grounded = hit.collider != null;
        hitPoint = grounded ? (Vector3)hit.point : Vector3.zero;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!footAnchor) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(footAnchor.position, footAnchor.position + Vector3.down * rayLength);
    }
#endif
}