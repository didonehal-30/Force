using UnityEngine;

public class FloatingMenuController : MonoBehaviour
{
    public RectTransform panelRect;
    public GameObject panelContainer; // 展开时内容
    private bool isExpanded = false;

    // 建议大小（可调）
    private Vector2 collapsedSize = new Vector2(64, 64);
    private Vector2 expandedSize = new Vector2(400, 250);

    void Start()
    {
        Collapse(); // 初始收起
    }

    public void ToggleMenu()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
            Expand();
        else
            Collapse();
    }

    private void Expand()
    {
        panelRect.sizeDelta = expandedSize;
        panelContainer.SetActive(true);
    }

    private void Collapse()
    {
        panelRect.sizeDelta = collapsedSize;
        panelContainer.SetActive(false);
    }
}
