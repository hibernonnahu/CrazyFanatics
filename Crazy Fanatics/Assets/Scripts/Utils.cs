using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

    public static GameObject FindGameObjectInChildren(string objectName, GameObject parentObject)
    {
        GameObject go = null;

        foreach (GameObject child in GetChildRecursive(parentObject))
        {
            if (child.name == objectName)
            {
                go = child;
                break;
            }
        }
        return go;
    }

    private static List<GameObject> GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return null;
        List<GameObject> listOfChildren = new List<GameObject>();
        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            listOfChildren.Add(child.gameObject);
            listOfChildren.AddRange(GetChildRecursive(child.gameObject));
        }
        return listOfChildren;
    }
}
