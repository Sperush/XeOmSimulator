using UnityEngine;

public class MinimapTargetIcon : MonoBehaviour
{
    public Transform[] player;
    public Transform target;
    public Camera minimapCam;
    public float edgeClampPercent = 0.9f; // icon cách mép 10%
    public static MinimapTargetIcon Instance;
    private float iconYoffset = 60f; // icon nằm cao hơn mặt đất

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
    }

    void LateUpdate()
    {
        Vector3 worldOffset = target.position - player[PlayerController.Instance.isMotorbike ? 0 : 1].position;
        worldOffset.y = 0;

        Vector3 iconWorldPos = target.position;

        // Dự đoán target có trong vùng hiển thị minimap không
        Vector3 viewportPos = minimapCam.WorldToViewportPoint(target.position);

        bool isInMinimap =
            viewportPos.z > 0 &&
            viewportPos.x > 0 && viewportPos.x < 1 &&
            viewportPos.y > 0 && viewportPos.y < 1;

        if (isInMinimap)
        {
            // Target trong vùng → đặt icon đúng chỗ
            iconWorldPos = target.position;
        }
        else
        {
            // Target ngoài vùng → kéo icon về rìa minimap

            Vector3 dir = (target.position - player[PlayerController.Instance.isMotorbike ? 0 : 1].position).normalized;
            float dist = 60f; // khoảng cách từ player ra rìa minimap (tùy camera size)

            Vector3 edgeWorldPos = player[PlayerController.Instance.isMotorbike ? 0 : 1].position + dir * dist;
            iconWorldPos = edgeWorldPos;
        }

        // Cập nhật vị trí icon trên minimap
        transform.position = new Vector3(iconWorldPos.x, player[PlayerController.Instance.isMotorbike ? 0 : 1].position.y + iconYoffset, iconWorldPos.z);

        // 👉 Xoay icon đỏ sao cho mũi nhọn trỏ về player (tức icon "đối diện với player")
        Vector3 toPlayer = player[PlayerController.Instance.isMotorbike ? 0 : 1].position - transform.position;
        float angle = Mathf.Atan2(toPlayer.x, toPlayer.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(90f, 0f, -angle); // quay theo mặt phẳng minimap
    }
}
