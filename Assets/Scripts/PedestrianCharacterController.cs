using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PedestrianCharacterController : MonoBehaviour
{
    public Vector3 destination;
    Vector3 lastPosition;
    public bool reachedDestination;
    public float stopDistance = 1;
    public float rotationSpeed;
    public float minSpeed, maxSpeed;
    public float movementSpeed;
    Vector3 velocity;
    private Animator animator;
    public float frontSensorAngle = 30;
    private bool light = true;

    [Header("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition;
    public float frontSideSensorPosition = 0f;
    private void Start()
    {
        movementSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        animator = this.GetComponent<Animator>();
        if (animator == null)
            throw new InvalidOperationException();
    }
    private void Update()
    {
        Sensors();
        if (transform.position != destination)
        {
            if (light)
            {
                Vector3 destinationDirection = destination - transform.position;
                destinationDirection.y = 0;

                float destinationDistance = destinationDirection.magnitude;

                if (destinationDistance >= stopDistance)
                {
                    reachedDestination = false;
                    Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                }
                else
                {
                    reachedDestination = true;
                }
                animator.SetBool("isMooving", true);
                velocity = (transform.position - lastPosition) / Time.deltaTime;
                velocity.y = 0;
                var velocityMagnitude = velocity.magnitude;
                velocity = velocity.normalized;
                var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
                var rightDotProduct = Vector3.Dot(transform.right, velocity);
            }
            else
                animator.SetBool("isMooving", false);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        //sensorLength = curentSpeed;
        Physics2D.queriesStartInColliders = false;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, 2))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            if (hit.collider.CompareTag("Traffic lights"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                light = hit.transform.GetComponent<TrafficLights>().light;
            }
        }
    }
}