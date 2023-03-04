using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool :MonoBehaviour {
    
    public GameObject pooleableObject;
    
    private List<IPooleable> storedList=new List<IPooleable>();
    private List<IPooleable> usedList = new List<IPooleable>();
    public int elementsToCreate = 8;
	void Start () {
        CreateElements(pooleableObject.GetComponent<IPooleable>());
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = usedList.Count-1; i >= 0; i--)
        {
            if(usedList[i].IsTimeToLeave())
            {
                RestoreElement(usedList[i]);
            }
        }
	}
    private void CreateElements(IPooleable poolObj) 
    {
        if(poolObj==null)
        {
            Debug.LogError("intentando crear un no IPooleable");
            Destroy(this.gameObject);
            return;
        }
        for (int i = 0; i < elementsToCreate; i++)
        {
            GameObject go = Instantiate<GameObject>(poolObj.GetGameObject());
            IPooleable ip = go.GetComponent<IPooleable>();
            ip.SetPoolParent(this);
            ip.DisableElement();
            storedList.Add(ip);
            
        }
    }
    public GameObject GetElement(Vector3 position,bool activated=true,float optionalTime=-1,float speed=-1)
    {
        if(storedList.Count>0)
        {
            GameObject go = storedList[0].GetGameObject();
            if(activated)
            {
                go.transform.position = position;
                
                storedList[0].ActivateElement(optionalTime,speed);
            }
            usedList.Add(storedList[0]);
             storedList.RemoveAt(0);
            return go;
        }
        else
        {
            return null;
        }
    }
    public void RestoreElement(IPooleable ip)
    {
        
        if(usedList.Remove(ip))
        {
            ip.DisableElement();
            storedList.Add(ip);
        }
    }
}
