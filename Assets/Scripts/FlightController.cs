using UnityEngine;
using System.Collections;
using System;

public class FlightController : MonoBehaviour {

    public Vector3 setPoint;
    Quaternion setRotation;
    public float pGainA;
    public float iGainA;
    public float dGainA;
    public float pGainH;
    public float iGainH;
    public float dGainH;

    public GameObject zCCWRotor1;
    public GameObject zCCWRotor2;
    public GameObject xCWRotor1;
    public GameObject xCWRotor2;

    float forceHeight;
    Vector3 force;
    Vector3 curPos;
    Quaternion curRot;

    PIDController heightController;
    PIDController attitudeControllerX;
    PIDController attitudeControllerY;
    PIDController attitudeControllerZ;

    float throttleMin=0;
    float throttleMax=100;

    /*
    PIDController attitudeController;
    */

    // Use this for initialization
    void Start () {
        /* For those with time constraints: */
        Time.timeScale=1f;
        transform.position = new Vector3(0f,0.5f,0f);
        forceHeight = 9.81f/4f;
        setRotation = new Quaternion (0, 0, 0, 1);

        heightController = new PIDController(setPoint.y,pGainH,iGainH,dGainH,forceHeight,transform.position.y);
        heightController.updateOutputSignal(transform.position.y);

/*
        attitudeControllerX = new PIDController(0f,pGainA,iGainA,dGainA,0f,curRot.eulerAngles.x);
        attitudeControllerX.updateOutputSignal(curRot.eulerAngles.x);
        attitudeControllerY = new PIDController(0f,pGainA,iGainA,dGainA,0f,curRot.eulerAngles.y);
        attitudeControllerY.updateOutputSignal(curRot.eulerAngles.y);
        attitudeControllerZ = new PIDController(0f,pGainA,iGainA,dGainA,0f,curRot.eulerAngles.z);
        attitudeControllerZ.updateOutputSignal(curRot.eulerAngles.z);
*/
        /*
        attitudeControllerX = new PIDController(0f,pGain,iGain,dGain,forceHeight,transform.position.x);
        attitudeControllerX.updateOutputSignal(transform.position.x);
        attitudeControllerY = new PIDController(0f,pGain,iGain,dGain,forceHeight,transform.position.y);
        attitudeControllerY.updateOutputSignal(transform.position.y);
        attitudeControllerZ = new PIDController(0f,pGain,iGain,dGain,forceHeight,transform.position.z);
        attitudeControllerZ.updateOutputSignal(transform.position.z);
        */
        /*
        attitudeController = new PIDController(setPoint,pGain,iGain,dGain,force,transform.position);
        attitudeController.updateOutputSignal(transform.position);
        */
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        HeightStabiliser();
        AttitudeStabiliser();
    }

