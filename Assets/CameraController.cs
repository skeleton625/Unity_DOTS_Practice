using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lookSpeedH = 2f;
    public float lookSpeedV = 2f;
    public float zoomSpeed = 2f;
    public float dragSpeed = 5f;

    private Vector3 moveOffset = Vector3.zero;
    private Vector3 rotateOffset = Vector3.zero;
    private void Start()
    {
        // x - right    pitch = rotateOffset.x
        // y - up       yaw = rotateOffset.y
        // z - forward  roll
        rotateOffset.y = transform.eulerAngles.y;
        rotateOffset.x = transform.eulerAngles.x;
    }

    private void Update()
    {
        if (!enabled) return;

        if(Input.GetMouseButton(1))
        {
            rotateOffset.y += lookSpeedH * Input.GetAxis("Mouse X");
            rotateOffset.x -= lookSpeedV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = rotateOffset;

            moveOffset = Vector3.zero;
            float offsetDelta = Time.deltaTime * dragSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) offsetDelta *= 5.0f;
            if (Input.GetKey(KeyCode.S)) moveOffset.z -= offsetDelta;
            if (Input.GetKey(KeyCode.W)) moveOffset.z += offsetDelta;
            if (Input.GetKey(KeyCode.A)) moveOffset.x -= offsetDelta;
            if (Input.GetKey(KeyCode.D)) moveOffset.x += offsetDelta;
            if (Input.GetKey(KeyCode.Q)) moveOffset.y -= offsetDelta;
            if (Input.GetKey(KeyCode.E)) moveOffset.y += offsetDelta;

            transform.Translate(moveOffset, Space.Self);
        }

        if(Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxis("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }

        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
    }
}
