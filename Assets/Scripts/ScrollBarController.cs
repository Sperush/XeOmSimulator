using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollBarController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background; // Thanh ngang (Panel)
    public RectTransform handle; // Nút tròn (Panel)
    public EnergyBoostController energyBoost;
    private float inputValue; // Giá trị từ -1 (trái) đến 1 (phải)
    private bool isActive; // Trạng thái chạm/giữ
    public float maxOffset = 125f; // Nửa chiều dài thanh ngang
    private Vector2 startTouchPos; // Vị trí chạm ban đầu
    public static ScrollBarController Instance; // Singleton instance
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (background == null || handle == null)
        {
            Debug.LogError("Thiếu tham chiếu background hoặc handle trong " + gameObject.name);
            enabled = false;
            return;
        }
        if (energyBoost == null)
        {
            Debug.LogError("EnergyBoostController reference missing!");
            enabled = false;
            return;
        }
        maxOffset = background.rect.width / 2;
        handle.localPosition = Vector2.zero; // Đặt nút tròn ở giữa
        isActive = false;
        inputValue = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isActive = true;
        CameraFollow.isXoay = false;
        startTouchPos = eventData.position;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive)
        {
            isActive = eventData.position.y - startTouchPos.y <= 250f;
            return;
        }
        // Tính tọa độ tương đối so với tâm background
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            // Kiểm tra vuốt lên
            if (eventData.position.y - startTouchPos.y > 250f) // Vuốt lên 50 pixel
            {
                isActive = false;
                inputValue = 0;
                energyBoost.StopBoost();
                Debug.Log("Vuốt lên: Dừng xe, reset nút tròn");
                return;
            }

            // Giới hạn di chuyển theo trục x
            float x = Mathf.Clamp(localPoint.x, -maxOffset, maxOffset);
            handle.localPosition = new Vector2(x, 0);

            // Chuẩn hóa giá trị (-1 đến 1)
            inputValue = x / maxOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.localPosition = Vector2.zero; // Reset về giữa
        inputValue = 0;
        isActive = false;
    }

    public float GetInputValue()
    {
        return inputValue;
    }

    public bool IsActive()
    {
        return isActive;
    }
}