using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UpdatePointAt(Vector3 target)
    {
        
        transform.LookAt(CustomMath.Vector3NotNew(target.x, transform.position.y, target.z));
    }
}
