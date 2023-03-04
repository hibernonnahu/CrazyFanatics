using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancelButtonOnUI 
{
    static List<RaycastResult> results = new List<RaycastResult>();
    static List<RaycastResult> helper = new List<RaycastResult>();
    public static bool IsOnUI()
    {
        //#if UNITY_EDITOR
        //        return (EventSystem.current.IsPointerOverGameObject());
        //#else

        //return (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) ||EventSystem.current.IsPointerOverGameObject());
        //#endif

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Vector2.right * Input.mousePosition.x + Vector2.up * Input.mousePosition.y;
        results.Clear();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        //    bool aux =results.Count > 0;
        //Debug.Log("ui ---------------------------");
        foreach (var item in results)
        {
            if (item.gameObject.layer != 5)
            {
                helper.Add(item);
            }
        }
        foreach (var item in helper)
        {
            results.Remove(item);
        }
        return results.Count > 0;


    }
}
