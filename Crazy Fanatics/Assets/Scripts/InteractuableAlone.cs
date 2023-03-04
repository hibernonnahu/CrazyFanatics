using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableAlone : MonoBehaviour {

    // Use this for initialization
    public float time = 2;
    public float speed = 1.5f;
    private float counter;
	void Start () {
		
	}
	public void Activate(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        counter = time;
    }
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.up * Time.deltaTime * speed;
        counter -= Time.deltaTime;
        if(counter<0)
        {
            gameObject.SetActive(false);
        }
    }
}
