using UnityEngine;
using System.Collections;

public class PIDController{
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
        errorAcumulator=errorAcumulator+err*Time.deltaTime;
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
