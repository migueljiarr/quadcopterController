using UnityEngine;
using System.Collections;

public class RotationSensor : MonoBehaviour {
    
    Quaternion currentRotation;

    void Start () {
        currentRotation = this.transform.GetComponent<Rigidbody>().rotation;
    }
    
    void Update () {
        currentRotation = this.transform.GetComponent<Rigidbody>().rotation;
    }

    public Quaternion getRotation(){
        return currentRotation;
    }
}
