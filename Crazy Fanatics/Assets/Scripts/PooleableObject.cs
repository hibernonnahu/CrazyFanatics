using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooleableObject : MonoBehaviour, IPooleable
{
    public float speed = 1f;
    protected float currentSpeed;
    public float time = 1.5f;
    protected float currentTime;
    public void ActivateElement(float optionalTime=-1,float optionalSpeed=-1)
    {
        gameObject.SetActive(true);
        if(optionalTime==-1)
        {
            currentTime = time;
        }
        else
        {
            currentTime = optionalTime;
        }
        if (optionalSpeed == -1)
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = optionalSpeed;
        }
        //Debug.Log("element activated");
    }

    public void DisableElement()
    {
        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool IsTimeToLeave()
    {
        currentTime -= Time.deltaTime;
        return (currentTime < 0);
    }

   

    virtual public void SetPoolParent(Pool p)
    {
       
    }
}
