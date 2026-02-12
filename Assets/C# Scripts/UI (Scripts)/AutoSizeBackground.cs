using TMPro;
using UnityEngine;

[ExecuteAlways]
public class AutoSizeBackground : UpdateMonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float extraSizeDeltaX;
    private float initialRightEdgeX = 20;


    private void Awake()
    {
        if (background != null)
        {
            // Calculate the initial right edge position in local space
            initialRightEdgeX = background.anchoredPosition.x + background.sizeDelta.x * (1 - background.pivot.x);
        }
    }

    protected override void OnUpdate()
    {
        if (background == null || text == null) return;

        // Get current text width
        float textWidth = text.GetRenderedValues(false).x;

        // Update background width
        Vector2 size = background.sizeDelta;
        size.x = textWidth + extraSizeDeltaX;

        background.sizeDelta = size;

        // Keep the **right edge fixed**, adjust anchoredPosition
        float pivotX = background.pivot.x;
        Vector2 anchoredPos = background.anchoredPosition;
        anchoredPos.x = initialRightEdgeX - size.x * (1 - pivotX);
        background.anchoredPosition = anchoredPos;
    }
}
