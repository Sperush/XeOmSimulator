using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform[] player;

    void LateUpdate()
    {
        Vector3 newPos = player[PlayerController.Instance.isMotorbike ? 0:1].position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(90f, player[PlayerController.Instance.isMotorbike ? 0 : 1].eulerAngles.y, 0f); // nếu muốn xoay theo hướng player
    }
}
