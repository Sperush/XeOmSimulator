using UnityEngine;
using TMPro;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using static TrafficLightController;

public class MotorbikeController : MonoBehaviour
{
    [Header("Wheels")]
    public Transform frontRight;
    public Transform frontLeft;
    public Transform rearRight;
    public Transform rearLeft;

    //Wheel colliders
    public WheelCollider frontRightCollider;
    public WheelCollider frontLeftCollider;
    public WheelCollider rearRightCollider;
    public WheelCollider rearLeftCollider;
    public ScrollBarController scrollBar;
    public TrafficLightController trafficLight;
    public float maxSpeed = 15f;
    public float acceleration = 50f; // Tăng lực tiến
    public float rotationSpeed = 50f;
    public float brakeForce = 1000f;
    public EnergyBoostController energyBoost; // Thêm Energy Boost
    public float boostAcceleration = 60f; // Tăng tốc khi boost
    public float decelerationTime = 3f; // Thời gian chậm dần (3 giây)
    private float decelerationTimer; // Bộ đếm thời gian
    private bool isDecelerating; // Trạng thái chậm dần
    private Vector3 initialVelocity; // Lưu vận tốc ban đầu khi bắt đầu giảm tốc
    private Rigidbody rb;
    private Vector3 appliedForce;
    public float TimeSub = 1f;
    private float timer = 0f;
    private bool touchingOnlyPlane = false;
    public Transform handlebar; // Cylinder002
    public float handlebarMaxRotation = 45f; // Góc xoay tối đa của tay lái


    private HashSet<Collider> currentColliders = new HashSet<Collider>();
    private List<string> trafficCheckOrder = new List<string>();

