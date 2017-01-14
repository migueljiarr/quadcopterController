﻿using UnityEngine;
using System.Collections;

public class PIDController{
    float controllerPGain; // 20 or 100
    float controllerIGain;
    float controllerDGain; // 30 or 150

    float controllerBias;
    float setPoint;
    float processVar;
    float prevPV;
    float errorAcumulator;
    float outC;
    float outP;
    float outI;
    float outD;

    public PIDController(float sp, float kp, float ki, float kd, float cbias, float ppv){
        setPoint = sp;
        controllerPGain = kp;
        controllerIGain = ki;
        controllerDGain = kd;
        controllerBias = cbias;
        prevPV = ppv;
    }

    float getError(float sp, float pv){
        return sp-pv;
    }
    
    float getDifferencePV(float prev, float pv){
        return prev-pv;
    }
    
    float getErrAcum(float err){
        errorAcumulator=errorAcumulator+err*Time.deltaTime;
        return errorAcumulator;
    }

    public float updateOutputSignal(float pv){
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
