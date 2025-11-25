using UnityEngine;
using TMPro;
using System.Collections;

public class TrafficLightController : MonoBehaviour
{
    public Renderer redLight;
    public Renderer yellowLight;
    public Renderer greenLight;
    public TMP_Text timerText;

    public float redDuration = 5f;
    public float yellowDuration = 3f;
    public float greenDuration = 5f;

    public Color redOn = Color.red;
    public Color redOff = new Color(0.2f, 0f, 0f); // Đỏ tối  

    public Color yellowOn = Color.yellow;
    public Color yellowOff = new Color(0.2f, 0.2f, 0f); // Vàng tối  

    public Color greenOn = Color.green;
    public Color greenOff = new Color(0f, 0.2f, 0f); // Xanh tối  

    private float timer;
    public enum LightState { Red, Yellow, Green } // Changed to public  
    public LightState currentState;

    private bool isWaitingAtZero = false;

    void Start()
    {
        currentState = LightState.Red;
        timer = redDuration;
        UpdateLights();
    }

    void Update()
    {
        if (!isWaitingAtZero)
        {
            timer -= Time.deltaTime;
        }

        int displayTime = Mathf.Max(0, Mathf.CeilToInt(timer));
        timerText.text = displayTime.ToString("00");

        if (timer <= 0f && !isWaitingAtZero)
        {
            isWaitingAtZero = true;
            StartCoroutine(ShowZeroThenSwitch());
        }
    }

    IEnumerator ShowZeroThenSwitch()
    {
        // Đảm bảo hiện "00" ít nhất 1 frame + delay 0.5 giây cho người nhìn thấy rõ  
        yield return new WaitForSeconds(0.5f);

        SwitchToNextState();
        isWaitingAtZero = false;
    }

    void SwitchToNextState()
    {
        switch (currentState)
        {
            case LightState.Red:
                currentState = LightState.Green;
                timer = greenDuration;
                break;
            case LightState.Green:
                currentState = LightState.Yellow;
                timer = yellowDuration;
                break;
            case LightState.Yellow:
                currentState = LightState.Red;
                timer = redDuration;
                break;
        }

        UpdateLights();
    }

    void UpdateLights()
    {
        redLight.material.color = currentState == LightState.Red ? redOn : redOff;
        yellowLight.material.color = currentState == LightState.Yellow ? yellowOn : yellowOff;
        greenLight.material.color = currentState == LightState.Green ? greenOn : greenOff;
    }
}
