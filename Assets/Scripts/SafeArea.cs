using UnityEngine;

[ExecuteAlways]
public class SafeAreaFitter : MonoBehaviour
{
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) return;

        // ✳️ Fix lỗi chia 0 → NaN
        if (Screen.width == 0 || Screen.height == 0)
        {
            Debug.LogWarning("SafeAreaFitter: Screen width or height is 0. Skipping safe area apply.");
            return;
        }

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Gán anchor
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // ✳️ Reset lại position nếu đang là NaN
        Vector3 pos = rectTransform.position;
        if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
        {
            rectTransform.position = Vector3.zero;
            Debug.LogWarning("SafeAreaFitter: Đã phát hiện position NaN và reset về (0,0,0)");
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
            ApplySafeArea();
    }
#endif
}
