using UnityEngine;
using UnityEngine.EventSystems;

public class MobileStrikeTouchPad : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum PadMode { Move, Look }
    public PadMode mode = PadMode.Move;
    public MobileStrikePlayerController player;
    public RectTransform padArea;
    private Vector2 lastPosition;

    private void Awake()
    {
        if (padArea == null) padArea = transform as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        Apply(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Apply(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (player == null) return;
        if (mode == PadMode.Move) player.SetMoveInput(Vector2.zero);
        else player.SetLookInput(Vector2.zero);
    }

    private void Apply(PointerEventData eventData)
    {
        if (player == null) return;
        if (mode == PadMode.Move)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                padArea, eventData.position, eventData.pressEventCamera, out Vector2 local);
            Vector2 size = padArea.rect.size;
            Vector2 normalized = new Vector2(local.x / Mathf.Max(1f, size.x * 0.5f), local.y / Mathf.Max(1f, size.y * 0.5f));
            player.SetMoveInput(Vector2.ClampMagnitude(normalized, 1f));
        }
        else
        {
            Vector2 delta = eventData.position - lastPosition;
            lastPosition = eventData.position;
            player.SetLookInput(delta);
        }
    }
}
