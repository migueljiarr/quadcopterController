using UnityEngine;
using System.Collections;

public class HeightStabiliser : MonoBehaviour {

    public Vector3 setPoint;
    public float pGain;
    public float iGain;
    public float dGain;

    public GameObject zCCWRotor1;
    public GameObject zCCWRotor2;
    public GameObject xCWRotor1;
    public GameObject xCWRotor2;

    Vector3 force;
    Vector3 curPos;

    PIDController controller;

    // Use this for initialization
    void Start () {
        /* For those with time constraints: */
        Time.timeScale=1f;
        transform.position = new Vector3(0f,0.5f,0f);
        force = new Vector3(0f,9.81f/4,0f);

        controller = new PIDController(setPoint,pGain,iGain,dGain,force,transform.position);
        controller.updateOutputSignal(transform.position);
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        curPos = transform.position;
        Vector3 f = controller.updateOutputSignal(curPos);
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
    }
}
