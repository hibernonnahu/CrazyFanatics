using UnityEngine;
using System.Collections;
using System;

public class Idle : HumanState
{

    
	public Idle(Human human,StateMachine sm):base(sm,human)
	{
        
	}

	public override void Awake ()
	{
        //Debug.Log("idle awake");
        human.Rigidbody.velocity = Vector3.zero;
        if(human.debug)
        { }
        human.SetAnimation("idle", 0.1f, 1);
    }

	public override void Sleep ()
	{

	}

	public override void Update ()
	{
        human.Alert();
        human.decitionBehaviour();
        
	}

	
	public override void OnCollision(Collision col)
	{}
	public override void OnTrigger(Collider col){}
    //public override void ChangeState(Type type)
    //{
        
    //}
}
