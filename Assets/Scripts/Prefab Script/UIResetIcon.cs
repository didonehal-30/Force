using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIResetIcon : Graphic
{
    public float radius = 40f;
    public float thickness = 10f;
    public int segments = 24;
    public float arcAngle = 270f; // degrees
    public float arrowSize = 14f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float centerX = rectTransform.rect.width / 2;
        float centerY = rectTransform.rect.height / 2;
        Vector2 center = new Vector2(centerX, centerY);

        float angleStep = arcAngle / segments;
        float startAngle = -arcAngle / 2;

        for (int i = 0; i < segments; i++)
        {
            float angle0 = Mathf.Deg2Rad * (startAngle + i * angleStep);
            float angle1 = Mathf.Deg2Rad * (startAngle + (i + 1) * angleStep);

            Vector2 p0Inner = center + new Vector2(Mathf.Cos(angle0), Mathf.Sin(angle0)) * (radius - thickness / 2);
            Vector2 p0Outer = center + new Vector2(Mathf.Cos(angle0), Mathf.Sin(angle0)) * (radius + thickness / 2);
            Vector2 p1Inner = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * (radius - thickness / 2);
            Vector2 p1Outer = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * (radius + thickness / 2);

            int index = vh.currentVertCount;
            vh.AddVert(p0Inner, color, Vector2.zero);
            vh.AddVert(p0Outer, color, Vector2.zero);
            vh.AddVert(p1Outer, color, Vector2.zero);
            vh.AddVert(p1Inner, color, Vector2.zero);

            vh.AddTriangle(index, index + 1, index + 2);
            vh.AddTriangle(index, index + 2, index + 3);
        }

        // Draw arrow head at the end of arc
        float endAngle = Mathf.Deg2Rad * (startAngle + arcAngle);
        Vector2 arrowBase = center + new Vector2(Mathf.Cos(endAngle), Mathf.Sin(endAngle)) * (radius + thickness / 2);

        Vector2 dir = (arrowBase - center).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x);

        Vector2 tip = arrowBase + dir * arrowSize;
        Vector2 left = arrowBase + perp * (arrowSize / 2);
        Vector2 right = arrowBase - perp * (arrowSize / 2);

        int arrowIndex = vh.currentVertCount;
        vh.AddVert(tip, color, Vector2.zero);
        vh.AddVert(left, color, Vector2.zero);
        vh.AddVert(right, color, Vector2.zero);
        vh.AddTriangle(arrowIndex, arrowIndex + 1, arrowIndex + 2);
    }
}
