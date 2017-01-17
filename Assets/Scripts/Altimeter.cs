using UnityEngine;
using System.Collections;

public class Altimeter : MonoBehaviour {
    
    public float altitude;
    private RaycastHit hit;

    void Start () {
        /*altitude = 10F;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit)){
            
            altitude = hit.distance;
            Debug.Log(altitude);
        }
        */

    }
    
    void Update () {
        if (Physics.Raycast(transform.position, -Vector3.up, out hit)){
            altitude = hit.distance;
            //Debug.Log(altitude);
            //Debug.DrawLine(transform.position, -Vector3.up, Color.red);
        }
    }

    float getHeight(){
        // Not used at the moment because the floor is not big enough.
        return altitude;
    }
}
