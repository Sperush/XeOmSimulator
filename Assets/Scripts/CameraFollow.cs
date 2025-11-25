using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform[] target;
    public Vector3 offset = new Vector3(0, 25f, -50f); // Tùy chỉnh khoảng cách
    public float smoothSpeed = 10f;
    public float rotationSmoothSpeed = 5f;
    public LayerMask collisionMask;
    public ScrollBarController scrollBar;
    public static bool isXoay;

    private Vector3 currentVelocity;

    private void Start()
    {
        if (target.Length == 0)
        {
            Debug.LogError("Target không được gán trong " + gameObject.name);
            enabled = false;
            return;
        }
        if (scrollBar == null)
        {
            Debug.LogError("ScrollBarController reference missing!");
            enabled = false;
            return;
        }
    }
    void LateUpdate()
    {
        if (target.Length == 0) return;
        Transform taget1 = target[PlayerController.Instance.isMotorbike ? 0:1];
        // Tính offset xoay theo hướng của xe
        Vector3 rotatedOffset = taget1.rotation * offset;
        Vector3 desiredPosition = taget1.position + rotatedOffset;

        // Raycast từ target về phía desiredPosition để phát hiện tường
        RaycastHit hit;
        Vector3 direction = (desiredPosition - taget1.position).normalized;
        float distance = rotatedOffset.magnitude;
        Vector3 finalPosition = desiredPosition;

        if (Physics.SphereCast(taget1.position, 0.5f, direction, out hit, distance, collisionMask))
        {
            finalPosition = hit.point - direction * 0.5f; // Đẩy lùi khỏi tường chút
        }

        // Làm mượt vị trí (dùng SmoothDamp ổn định hơn Lerp)
        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref currentVelocity, 1f / smoothSpeed);

        if (!isXoay)
        {
            // Xoay camera để nhìn vào xe (tâm hơi lệch lên để nhìn từ trên xuống)
            Vector3 lookTarget = taget1.position + Vector3.up; // Nhìn vào phần thân trên xe
            Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothSpeed);
        }
    }
}
