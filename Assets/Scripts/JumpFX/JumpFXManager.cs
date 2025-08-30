using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFXManager : MonoBehaviour
{
    [Header("Prefabs")]
    public ParticleSystem groundDustPrefab; // 地面尘土
    public GameObject airStepPrefab;        // 空中虚化平台（SpriteRenderer）

    [Header("Air Step")]
    public float airStepDuration = 0.35f;
    public Vector2 airStepSize = new Vector2(1.2f, 0.25f);
    public AnimationCurve airStepScaleCurve = AnimationCurve.EaseInOut(0,1,1,0);
    public AnimationCurve airStepAlphaCurve = AnimationCurve.Linear(0,1,1,0);
    public bool useUnscaledTime = false;          // 新增：受 Time.timeScale 影响时也能按时结束
    public float autoHideBuffer = 0.05f;          // 新增：兜底强制隐藏缓冲

    [Header("Ground Dust")]
    public Vector2 groundOffset = new(0f, -0.02f); // 落点微调（更贴地）
    public float groundDustScale = 1f;             // 范围太大就调小它

    private readonly List<ParticleSystem> dustPool = new();
    private readonly List<GameObject> airPool = new();

    public void PlayGroundJumpFX(Vector3 pos)
    {
        if (!groundDustPrefab) return;
        var fx = GetDust();
        fx.transform.position = pos + (Vector3)groundOffset;
        fx.transform.localScale = Vector3.one * Mathf.Max(0.01f, groundDustScale);
        fx.Clear(true);
        fx.Play(true);
        StartCoroutine(RecycleDustWhenStopped(fx));
    }

    public void PlayAirJumpFX(Vector3 pos)
    {
        if (!airStepPrefab) return;
        var go = GetAirStep();
        go.transform.position = pos + Vector3.down * 0.05f;
        go.transform.localScale = new Vector3(airStepSize.x, airStepSize.y, 1f);

        var sr = go.GetComponentInChildren<SpriteRenderer>(true);
        if (sr) SetAlpha(sr, 1f);

        go.SetActive(true);
        // 同时开两个协程：一个做渐隐动画；一个兜底强制隐藏
        StartCoroutine(AirStepLife(go, sr));
        StartCoroutine(ForceDisableAfter(go, airStepDuration + autoHideBuffer)); // 兜底
    }

    // —— 对象池：地面尘土 ——
    ParticleSystem GetDust()
    {
        if (dustPool.Count > 0)
        {
            var fx = dustPool[^1];
            dustPool.RemoveAt(dustPool.Count - 1);
            return fx;
        }
        return Instantiate(groundDustPrefab);
    }
    IEnumerator RecycleDustWhenStopped(ParticleSystem fx)
    {
        while (fx && fx.IsAlive(true)) yield return null;
        if (fx) dustPool.Add(fx);
    }

    // —— 对象池：空中平台 ——
    GameObject GetAirStep()
    {
        if (airPool.Count > 0)
        {
            var go = airPool[^1];
            airPool.RemoveAt(airPool.Count - 1);
            return go;
        }
        return Instantiate(airStepPrefab);
    }

    IEnumerator AirStepLife(GameObject go, SpriteRenderer sr)
    {
        float t = 0f;
        Vector3 baseScale = go.transform.localScale;
        while (t < airStepDuration && go)              // 防止运行中被销毁
        {
            float u = Mathf.Clamp01(t / Mathf.Max(0.0001f, airStepDuration));
            float ks = airStepScaleCurve.Evaluate(u);
            float ka = airStepAlphaCurve.Evaluate(u);

            go.transform.localScale = baseScale * Mathf.Max(0.0001f, ks);
            if (sr) SetAlpha(sr, ka);

            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            yield return null;
        }
        if (go)
        {
            go.SetActive(false);
            airPool.Add(go);
        }
    }

    // 兜底：无论动画是否执行到位，到时强制隐藏
    IEnumerator ForceDisableAfter(GameObject go, float seconds)
    {
        if (useUnscaledTime)
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, seconds));
        else
            yield return new WaitForSeconds(Mathf.Max(0f, seconds));

        if (go && go.activeSelf)
        {
            go.SetActive(false);
            airPool.Add(go);
        }
    }

    static void SetAlpha(SpriteRenderer sr, float a)
    {
        var c = sr.color; c.a = a; sr.color = c;
    }
}