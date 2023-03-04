using UnityEngine;
using System.Collections;
using System;


public class State
{
	protected StateMachine stateMachine;
	// Use this for initialization
	public  State (StateMachine sm)
	{
		stateMachine = sm;
	}

	public virtual void Awake ()
	{

	}

	public virtual void Sleep ()
	{

	}

	public virtual void Update ()
	{

	}

	public virtual void OnLookAtMouse ()
	{

	}
	
	public virtual void OnMouseUp ()
	{

	}
	
	public virtual void OnMousePress ()
	{
	
	}
	public virtual void OnMouseDown( )
	{

	}
	public virtual void OnCollision(Collision col)
	{}
    public virtual void OnCollisionStay(Collision col)
    { }
    public virtual void OnTrigger(Collider col){}
    public virtual void ChangeState(Type type)
    {
        this.stateMachine.changeState(type);
    }
}