    Quaternion errorRot, lastError;
    float integralX=0,integralY=0,integralZ=0;
    private void AttitudeStabiliser(){
        // I calculate the PID values without using PIDController due to it being quaternions.
        // Thanks to: https://github.com/richardhannah/honours-project/blob/master/AttitudeControl.cs
        //
        // PROVISIONAL::::: if this doesn't work use tMin=tMax. And something else??
        curRot = gameObject.GetComponent<RotationSensor>().getRotation();
        errorRot = Quaternion.Inverse (curRot) * setRotation;
        
        integralX += errorRot.x * Time.deltaTime;
        integralY += errorRot.y * Time.deltaTime;
        integralZ += errorRot.z * Time.deltaTime;
        
        float derivX = (errorRot.x - lastError.x);
        float derivY = (errorRot.y - lastError.y);
        float derivZ = (errorRot.z - lastError.z);
        
        lastError = errorRot;
        float pitchCorrectionX = (float)Math.Round((double)errorRot.x * pGainA + integralX * iGainA + derivX * dGainA,2);
        float pitchCorrectionY = (float)Math.Round((double)errorRot.y * pGainA + integralY * iGainA + derivY * dGainA,2);
        float pitchCorrectionZ = (float)Math.Round((double)errorRot.z * pGainA + integralZ * iGainA + derivZ * dGainA,2);
        Debug.Log("pitchCorrectionX: " + pitchCorrectionX);        
        Debug.Log("pitchCorrectionY: " + pitchCorrectionY);        
        Debug.Log("pitchCorrectionZ: " + pitchCorrectionZ);        
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor1.GetComponent<Rotor>().getThrottle() + pitchCorrectionZ, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor2.GetComponent<Rotor>().getThrottle() - pitchCorrectionZ, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor1.GetComponent<Rotor>().getThrottle() + pitchCorrectionX, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor2.GetComponent<Rotor>().getThrottle() - pitchCorrectionX, throttleMin, throttleMax));
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor1.GetComponent<Rotor>().getThrottle() + pitchCorrectionY, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor2.GetComponent<Rotor>().getThrottle() + pitchCorrectionY, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor1.GetComponent<Rotor>().getThrottle() - pitchCorrectionY, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor2.GetComponent<Rotor>().getThrottle() - pitchCorrectionY, throttleMin, throttleMax));
        /*
        Debug.Log("rot x: " + curRot.eulerAngles.x);
        Debug.Log("rot y: " + curRot.eulerAngles.y);
        Debug.Log("rot z: " + curRot.eulerAngles.z);
        */
/*
        Vector3 f = Vector3.zero;
        f.x = attitudeControllerX.updateOutputSignal(curRot.eulerAngles.x);
        f.y = attitudeControllerY.updateOutputSignal(curRot.eulerAngles.y);
        f.z = attitudeControllerZ.updateOutputSignal(curRot.eulerAngles.z);
*/
/*
        Debug.Log("att f.x: " + f.x);
        Debug.Log("att f.y: " + f.y);
        Debug.Log("att f.z: " + f.z);
*/
        /*
        Vector3 f = Vector3.zero;
        f = attitudeController.updateOutputSignal(transform.position);
        */
/*
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor1.GetComponent<Rotor>().getThrottle() + f.x, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor2.GetComponent<Rotor>().getThrottle() - f.x, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor1.GetComponent<Rotor>().getThrottle() + f.z, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor2.GetComponent<Rotor>().getThrottle() - f.z, throttleMin, throttleMax));
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor1.GetComponent<Rotor>().getThrottle() + f.y, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor2.GetComponent<Rotor>().getThrottle() + f.y, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor1.GetComponent<Rotor>().getThrottle() - f.y, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor2.GetComponent<Rotor>().getThrottle() - f.y, throttleMin, throttleMax));
*/
        /*
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(f.x, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(-f.x, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(f.z, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(-f.z, throttleMin, throttleMax));
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(f.y, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(f.y, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(f.y, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(f.y, throttleMin, throttleMax));
        */

        /*
        if(f.y < 0){
            zCCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            zCCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        if(f.y < 0){
            zCCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            zCCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        if(f.y < 0){
            xCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            xCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        if(f.y < 0){
            xCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            xCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        */
    }
    

    private void HeightStabiliser() {
        curPos = transform.position;
        float y;
        y = heightController.updateOutputSignal(curPos.y);
        throttleMin = Mathf.Clamp(y,0f,6f);
        throttleMax = throttleMin + 1f;
        /*
        Debug.Log("y: " + y);
        Debug.Log("thMin: " + throttleMin);
        Debug.Log("thMax: " + throttleMax);
        */
        /*
        curPos = transform.position;
        Vector3 f = Vector3.zero;
        f.y = heightController.updateOutputSignal(curPos.y);
        if(f.y < 0){
            zCCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            zCCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        if(f.y < 0){
            zCCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            zCCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        if(f.y < 0){
            xCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            xCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        if(f.y < 0){
            xCWRotor2.GetComponent<Rotor>().setThrottle(f.y);
        }
        else{
            xCWRotor1.GetComponent<Rotor>().setThrottle(f.y);
        }
        */
    }
}
