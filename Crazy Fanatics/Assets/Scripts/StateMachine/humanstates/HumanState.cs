using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanState : State {
    protected Human human;
    
	public HumanState(StateMachine sm,Human h):base(sm)
    {
        human = h;
    }
    virtual public void OnHelpWanted()
    {

    }
    virtual public void OnSkillActivated()
    { }
    virtual public void Reset() { }
}
