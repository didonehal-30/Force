using UnityEngine;
using UnityEngine.UI;

public class LevelStatusIcon : Graphic
{
    public enum IconState
    {
        None,
        Checkmark,
        Circle,
        Locked
    }

    public IconState currentState = IconState.None;

    public void SetState(IconState state)
    {
        currentState = state;
        SetVerticesDirty(); // 通知 Unity 重绘
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float w = rectTransform.rect.width;
        float h = rectTransform.rect.height;

        switch (currentState)
        {
            case IconState.Checkmark:
                DrawCheckmark(vh, w, h);
                break;
            case IconState.Circle:
                DrawCircle(vh, w, h, 32, 4f);
                break;
            case IconState.Locked:
                DrawTriangle(vh, w, h);
                break;
            case IconState.None:
            default:
                break;
        }
    }

    // ✅ 绘制 √
    void DrawCheckmark(VertexHelper vh, float w, float h)
    {
        Vector2 p0 = new Vector2(w * 0.2f, h * 0.5f);
        Vector2 p1 = new Vector2(w * 0.4f, h * 0.3f);
        Vector2 p2 = new Vector2(w * 0.8f, h * 0.7f);
        float thickness = 5f;

        DrawLine(vh, p0, p1, thickness);
        DrawLine(vh, p1, p2, thickness);
    }

    // ✅ 绘制圆形边框
    void DrawCircle(VertexHelper vh, float w, float h, int segments, float thickness)
    {
        float radius = Mathf.Min(w, h) / 2 - thickness;
        Vector2 center = new Vector2(w / 2, h / 2);

        float angleStep = 2 * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle0 = i * angleStep;
            float angle1 = (i + 1) * angleStep;

            Vector2 inner0 = center + new Vector2(Mathf.Cos(angle0), Mathf.Sin(angle0)) * (radius - thickness / 2);
            Vector2 outer0 = center + new Vector2(Mathf.Cos(angle0), Mathf.Sin(angle0)) * (radius + thickness / 2);
            Vector2 inner1 = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * (radius - thickness / 2);
            Vector2 outer1 = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * (radius + thickness / 2);

            int idx = vh.currentVertCount;

            vh.AddVert(inner0, color, Vector2.zero);
            vh.AddVert(outer0, color, Vector2.zero);
            vh.AddVert(outer1, color, Vector2.zero);
            vh.AddVert(inner1, color, Vector2.zero);

            vh.AddTriangle(idx, idx + 1, idx + 2);
            vh.AddTriangle(idx, idx + 2, idx + 3);
        }
    }

    // ✅ 绘制倒三角形
    void DrawTriangle(VertexHelper vh, float w, float h)
    {
        // 计算倒三角顶点位置（适配 100×100 宽高）
        Vector2 top = new Vector2(w / 2f, h * 0.25f);       // 上顶点
        Vector2 bottomLeft = new Vector2(w * 0.3f, h * 0.75f);
        Vector2 bottomRight = new Vector2(w * 0.7f, h * 0.75f);

        int idx = vh.currentVertCount;

        vh.AddVert(top, color, Vector2.zero);
        vh.AddVert(bottomLeft, color, Vector2.zero);
        vh.AddVert(bottomRight, color, Vector2.zero);

        vh.AddTriangle(idx, idx + 1, idx + 2);
    }


    // ✅ 绘制矩形
    void DrawRect(VertexHelper vh, Vector2 bl, Vector2 tr)
    {
        int idx = vh.currentVertCount;

        vh.AddVert(new Vector3(bl.x, bl.y), color, Vector2.zero);
        vh.AddVert(new Vector3(bl.x, tr.y), color, Vector2.zero);
        vh.AddVert(new Vector3(tr.x, tr.y), color, Vector2.zero);
        vh.AddVert(new Vector3(tr.x, bl.y), color, Vector2.zero);

        vh.AddTriangle(idx, idx + 1, idx + 2);
        vh.AddTriangle(idx, idx + 2, idx + 3);
    }

    // ✅ 绘制线段
    void DrawLine(VertexHelper vh, Vector2 start, Vector2 end, float thickness)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x) * thickness / 2f;

        Vector2 v0 = start + normal;
        Vector2 v1 = start - normal;
        Vector2 v2 = end - normal;
        Vector2 v3 = end + normal;

        int idx = vh.currentVertCount;

        vh.AddVert(v0, color, Vector2.zero);
        vh.AddVert(v1, color, Vector2.zero);
        vh.AddVert(v2, color, Vector2.zero);
        vh.AddVert(v3, color, Vector2.zero);

        vh.AddTriangle(idx, idx + 1, idx + 2);
        vh.AddTriangle(idx, idx + 2, idx + 3);
    }
}
