using UnityEngine;

public class AirStepAuto : MonoBehaviour
{
    public float life = 1.0f;                 // 寿命
    public Vector2 startSize = new(1.2f, 0.25f);
    public Vector2 endSize   = new(0.0f, 0.0f);
    private SpriteRenderer sr;
    private float t;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(startSize.x, startSize.y, 1f);
        if (sr != null)
        {
            var c = sr.color; c.a = 1f; sr.color = c;
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        float u = Mathf.Clamp01(t / life);

        // 尺寸插值
        var s = Vector2.Lerp(startSize, endSize, u);
        transform.localScale = new Vector3(Mathf.Max(0.0001f, s.x), Mathf.Max(0.0001f, s.y), 1f);

        // 透明度插值
        if (sr != null)
        {
            var c = sr.color;
            c.a = 1f - u;
            sr.color = c;
        }

        if (t >= life) Destroy(gameObject);
    }
}