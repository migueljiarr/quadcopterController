using UnityEngine;
using System.Collections;

public class Rotor : MonoBehaviour {

    public const int XCWRotor1 = 0;
    public const int XCWRotor2 = 1;
    public const int ZCCWRotor1 = 2;
    public const int ZCCWRotor2 = 3;
    public int rotorPos;

    public float maxThrottle;
    public float throttle;

    Vector3 force;
    Vector3 torque;
    Rigidbody rb;

    public void setThrottle(float f){
        throttle = f;
    }

    void Start(){
        rb = gameObject.transform.root.GetComponent<Rigidbody>();
        maxThrottle = 100;
    }

    // Update is called once per frame
    void FixedUpdate () {
        force = transform.up * (maxThrottle * (throttle/100));
        Debug.Log("Force: " + force + " aka: " + gameObject.name);
        //rb.AddForceAtPosition(force, curPos, ForceMode.Force); 
        rb.AddForce(force, ForceMode.Force);
        switch(rotorPos){
            case XCWRotor1:
        maxThrottle = 110;
                torque = transform.forward * -1 * (maxThrottle * (throttle/100));
                break;
            case XCWRotor2:
                torque = transform.forward * (maxThrottle * (throttle/100));
                break;
            case ZCCWRotor1:
                torque = transform.right * (maxThrottle * (throttle/100));
                break;
            case ZCCWRotor2:
                torque = transform.right * -1 * (maxThrottle * (throttle/100));
                break;
        }
        rb.AddForceAtPosition(torque, transform.position, ForceMode.Force);
        Debug.DrawRay(transform.position, torque, Color.red); 
    }
}
