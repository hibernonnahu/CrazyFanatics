using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour {
    private MainCharacter mainCharacter;
    public float moveAt = 0.3f;
    public float moveSpeed = 3f;
    public float movingOffset = 5;
    public float xOffset = 0f;
    public float yOffset = 0;
    public float zOffset = 0;
    public float[] camerabounds;
    private float initialY;
    Action onCamera;
    Action onOffsetVariation=()=> { };
    public float offsetSpeed = 1;
    
	void Start () {
        initialY = Camera.main.transform.position.y;
        moveAt = 0.5f - moveAt;
        onCamera = OnFollowPlayerCamera;
        mainCharacter = FindObjectOfType<MainCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
        onCamera();
        onOffsetVariation();
    }
    void OnFollowPlayerCamera()
    {
        var xx = mainCharacter.transform.position.x;
        var zz = mainCharacter.transform.position.z;
        if (xx < camerabounds[0]+movingOffset)
        {
            xx = camerabounds[0] + movingOffset;
        }
        else if (xx > camerabounds[1] - movingOffset)
        {
            xx = camerabounds[1] - movingOffset;
        }
        if (zz < camerabounds[2] + movingOffset)
        {
            zz = camerabounds[2] + movingOffset;
        }
        else if (zz > camerabounds[3] - movingOffset)
        {
            zz = camerabounds[3] - movingOffset;
        }
        //Debug.Log(zz + " 2:" + camerabounds[2] + " 3:" + camerabounds[3]);
        Camera.main.transform.position = Vector3.right*(xx+xOffset)+Vector3.up* (initialY+yOffset)+ Vector3.forward *(zz+zOffset);
    }

    internal void GoToOffset(float xx, float yy,float zz)
    {
       
        Vector2 destiny = Vector2.right * xx + Vector2.up * yy;

        onOffsetVariation = () => {
            Vector3 pDpC = (Vector3.right * xx + Vector3.up * yy+Vector3.forward*zz) - (Vector3.right * xOffset + Vector3.up * yOffset+Vector3.forward*zOffset);
            Vector3 aux= CustomMath.Normalize(pDpC) * Time.deltaTime * offsetSpeed;
            xOffset += aux.x;
            yOffset += aux.y;
            zOffset += aux.z;
            if (pDpC.sqrMagnitude < 0.5f) {
                onOffsetVariation = () => { };
            }
        };
    }

    void OnMouseCamera()
    {
        var xx = Mathf.Clamp01( Input.mousePosition.x / Screen.width)-0.5f;
        var yy = Mathf.Clamp01(Input.mousePosition.y / Screen.height)-0.5f;
       
        if(xx<0)
        {
            xx += moveAt;
           
            if (xx<0)
            {
                xx = Time.deltaTime * moveSpeed * xx;
                if(xx+Camera.main.transform.position.x<camerabounds[0])
                {
                    xx = 0;
                }
            }
            else
            {
                xx = 0;
            }
            
        }
        else
        {
            xx -= moveAt;
            
            if (xx > 0)
            {
                xx = Time.deltaTime * moveSpeed * xx;
                if (xx + Camera.main.transform.position.x > camerabounds[1])
                {
                    xx = 0;
                }
            }
            else
            {
                xx = 0;
            }
        }
        Camera.main.transform.position += Vector3.right * xx;

        if (yy < 0)
        {
            yy += moveAt;

            if (yy < 0)
            {
                yy = Time.deltaTime * moveSpeed * yy;
                if (yy + Camera.main.transform.position.z < camerabounds[3])
                {
                    yy = 0;
                }
            }
            else
            {
                yy = 0;
            }

        }
        else
        {
            yy -= moveAt;

            if (yy > 0)
            {
                yy = Time.deltaTime * moveSpeed * yy;
                if (yy + Camera.main.transform.position.z> camerabounds[2])
                {
                    yy = 0;
                }
            }
            else
            {
                yy = 0;
            }
        }
        Camera.main.transform.position += Vector3.forward * yy;

        //Debug.Log(camerabounds[0] + " " + camerabounds[1] + " " + camerabounds[2] + " " + camerabounds[3] );
    }
}
