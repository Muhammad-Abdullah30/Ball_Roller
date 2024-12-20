using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Variables Intialize")]
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed=5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(target == null)return;
        
    }

    // Update is called once per frame
    void Update()
    {


        
    }
    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Optionally look at the ball
        transform.LookAt(target);
    }

}

