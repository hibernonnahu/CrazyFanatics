using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : HumanState
{


    public Dead(Human human, StateMachine sm) : base(sm, human)
    {

    }

    public override void Awake()
    {
        //Debug.Log("idle awake");
        human.CancelInvoke();
        human.CancelSkill();
        if(human.enemyLayer==LayerMask.NameToLayer("human"))
        {
            WinningManager.Instance.CheckEnemyKill(human.stats.id);
        }
        
        human.Rigidbody.velocity = Vector3.zero;
        human.Dead = true;
        human.mainBar.transform.position = Vector3.up * 999;
        human.SetInteractionFeedBack("dead");
        human.OnDeadEnter();
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        human.OnDeadUpdate();
        
    }

    public override void ChangeState(Type type)
    {
       
    }
    public override void OnCollision(Collision col)
    { }
    public override void OnTrigger(Collider col) { }
    //public override void ChangeState(Type type)
    //{

    //}
}
