using UnityEngine;
using System.Collections;

public class EvolutionaryProgramming: MonoBehaviour {

    public bool getOscillation(Vector3 prevPos, Vector3 curPos, Vector3 targetPos){
        if(prevPos.magnitude < targetPos.magnitude && curPos.magnitude > targetPos.magnitude)
            return true;
        else if(prevPos.magnitude > targetPos.magnitude && curPos.magnitude < targetPos.magnitude)
            return true;
        else return false;
    }

}
