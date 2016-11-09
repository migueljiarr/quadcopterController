﻿using UnityEngine;
using System.Collections;

public class HeightStabiliser : MonoBehaviour {

    public Vector3 applyPositionZ;
    public Vector3 applyPositionX;
    public Vector3 setPoint;
    public float pGain;
    public float iGain;
    public float dGain;
    Vector3 force;
    Vector3 curPos;
    Rigidbody rb;

    PIDController controller;

    class PIDController{
        Vector3 controllerBias;
        float controllerPGain; // 20 or 100
        float controllerIGain;
        float controllerDGain; // 30 or 150
        Vector3 setPoint;
        Vector3 processVar;
        Vector3 prevPV;
        Vector3 errorAcumulator;
        Vector3 outC;
        Vector3 outP;
        Vector3 outI;
        Vector3 outD;

        public PIDController(Vector3 sp, float kp, float ki, float kd, Vector3 cbias, Vector3 ppv){
            setPoint = sp;
            controllerPGain = kp;
            controllerIGain = ki;
            controllerDGain = kd;
            controllerBias = cbias;
            prevPV = ppv;
        }

        Vector3 getError(Vector3 sp, Vector3 pv){
            return sp-pv;
        }

        Vector3 getDifferencePV(Vector3 prev, Vector3 pv){
            return prev-pv;
        }

        Vector3 getErrAcum(Vector3 err){
            errorAcumulator=errorAcumulator+err;
            return errorAcumulator;
        }

        public Vector3 updateOutputSignal(Vector3 pv){
            //Debug.Log("pRrror: " + getError(setPoint,pv));
            //Debug.Log("difference: " + getDifferencePV(prevPV,pv));
            //Debug.Log("errAcum: " + getErrAcum(getError(prevPV,pv)));
            outP = controllerPGain*getError(setPoint,pv) + controllerBias;
            outI = controllerIGain*getErrAcum(getError(setPoint,pv));
            outD = controllerDGain*getDifferencePV(prevPV,pv);
            outC = outP + outI + outD;
            prevPV = pv;
            return outC;
        }

    }

    // Use this for initialization
    void Start () {
        /* For those with time constraints: */
        //Time.timeScale=5f;
        rb = gameObject.transform.root.gameObject.GetComponent<Rigidbody>();
        transform.position = new Vector3(0f,0.5f,0f);
        force = new Vector3(0f,9.81f/4,0f);

        controller = new PIDController(setPoint,pGain,iGain,dGain,force,transform.position);
        //controller = new PIDController(setPoint,gain,new Vector3(0f,0f,0f));
        controller.updateOutputSignal(transform.position);
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        curPos = transform.position;
        Vector3 f = controller.updateOutputSignal(curPos);
        Debug.Log("F: " + f);
        rb.AddForceAtPosition(f, curPos+applyPositionZ, ForceMode.Force); 
        rb.AddForceAtPosition(f, curPos-applyPositionZ, ForceMode.Force); 
        rb.AddForceAtPosition(f, curPos+applyPositionX, ForceMode.Force); 
        rb.AddForceAtPosition(f, curPos-applyPositionX, ForceMode.Force); 
    }
}
