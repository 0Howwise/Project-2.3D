using System.Collections;
using UnityEngine;

[AddComponentMenu ("Camera-Control/Mouse Orbit with zoom")]
public class CameraOrbit : MonoBehaviour {

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float xMinLimit = -20f;
    public float xMaxLimit = 80f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;

    public float lastDistance;
    public float desiredDistance;
    public float zoomSpeed;
    bool hasHit;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start () {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody> ();

        // Make the rigid body not change rotation
        if (rigidbody != null) {
            rigidbody.freezeRotation = true;
        }
    }

    void LateUpdate () {
        if (target) {
            x += Input.GetAxis ("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle (y, yMinLimit, yMaxLimit);
            x = ClampAngle (x, xMinLimit, xMaxLimit);

            Quaternion rotation = Quaternion.Euler (y, x, 0);

            desiredDistance = Mathf.Clamp (desiredDistance - Input.GetAxis ("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            distance = Mathf.Lerp(distance, desiredDistance, Time.deltaTime * zoomSpeed);

            RaycastHit hit;
            if (Physics.Linecast (target.position, transform.position, out hit)) {
                desiredDistance -= hit.distance;
                if(hasHit == false){
                    lastDistance = distance;
                    hasHit = true;
                }

            }
            else if(hasHit == true){
                desiredDistance = lastDistance;
                hasHit = false;
            }
            Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle (float angle, float min, float max) {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp (angle, min, max);
    }
}