using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0f, 4f, -6f);
    public float followSpeed = 8f;

    [Header("Camera Look")]
    public float lookHeight = 1.3f;
    public float rotationSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        // This makes the camera offset rotate with the player
        Vector3 rotatedOffset = target.TransformDirection(offset);

        Vector3 desiredPosition = target.position + rotatedOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Vector3 lookPoint = target.position + Vector3.up * lookHeight;

        Quaternion targetRotation = Quaternion.LookRotation(lookPoint - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}