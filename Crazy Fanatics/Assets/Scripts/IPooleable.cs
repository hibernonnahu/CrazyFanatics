using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooleable {

    void SetPoolParent(Pool p);
    bool IsTimeToLeave();
   
    GameObject GetGameObject();
    void DisableElement();
    void ActivateElement(float optionalTime = -1,float optionalSpeed=-1);
}
