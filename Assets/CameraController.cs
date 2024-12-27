using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; // Speed of the camera movement
    public Vector2 panLimit;    // Limits for camera movement

    void Update()
    {
        Vector3 pos = transform.position;

        // Move camera with arrow keys or WASD
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
            pos.z += panSpeed * Time.deltaTime;
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
            pos.z -= panSpeed * Time.deltaTime;
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
            pos.x += panSpeed * Time.deltaTime;
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
            pos.x -= panSpeed * Time.deltaTime;

        // Apply movement limits
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}

