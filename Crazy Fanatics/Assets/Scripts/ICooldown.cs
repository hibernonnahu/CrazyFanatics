using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICooldown  {

    
    void UpdateCoolDown(List<ICooldown> list,Human caster);
}
