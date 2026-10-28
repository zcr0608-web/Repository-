using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class MobileStrikePlayerController : MonoBehaviour
{
    public Transform cameraPivot;
    public float moveSpeed = 5f;
    public float lookSensitivity = 0.12f;
    public float gravity = -18f;
    public Vector2 moveInput;
    public Vector2 lookInput;

    private CharacterController controller;
    private float pitch;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = moveInput.x;
        float v = moveInput.y;
#if UNITY_EDITOR || UNITY_STANDALONE
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButton(1))
        {
            lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 8f;
        }
        else lookInput = Vector2.zero;
#endif
        transform.Rotate(0f, lookInput.x * lookSensitivity, 0f);
        pitch = Mathf.Clamp(pitch - lookInput.y * lookSensitivity, -75f, 75f);
        if (cameraPivot != null) cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        Vector3 move = transform.right * h + transform.forward * v;
        if (move.sqrMagnitude > 1f) move.Normalize();
        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move((move * moveSpeed + Vector3.up * verticalVelocity) * Time.deltaTime);
    }

    public void SetMoveInput(Vector2 value) => moveInput = Vector2.ClampMagnitude(value, 1f);
    public void SetLookInput(Vector2 value) => lookInput = value;
}
