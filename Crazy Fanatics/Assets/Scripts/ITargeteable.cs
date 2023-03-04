using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargeteable  {
     GameObject GetTargetGameObject();
    float GetRadius();
    Human GetHuman();
    Item GetItem();
    void SetSelected(bool b);
}
