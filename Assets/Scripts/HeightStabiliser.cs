using UnityEngine;
using System.Collections;

public class HeightStabiliser : MonoBehaviour {

    public Vector3 setPoint;
    public float pGain;
    public float iGain;
    public float dGain;

    public GameObject zCWRotor;
    public GameObject zCCWRotor;
    public GameObject xCWRotor;
    public GameObject xCCWRotor;

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
            zCCWRotor.GetComponent<Rotor>().setForce(f);
        }
        else{
            zCWRotor.GetComponent<Rotor>().setForce(f);
        }
        if(f.y < 0){
            zCWRotor.GetComponent<Rotor>().setForce(f);
        }
        else{
            zCCWRotor.GetComponent<Rotor>().setForce(f);
        }
        if(f.y < 0){
            xCCWRotor.GetComponent<Rotor>().setForce(f);
        }
        else{
            xCWRotor.GetComponent<Rotor>().setForce(f);
        }
        if(f.y < 0){
            xCWRotor.GetComponent<Rotor>().setForce(f);
        }
        else{
            xCCWRotor.GetComponent<Rotor>().setForce(f);
        }
    }
}
