using UnityEngine;
using System.Collections;

public class Rotor : MonoBehaviour {

    public Vector3 force;
    Vector3 curPos;
    Rigidbody rb;

    public void setForce(Vector3 f){
        force = f;
    }

    void Start(){
        rb = gameObject.transform.root.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        curPos = transform.position;
        Debug.Log("Force: " + force + " at position: " + curPos + " aka: " + gameObject.name);
        rb.AddForceAtPosition(force, curPos, ForceMode.Force); 
    }
}
