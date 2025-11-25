using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragRotate : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    public Rigidbody[] rb;

    private void Start()
    {
        if (rb == null || rb.Length == 0)
        {
            Debug.LogError("Rigidbody không được gán trong " + gameObject.name);
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 screenPos = touch.position;

            // Tránh UI và kiểm tra vùng xoay
            if (IsTouchOverUI() || !IsInLookRegion(screenPos) || rb[PlayerController.Instance.isMotorbike ? 0:1].velocity.magnitude >= 1.7f) return;

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = screenPos;
                isDragging = true;
                CameraFollow.isXoay = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = screenPos - lastTouchPosition;
                float yaw = delta.x * rotationSpeed;
                float pitch = -delta.y * rotationSpeed;

                transform.Rotate(Vector3.up, yaw, Space.World);
                transform.Rotate(Vector3.right, pitch, Space.Self);

                lastTouchPosition = screenPos;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    bool IsInLookRegion(Vector2 screenPos)
    {
        return screenPos.y > Screen.height * 0.25f && screenPos.y < Screen.height * 0.75f;
    }

    bool IsTouchOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
}
