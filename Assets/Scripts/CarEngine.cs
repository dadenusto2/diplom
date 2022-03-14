using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public Transform path;
    public float maxStreetAngle = 45f;
    private List<Transform> nodes;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    private int curentNode = 0;
    public float maxMotorTorque = 80f;
    public float maxBreakTorque = 150f;
    public float curentSpeed;
    public float maxSpeed = 100f;
    public Vector3 centerOfMass;
    public bool isBreaking  = false;
    public bool isStoping  = false;
    [Header("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition;
    public float frontSideSensorPosition = 0f;
    public float frontSensorAngle = 30;
    private bool avoiding = false;
    private bool avoidingLeft = false;
    private bool avoidingRight = false;
    private bool turn = false;
    private bool noTurn = false;
    public bool carNext = false;
    public float drive = 0f;
    public float breakk = 0f;
    public bool noPath = true;
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        Transform[] pathTransform = path.GetComponentsInChildren<Transform>(); 
        nodes = new List<Transform>();
        for(int i = 0; i< pathTransform.Length;i++){
            if(pathTransform[i] != path.transform){
                nodes.Add(pathTransform[i]);
            }
        } 
    }

    public float GetCurSpeed(){
        return curentSpeed;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        Sensors();
        CheckWaypointDistance();
        ApplySteer();
        Drive();
        Breaking();
        Stoping();
        CarNext();
    }
    private void ApplySteer(){
        if(avoiding) return;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[curentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude)*maxStreetAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }
    private void Drive(){
        drive = wheelFL.motorTorque;
        breakk = wheelFL.brakeTorque;
        curentSpeed = Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        wheelFL.motorTorque = maxMotorTorque;
        wheelFR.motorTorque = maxMotorTorque;
        if(curentSpeed < maxSpeed){
            wheelFL.brakeTorque = 0f;
            wheelFR.brakeTorque = 0f;
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;  
        }else{
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0; 
        }
    }
    private void CheckWaypointDistance(){
        if(Vector3.Distance(transform.position, nodes[curentNode].position) < 0.5f){
            if(curentNode != nodes.Count - 1){
                curentNode++;
            }
        }
    }
    private void Breaking(){
        if(isBreaking){
            wheelFL.brakeTorque = maxBreakTorque;
            wheelFR.brakeTorque = maxBreakTorque;
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0; 
        }
    }   
    private void CarNext(){
        if(carNext){
            wheelFL.brakeTorque = maxBreakTorque / 4;
            wheelFR.brakeTorque = maxBreakTorque / 4;
            wheelFL.motorTorque = maxMotorTorque / 2;
            wheelFR.motorTorque = maxMotorTorque / 2; 
        }
    }
    private void Sensors(){
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        float avoidMultiplayer = 0;
        avoiding = false;
        turn = false;
        noTurn = false;
        isBreaking = false;
        isStoping = false;
        carNext = false;
        //sensorLength = curentSpeed;
        if(sensorLength<10){
            sensorLength = 10;
        }
        Physics2D.queriesStartInColliders = false;
        if(Physics.Raycast(sensorStartPos, transform.forward, out hit, 10)){ 
            Debug.DrawLine(sensorStartPos, hit.point);
            if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
            }
        }
        if(Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){ 
            if(hit.collider.CompareTag("turn")){
                turn=true;
                Debug.DrawLine(sensorStartPos, hit.point);               
                if(noPath){
                    path = hit.collider.GetComponent<PathSpawner>().getPath();
                    noPath = false;
                    curentNode = 0;
                    Transform[] pathTransform = path.GetComponentsInChildren<Transform>(); 
                    nodes = new List<Transform>();
                    for(int i = 0; i< pathTransform.Length;i++){
                        if(pathTransform[i] != path.transform){
                            nodes.Add(pathTransform[i]);
                        }
                    }       
                    Debug.Log(nodes.Count);
                }
            }
            if(hit.collider.CompareTag("No turn")){
                Debug.DrawLine(sensorStartPos, hit.point);               
                noPath = true;
            }
            if(hit.collider.CompareTag("Crash")){
                isBreaking=true;
                Debug.DrawLine(sensorStartPos, hit.point);
            }
        }
        float k = 0f;
        for (int i = 0, j = 15; i< 15; i++, j--, k+=0.2f){
            Vector3 rightSensorPosition = sensorStartPos + k * transform.right * frontSideSensorPosition;
            if(Physics.Raycast(rightSensorPosition, transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(rightSensorPosition, hit.point);
                    avoiding = true;
                    avoidMultiplayer -= 1/j;
                }

            }
            if(Physics.Raycast(rightSensorPosition, transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(rightSensorPosition, hit.point);
                // }
            }
            if(Physics.Raycast(rightSensorPosition, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(rightSensorPosition, hit.point);
                    avoidingRight = true;
                    avoidMultiplayer -= 1/(2*j);
                }

            }
            if(Physics.Raycast(rightSensorPosition, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(rightSensorPosition, hit.point);
                // }
            }
            if(Physics.Raycast(rightSensorPosition, Quaternion.AngleAxis(frontSensorAngle/2, transform.up) * transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(rightSensorPosition, hit.point);
                    avoidingRight = true;
                    avoidMultiplayer -= 1/(2*j);
                }

            }
            if(Physics.Raycast(rightSensorPosition, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(rightSensorPosition, hit.point);
                // }
            }
            if(Physics.Raycast(rightSensorPosition, Quaternion.AngleAxis(-frontSensorAngle/2, transform.up) * transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(rightSensorPosition, hit.point);
                    avoidingRight = true;
                    avoidMultiplayer -= 1/(2*j);
                }

            }
            if(Physics.Raycast(rightSensorPosition, Quaternion.AngleAxis(-frontSensorAngle/2, transform.up) * transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(rightSensorPosition, hit.point);
                // }
            }
            Vector3 leftSensorPosition = sensorStartPos - k * transform.right * frontSideSensorPosition;
            if(Physics.Raycast(leftSensorPosition, transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(leftSensorPosition, hit.point);
                    avoiding = true;
                    avoidMultiplayer += 1/j;
                }

            }
            if(Physics.Raycast(leftSensorPosition, transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(leftSensorPosition, hit.point);
                // }
            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(leftSensorPosition, hit.point);
                    avoidingLeft = true;
                    avoidMultiplayer += 1/(2*j);
                }

            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(leftSensorPosition, hit.point);
                // }
            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(-frontSensorAngle/2, transform.up) * transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(leftSensorPosition, hit.point);
                    avoidingLeft = true;
                    avoidMultiplayer += 1/(2*j);
                }

            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(-frontSensorAngle/2, transform.up) * transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(leftSensorPosition, hit.point);
                // }
            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(leftSensorPosition, hit.point);
                    avoidingLeft = true;
                    avoidMultiplayer += 1/(2*j);
                }

            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(leftSensorPosition, hit.point);
                // }
            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(frontSensorAngle/2, transform.up) * transform.forward, out hit, 10)){
                if(hit.collider.CompareTag("Work")||hit.collider.CompareTag("Car")){
                    Debug.DrawLine(leftSensorPosition, hit.point);
                    avoidingLeft = true;
                    avoidMultiplayer += 1/(2*j);
                }

            }
            if(Physics.Raycast(leftSensorPosition, Quaternion.AngleAxis(frontSensorAngle/2, transform.up) * transform.forward, out hit, sensorLength)){
                // if(hit.collider.CompareTag("No turn")){
                //     noTurn=true;
                //     Debug.DrawLine(leftSensorPosition, hit.point);
                // }
            }
        }
        if (avoiding){
            if(curentSpeed>15)
                isStoping = true;
            if(avoidMultiplayer!=0){
                wheelFL.steerAngle = maxStreetAngle * avoidMultiplayer;
                wheelFR.steerAngle = maxStreetAngle * avoidMultiplayer;
            }
            else{
                wheelFL.steerAngle = -maxStreetAngle;
                wheelFR.steerAngle = -maxStreetAngle;
            }
        }
        if(turn){
            isStoping = true;
        }
        else if(noTurn){
            isStoping = true;
        }
    }
    private void Stoping(){
        if(isStoping){
            wheelFL.brakeTorque = maxBreakTorque;
            wheelFR.brakeTorque = maxBreakTorque;       
            wheelFL.motorTorque = maxMotorTorque*1.02f;
            wheelFR.motorTorque = maxMotorTorque*1.02f;
        }
    }
}
