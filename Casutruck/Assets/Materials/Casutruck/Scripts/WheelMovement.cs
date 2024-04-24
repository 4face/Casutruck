using UnityEngine;

public class WheelMovement : MonoBehaviour
{

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backLeft;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backLeftTransform;

    [Header("Movement Settings")]
    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float maxAcceleration = 1800f;
    [SerializeField] private const float maxTurnAngle = 15f;
    [SerializeField][Range(0f, 1f)] private float wheelRotationTime = 1f;

    [Header("Procedural Generation")]
    [SerializeField] private ProceduralGeneration PG;
    [SerializeField] private Car car;

    private float currentAcceleration = 0f;
    private float currentTurnAngle = 0f;
    private float turnDirection = 0f;

    void FixedUpdate()
    {
        if (!UIController.played) return; // If game not started - return;

        UpdateAcceleration();

        UpdateSteering();

        UpdateWheelTransforms();
    }

    private void UpdateAcceleration() // Moving a car using a progression that depends on the number of locations passed
    {
        currentAcceleration = Mathf.Clamp(acceleration + PG.passedLocations * 5, 0, maxAcceleration);
        Debug.Log(currentAcceleration);

        if (car.number == 0) // something wrong with my first car, that's why
        {
            frontRight.motorTorque = -currentAcceleration;
            frontLeft.motorTorque = -currentAcceleration;
        }
        else
        {
            frontRight.motorTorque = currentAcceleration;
            frontLeft.motorTorque = currentAcceleration;
        }
    }

    private void UpdateSteering() /* When a person presses the screen, the position of the press is calculated and the screen is divided in half,
                                     the side to which the press is closer = the side the machine will turn towards.
                                     Thanks to Mathf.Lerp, we make the turn less sharp if the click was closer to the center
                                   */
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 pressPosition = Input.mousePosition;
            float distanceFromCenter = Mathf.Abs(pressPosition.x - Screen.width / 2) / (Screen.width / 2);
            turnDirection = pressPosition.x < Screen.width / 2 ?
                            Mathf.Lerp(-0.1f, -1f, distanceFromCenter) :
                            Mathf.Lerp(0.1f, 1f, distanceFromCenter);
        }
        else
        {
            turnDirection = 0;
        }

        currentTurnAngle = maxTurnAngle * turnDirection;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
    }

    private void UpdateWheelTransforms() // combines all WheelsUpdates for each WheelCollider and their mesh
    {
        WheelUpdate(frontLeft, frontLeftTransform);
        WheelUpdate(frontRight, frontRightTransform);
        WheelUpdate(backLeft, backLeftTransform);
        WheelUpdate(backRight, backRightTransform);
    }

    private void WheelUpdate(WheelCollider wheel, Transform transform)  // Take the world position and rotation of the WheelCollider and pass them on transform component of our Wheel Mesh
    {
        Vector3 position;
        Quaternion rotation;
        wheel.GetWorldPose(out position, out rotation);

        if (car.number == 0) // something wrong with my first car, that's why
        {
            Quaternion correctedRotation = rotation * Quaternion.Euler(0f, 90f, 0f);
            Quaternion lerpedRotation = Quaternion.Lerp(transform.rotation, correctedRotation, wheelRotationTime);
            transform.position = position;
            transform.rotation = lerpedRotation;
        }
        else
        {
            Quaternion correctedRotation = rotation;
            Quaternion lerpedRotation = Quaternion.Lerp(transform.rotation, correctedRotation, wheelRotationTime);
            transform.position = position;
            transform.rotation = lerpedRotation;
        }
    }
}