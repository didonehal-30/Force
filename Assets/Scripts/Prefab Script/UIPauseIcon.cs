using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIPauseIcon : Graphic
{
    public float barWidth = 10f;
    public float spacing = 10f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float height = rectTransform.rect.height;
        float width = rectTransform.rect.width;

        float totalWidth = 2 * barWidth + spacing;
        float startX = (width - totalWidth) / 2;

        // Left bar
        DrawRect(vh, new Vector2(startX, 0), new Vector2(startX + barWidth, height));
        // Right bar
        DrawRect(vh, new Vector2(startX + barWidth + spacing, 0), new Vector2(startX + 2 * barWidth + spacing, height));
    }

    void DrawRect(VertexHelper vh, Vector2 bottomLeft, Vector2 topRight)
    {
        int index = vh.currentVertCount;

        vh.AddVert(new Vector3(bottomLeft.x, bottomLeft.y), color, Vector2.zero);
        vh.AddVert(new Vector3(bottomLeft.x, topRight.y), color, Vector2.zero);
        vh.AddVert(new Vector3(topRight.x, topRight.y), color, Vector2.zero);
        vh.AddVert(new Vector3(topRight.x, bottomLeft.y), color, Vector2.zero);

        vh.AddTriangle(index, index + 1, index + 2);
        vh.AddTriangle(index, index + 2, index + 3);
    }
}
