using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpVFXListener : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject dustJumpEffectPrefab;   // 地面尘土
    public GameObject airJumpEffectPrefab;    // 空中二段/空踏

    [Header("Spawn Points")]
    public Transform dustSpawnPoint;          // 玩家脚下定位点（必填）
    public Transform airSpawnPoint;           // 空中特效生成点（可用玩家中心/脚下）

    [Header("Ground Check")]
    public LayerMask groundLayers;            // 你的 Ground / Default / KineticObjects 等
    public float groundCheckRadius = 0.12f;   // 脚底圆形检测半径
    public Vector2 groundCheckOffset = new Vector2(0f, -0.2f); // 相对玩家中心的偏移

    [Header("Debounce")]
    public float jumpDetectCooldown = 0.08f;  // 防手抖：按键→判定的最小间隔
    public float airFxCooldown = 0.15f;       // 防止空中误触多次

    private Rigidbody2D rb;
    private bool wasGrounded;                 // 上一帧是否在地面
    private float lastJumpPressTime = -999f;  // 最近一次“按下跳跃”时间
    private float lastAirFxTime   = -999f;    // 最近一次空中特效触发时间

    // 移动端支持：UI Button 可调用这个函数告诉监听器“按下了跳跃”
    public void NotifyJumpPressed()
    {
        lastJumpPressTime = Time.time;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!dustSpawnPoint) dustSpawnPoint = this.transform; // 兜底
        if (!airSpawnPoint)  airSpawnPoint  = this.transform;
    }

    void Update()
    {
        // 1) 监听键鼠“按下跳跃键”
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastJumpPressTime = Time.time;
        }

        // 2) 自己做落地检测
        bool isGrounded = CheckGrounded();

        // 3) 贴墙推断（来自原脚本特征：贴墙时 gravityScale=0 且速度≈0）
        bool onWallLike = (Mathf.Approximately(rb.gravityScale, 0f) && rb.linearVelocity.sqrMagnitude < 0.0001f);

        // 4) 有“跳跃按下”信号 → 判断播哪个特效
        bool recentlyPressed = (Time.time - lastJumpPressTime) <= jumpDetectCooldown;
        if (recentlyPressed)
        {
            if (isGrounded && !onWallLike)
            {
                // 地面跳特效
                SpawnOnce(dustJumpEffectPrefab, dustSpawnPoint.position);
                // 消耗这次触发，避免多播
                lastJumpPressTime = -999f;
            }
            else if (!isGrounded && (Time.time - lastAirFxTime) > airFxCooldown)
            {
                // 空中特效（含二段跳/墙离开后在空中的再次跳跃）
                SpawnOnce(airJumpEffectPrefab, airSpawnPoint.position);
                lastAirFxTime = Time.time;
                lastJumpPressTime = -999f;
            }
        }

        wasGrounded = isGrounded;
    }

    bool CheckGrounded()
    {
        Vector2 center = (Vector2)transform.position + groundCheckOffset;
        // 圆形检测更宽容，避免因为脚形状导致误判
        Collider2D hit = Physics2D.OverlapCircle(center, groundCheckRadius, groundLayers);
        return hit != null;
    }

    void SpawnOnce(GameObject prefab, Vector3 pos)
    {
        if (!prefab) return;
        Instantiate(prefab, pos, Quaternion.identity);
        // Prefab 内请设置 ParticleSystem：Play On Awake 关，使用 Burst，Stop Action=Destroy
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 center = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(center, groundCheckRadius);
    }
}