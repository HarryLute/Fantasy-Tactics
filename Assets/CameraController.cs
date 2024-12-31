using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cam;  // Assign the camera transform in the Inspector
    public float moveSpeed = 5f;  // Speed of the camera movement
    public Vector2 mapBoundsX;  // Min and Max X for clamping
    public Vector2 mapBoundsY;  // Min and Max Y for clamping

    void Update()
    {
        HandleCameraMovement();
    }

    public void CenterCameraOnTile(Vector2Int tilePosition)
    {
        if (cam != null)
        {
            cam.position = new Vector3(tilePosition.x, tilePosition.y, cam.position.z);
        }
    }

    private void HandleCameraMovement()
    {
        float moveX = Input.GetAxis("Horizontal");  // Arrow keys or A/D
        float moveY = Input.GetAxis("Vertical");    // Arrow keys or W/S

        if (cam != null)
        {
            Vector3 newPosition = cam.position + new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

            // Clamp camera position to the map bounds
            newPosition.x = Mathf.Clamp(newPosition.x, mapBoundsX.x, mapBoundsX.y);
            newPosition.y = Mathf.Clamp(newPosition.y, mapBoundsY.x, mapBoundsY.y);

            cam.position = newPosition;
        }
    }
}

