using UnityEngine;
using System.Collections;

public class FlightController : MonoBehaviour {

    public Vector3 setPoint;
    public float pGain;
    public float iGain;
    public float dGain;

    public GameObject zCCWRotor1;
    public GameObject zCCWRotor2;
    public GameObject xCWRotor1;
    public GameObject xCWRotor2;

    float forceHeight;
    Vector3 force;
    Vector3 curPos;

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

        heightController = new PIDController(setPoint.y,pGain,iGain,dGain,forceHeight,transform.position.y);
        heightController.updateOutputSignal(transform.position.y);

        attitudeControllerX = new PIDController(0f,pGain,iGain,dGain,forceHeight,transform.position.x);
        attitudeControllerX.updateOutputSignal(transform.position.x);
        attitudeControllerY = new PIDController(0f,pGain,iGain,dGain,forceHeight,transform.position.y);
        attitudeControllerY.updateOutputSignal(transform.position.y);
        attitudeControllerZ = new PIDController(0f,pGain,iGain,dGain,forceHeight,transform.position.z);
        attitudeControllerZ.updateOutputSignal(transform.position.z);
        /*
        attitudeController = new PIDController(setPoint,pGain,iGain,dGain,force,transform.position);
        attitudeController.updateOutputSignal(transform.position);
        */
    }
    
    // Update is called once per frame
    void Update () {
        HeightStabiliser();
        AttitudeStabiliser();
    }

    private void AttitudeStabiliser(){
        curPos = transform.position;
        Vector3 f = Vector3.zero;
        f.x = attitudeControllerX.updateOutputSignal(transform.position.x);
        f.y = attitudeControllerY.updateOutputSignal(transform.position.y);
        f.z = attitudeControllerZ.updateOutputSignal(transform.position.z);
        Debug.Log("att f.x: " + f.x);
        Debug.Log("att f.y: " + f.y);
        Debug.Log("att f.z: " + f.z);
        /*
        Vector3 f = Vector3.zero;
        f = attitudeController.updateOutputSignal(transform.position);
        */
        /*
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor1.GetComponent<Rotor>().getThrottle() + f.x, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor2.GetComponent<Rotor>().getThrottle() - f.x, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor1.GetComponent<Rotor>().getThrottle() + f.z, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor2.GetComponent<Rotor>().getThrottle() - f.z, throttleMin, throttleMax));
        */
        zCCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor1.GetComponent<Rotor>().getThrottle() + f.y, throttleMin, throttleMax));
        zCCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(zCCWRotor2.GetComponent<Rotor>().getThrottle() + f.y, throttleMin, throttleMax));
        xCWRotor1.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor1.GetComponent<Rotor>().getThrottle() + f.y, throttleMin, throttleMax));
        xCWRotor2.GetComponent<Rotor>().setThrottle(Mathf.Clamp(xCWRotor2.GetComponent<Rotor>().getThrottle() + f.y, throttleMin, throttleMax));
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
        throttleMin = Mathf.Clamp(y,1f,5f);
        throttleMax = throttleMin + 5;
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
