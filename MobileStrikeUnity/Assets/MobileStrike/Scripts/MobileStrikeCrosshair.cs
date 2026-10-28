using UnityEngine;

public class MobileStrikeCrosshair : MonoBehaviour
{
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;
    public float baseGap = 8f;
    public float shotExpand = 12f;
    public float recoverSpeed = 18f;

    private float currentExpand;

    private void Update()
    {
        currentExpand = Mathf.Lerp(currentExpand, 0f, Time.deltaTime * recoverSpeed);
        ApplyPositions();
    }

    public void OnShot()
    {
        currentExpand = shotExpand;
    }

    private void ApplyPositions()
    {
        if (top != null) top.anchoredPosition = new Vector2(0, baseGap + currentExpand);
        if (bottom != null) bottom.anchoredPosition = new Vector2(0, -baseGap - currentExpand);
        if (left != null) left.anchoredPosition = new Vector2(-baseGap - currentExpand, 0);
        if (right != null) right.anchoredPosition = new Vector2(baseGap + currentExpand, 0);
    }
}
