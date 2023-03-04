using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCastle : Dead
{


    public DeadCastle(Human human, StateMachine sm) : base(human, sm)
    {

    }

    public override void Awake()
    {
        //Debug.Log("idle awake");
       
        human.Rigidbody.velocity = Vector3.zero;
        human.Dead = true;
        
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        human.transform.localScale *= 0.9f;
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
