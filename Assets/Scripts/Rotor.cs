using UnityEngine;
using System.Collections;

public class Rotor : MonoBehaviour {

    public const int XCWRotor1 = 0;
    public const int XCWRotor2 = 1;
    public const int ZCCWRotor1 = 2;
    public const int ZCCWRotor2 = 3;

    public int rotorId;
    public float maxThrottle;
    public float throttle;

    Vector3 force;
    Vector3 torque;
    Rigidbody rb;

    public void setThrottle(float f){
        throttle = f;
    }

    public float getThrottle(){
        return throttle;
    }

    public void IncreaseThrottle (){
        if (throttle < 100) {
            throttle=throttle+0.1f;
        }
    }
    
    public void DecreaseThrottle (){
        if (throttle > 0) {
            throttle=throttle-0.1f;
        }  
    }

    public void CutEngine(){
        throttle = 0;
    }

    void Start(){
        rb = gameObject.transform.root.GetComponent<Rigidbody>();
        maxThrottle = 100;
    }

    void FixedUpdate () {
        force = transform.up * (maxThrottle * (throttle/100));
        rb.AddForce(force, ForceMode.Force);
        //Debug.Log("Force: " + force + " aka: " + gameObject.name);
        //Debug.Debug.DrawRay (transform.position, force, Color.red);

        switch(rotorId){
            case XCWRotor1:
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
        //Debug.Debug.DrawRay (transform.position, torque, Color.red);
    }
}
