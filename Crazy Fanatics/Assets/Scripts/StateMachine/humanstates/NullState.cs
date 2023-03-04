using UnityEngine;
using System.Collections;
using System;

public class NullState : HumanState
{

    
	public NullState(Human human,StateMachine sm):base(sm,human)
	{
        
	}

	public override void Awake ()
	{
       
    }

	public override void Sleep ()
	{

	}

	public override void Update ()
	{
      
	}

	
	public override void OnCollision(Collision col)
	{}
	public override void OnTrigger(Collider col){}
    //public override void ChangeState(Type type)
    //{
        
    //}
}
