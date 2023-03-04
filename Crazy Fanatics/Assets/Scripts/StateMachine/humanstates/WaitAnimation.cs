using UnityEngine;
using System.Collections;
using System;

public class WaitAnimation : HumanState
{
   
    private float counter;
    private Type nextState;
    private bool start;
	public WaitAnimation(Human human,StateMachine sm):base(sm,human)
	{
        
	}

	public override void Awake ()
	{
        //Debug.Log("idle awake");
        //human.Rigidbody.velocity = Vector3.zero;
        start = false;
        human.EnableCollider(true);
        human.Rigidbody.useGravity = true;
        human.Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }

    public override void Sleep ()
	{
        human.EnableCollider(false);
        human.Rigidbody.useGravity = false;
        human.Rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

	public override void Update ()
	{
        if(start)
        {
            counter -= Time.deltaTime;
            if(counter<0)
            {
                ChangeState(nextState);
            }
        }
       
	}

	
	public override void OnCollision(Collision col)
	{}
	public override void OnTrigger(Collider col){}
    public void SetTime(float duration)
    {
        counter = duration;
    }
    public void SetParameters(float duration,Type nextState,string animation)
    {
        counter = duration;
        this.nextState = nextState;
        start = true;
    }
    //public override void ChangeState(Type type)
    //{
    //    if(type !=typeof(WaitAnimation))
    //    {
    //        base.ChangeState(type);
    //    }
        
    //}
    //public override void ChangeState(Type type)
    //{

    //}
}
