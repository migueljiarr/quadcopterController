using UnityEngine;
using System.Collections;

public class RotationSensor : MonoBehaviour {
    
    Quaternion currentRotation;

    void Start () {
        currentRotation = this.transform.parent.GetComponent<Rigidbody>().rotation;
    }
    
    void Update () {
        currentRotation = this.transform.parent.GetComponent<Rigidbody>().rotation;
    }

    public Quaternion getRotation(){
        return currentRotation;
    }
}