    public static MotorbikeController Instance;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody không được gắn vào " + gameObject.name);
            enabled = false;
            return;
        }
        if (scrollBar == null)
        {
            Debug.LogError("Thiếu tham chiếu ScrollBarController!");
            enabled = false;
            return;
        }
        if (energyBoost == null)
        {
            Debug.LogError("EnergyBoostController reference missing!");
            enabled = false;
            return;
        }
        //initialVelocity = Vector3.zero;
    }

    private void WheelUpdate(Transform transform, WheelCollider collider)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        if(!PlayerController.Instance.isMotorbike)
            transform.rotation = rot; // Chỉ xoay theo trục Y nếu không phải xe máy
        else
            transform.rotation = rot * Quaternion.Euler(0f, 90, 0f);
    }

    void FixedUpdate()
    {
        WheelUpdate(frontRight, frontRightCollider);
        WheelUpdate(rearRight, rearRightCollider);
        if (!PlayerController.Instance.isMotorbike)
        {
            WheelUpdate(frontLeft, frontLeftCollider);
            WheelUpdate(rearLeft, rearLeftCollider);
        }
        if (rb == null || scrollBar == null || energyBoost == null) return;

        if (scrollBar.IsActive())
        {
            // --- Đang tăng tốc ---
            float currentAcceleration = energyBoost.IsBoosting() ? boostAcceleration : acceleration;
            appliedForce = transform.forward * currentAcceleration;
            rb.AddForce(appliedForce, ForceMode.Acceleration);

            // Xoay xe
            float steer = scrollBar.GetInputValue();
            float targetAngle = steer * rotationSpeed;
            Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + targetAngle * Time.fixedDeltaTime, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

            // Gán góc lái cho bánh trước
            frontRightCollider.steerAngle = steer * handlebarMaxRotation;
            if (PlayerController.Instance.isMotorbike)
            {
                handlebar.localRotation = Quaternion.Euler(-65.304f, 95.623f + steer * handlebarMaxRotation, -92.744f);
            } else {
                frontLeftCollider.steerAngle = steer * handlebarMaxRotation;
            }

            // Làm mượt hướng velocity
            if (rb.velocity.magnitude > 0.1f)
            {
                Vector3 newVelocityDir = transform.forward * new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(newVelocityDir.x, rb.velocity.y, newVelocityDir.z), 0.1f);
            }

            // Tắt phanh
            frontRightCollider.brakeTorque = 0f;
            if (!PlayerController.Instance.isMotorbike)
            {
                frontLeftCollider.brakeTorque = 0f;
                rearLeftCollider.brakeTorque = 0f;
            }
            rearRightCollider.brakeTorque = 0f;

            isDecelerating = false;
            decelerationTimer = 0f;
            initialVelocity = Vector3.zero;
        }
        else
        {
            // --- Không tăng tốc: bắt đầu giảm tốc ---
            if (!isDecelerating)
            {
                isDecelerating = true;
                decelerationTimer = 0f;
                initialVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }

            decelerationTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(decelerationTimer / decelerationTime);
            Vector3 targetVelocity = Vector3.Lerp(initialVelocity, Vector3.zero, t);
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

            // Áp lực phanh lên tất cả bánh
            float brake = brakeForce * t;
            if (!PlayerController.Instance.isMotorbike)
            {
                frontLeftCollider.brakeTorque = brake;
                rearLeftCollider.brakeTorque = brake;
            }
            frontRightCollider.brakeTorque = brake;
            rearRightCollider.brakeTorque = brake;

            appliedForce = Vector3.zero;

            if (t >= 1f)
            {
                energyBoost.StopBoost();
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
        }

        // Giới hạn tốc độ
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bool isCollision = false;
        string textnotice = "";
        if (collision.gameObject.CompareTag("car") || collision.gameObject.CompareTag("human") || collision.gameObject.CompareTag("wall"))
        {
            isCollision = true;
            textnotice = "-10 điểm cảm xúc vì va chạm với chướng ngại vật!";
        } else if (collision.gameObject.CompareTag("dog"))
        {
            isCollision = true;
            textnotice = "-10 điểm cảm xúc vì bị chó đuổi kịp!";
        }
        if (isCollision)
        {
            GameControl.Instance.ShowNotice(textnotice);
            GameControl.Instance.StatesEnergy.SubEnergy(10);
            DogController.Instance.ResetDog();
        }
    }

    private void Update()
    {
        if (touchingOnlyPlane)
        {
            timer += Time.deltaTime;
            if (timer >= TimeSub)
            {
                GameControl.Instance.StatesEnergy.SubEnergy(3);
                GameControl.Instance.ShowNotice("-5 điểm cảm xúc/s vì đi trên vỉa hè!");
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!energyBoost.isFullEnergy())
        {
            if (other.CompareTag("energy"))
            {
                energyBoost.addEnergy(10);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("energy1"))
            {
                energyBoost.addEnergy(30);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("energy2"))
            {
                energyBoost.addEnergy(20);
                Destroy(other.gameObject);
            }
        }
        if (trafficLight.currentState != LightState.Red) return; // Chỉ xử lý khi đèn đỏ

        if (other.CompareTag("TrafficCheck1") || other.CompareTag("TrafficCheck2"))
        {
            string tag = other.tag;

            // Tránh ghi trùng tag nếu đã có
            if (!trafficCheckOrder.Contains(tag))
            {
                trafficCheckOrder.Add(tag);
                CheckTrafficViolation();
            }
        }
    }

    private void CheckTrafficViolation()
    {
        // Chỉ cần xét nếu có ít nhất 1 vùng đã chạm
        if (trafficCheckOrder.Count == 0) return;

        // Trường hợp chỉ chạm TrafficCheck1
        if (trafficCheckOrder.Count == 1 && trafficCheckOrder[0] == "TrafficCheck1")
        {
            TriggerPenalty("-15 điểm cảm xúc vì vượt đèn đỏ!");
        }
    }

    private void TriggerPenalty(string message)
    {
        GameControl.Instance.ShowNotice(message);
        GameControl.Instance.StatesEnergy.SubEnergy(10);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null && (other.CompareTag("Plane") || other.CompareTag("road") || other.CompareTag("QuestGiver")))
        {
            currentColliders.Add(other);
            bool onPlane = false;
            bool onRoad = false;
            foreach (var col in currentColliders)
            {
                if (col.CompareTag("Plane")) onPlane = true;
                if (col.CompareTag("road")) onRoad = true;
                if (col.CompareTag("road")) onRoad = true;
            }

            // Chỉ trừ khi đang chạm plane mà KHÔNG chạm road
            touchingOnlyPlane = onPlane && !onRoad;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TrafficCheck1") || other.CompareTag("TrafficCheck2"))
        {
            trafficCheckOrder.Remove(other.tag);
        }
        currentColliders.Remove(other);
        // Reset nếu không còn chạm gì
        if (currentColliders.Count == 0)
        {
            touchingOnlyPlane = false;
            timer = 0f;
        }
    }
}