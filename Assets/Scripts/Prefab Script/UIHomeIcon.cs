using UnityEngine;
using UnityEngine.UI;

public class UIHomeIcon: Graphic
{
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float w = rectTransform.rect.width;
        float h = rectTransform.rect.height;
        float thickness = w * 0.1f;

        Vector2 bottomLeft = new Vector2(0, 0);
        Vector2 topLeft = new Vector2(0, h);
        Vector2 middleBottom = new Vector2(w / 2, h * 0.3f);
        Vector2 topRight = new Vector2(w, h);
        Vector2 bottomRight = new Vector2(w, 0);

        // 左竖
        DrawLine(vh, bottomLeft, topLeft, thickness);
        // 左斜
        DrawLine(vh, topLeft, middleBottom, thickness);
        // 右斜
        DrawLine(vh, topRight, middleBottom, thickness);
        // 右竖
        DrawLine(vh, topRight, bottomRight, thickness);
    }

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
