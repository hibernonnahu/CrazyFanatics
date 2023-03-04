using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionFeedback : PooleableObject {
    public bool debug = false;
   
    private GameObject canvasObjectToGo;
    
    public string canvasTagName="";
    public float tagSpeed = 0.007f;
    public float tagSpeedOffset = 0.7f;
    public float zOffset = 4.5f;
    private float zMultiplier = 0.05f;
    private Action onUpdate;
    private Vector3 destiny;
    private RectTransform iconRectTransform;
    public float iconEnlargementSpeed = 1;
    private Action finalAction;
    private void Start()
    {
        
        if (canvasTagName != "")
        {
            if (canvasObjectToGo == null)
            {
                zAux = 1;
                
                canvasObjectToGo = GameObject.FindGameObjectWithTag(canvasTagName);
                destiny = ((canvasObjectToGo.transform.position.x / Screen.width) * Vector3.right + (canvasObjectToGo.transform.position.y / Screen.height) * Vector3.up)  ;
            }
           
        }
        else
        {
            GoNormal();
        }
        if(canvasObjectToGo!=null)
        {
            onUpdate = GoCanvas;
        }
        else
        {
            GoNormal();
        }
    }
    private void OnEnable()
    {
        variableSpeed = 0;
        currentXSpeed = UnityEngine.Random.Range(-randomX, randomX);
    }
    private void Update()
    {
        if (debug)
        {

        }
        onUpdate();
    }
    public void SetObjectToGo(GameObject go,Action finalAction)
    {
        if (go != null)
        {
            canvasObjectToGo = go;
        }
        
        this.finalAction = finalAction;
        zAux = 1;
    }
    private void GoCanvas()
    {
        UpdateDestiny();
        Vector3 aux = (destiny - Camera.main.WorldToViewportPoint(transform.position));
        
        transform.rotation = Camera.main.transform.rotation;
        if(aux.sqrMagnitude<0.001f)
        {
            if (finalAction != null)
            {
                finalAction();
            }
            transform.position = Vector3.up*-10000000;
            iconRectTransform = canvasObjectToGo.GetComponent<RectTransform>();
            onUpdate = () => {
                iconRectTransform.localScale += iconEnlargementSpeed * Time.deltaTime*Vector3.up+ iconEnlargementSpeed * Time.deltaTime * Vector3.right;
                if (iconRectTransform.localScale.y > 1.5f)
                {
                    onUpdate = () =>
                    {
                        iconRectTransform.localScale -= iconEnlargementSpeed * Time.deltaTime * Vector3.up + iconEnlargementSpeed * Time.deltaTime * Vector3.right;
                        if (iconRectTransform.localScale.y < 1f)
                        {
                            iconRectTransform.localScale = Vector3.one;
                            onUpdate = GoCanvas;
                            currentTime = -1;
                            
                        }
                    };
                }
            };
        }
        else
        {
            transform.position += CustomMath.Normalize(aux) * Time.deltaTime * tagSpeed*(aux.sqrMagnitude+tagSpeedOffset);
        }
       
    }
    private float variableSpeed;
    public float gravity=0;
    public float randomX=0;
    private float currentXSpeed;
    private void Normal()
    {
        variableSpeed += Time.deltaTime * gravity;
        transform.position += Vector3.up * Time.deltaTime * (currentSpeed+ variableSpeed) + Vector3.right * currentXSpeed * Time.deltaTime; ;
        transform.rotation = Camera.main.transform.rotation;
    }

    public void GoTo(string gotoTag)
    {
        //if(gotoTag!=null&&gotoTag!="")
        {
            canvasObjectToGo = GameObject.FindGameObjectWithTag(gotoTag);
            UpdateDestiny();
            onUpdate = GoCanvas;
        }
        
    }
    private float zAux=1;
    private void UpdateDestiny()
    {
        zAux += Time.deltaTime*2*zAux;
        if(zAux>zOffset)
        {
            zAux = zOffset;
        }
        //Debug.Log(zAux);
        destiny = ((canvasObjectToGo.transform.position.x / Screen.width) * Vector3.right + (canvasObjectToGo.transform.position.y / Screen.height) * Vector3.up) + Vector3.forward * zAux;
    }
    private void GoNormal()
    {
        
        onUpdate = Normal;
    }
    public void StartUpdate()
    {
        finalAction = null;
        if (canvasObjectToGo!=null)
        {
            onUpdate = GoCanvas;
        }
        else
        {
            GoNormal();
        }
    }
}
