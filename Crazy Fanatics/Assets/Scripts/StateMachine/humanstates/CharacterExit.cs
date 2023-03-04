using UnityEngine;
using System.Collections;
using System;

public class CharacterExit : HumanState
{

    Vector3 direction;
    Exit exit;
	public CharacterExit(Human human,StateMachine sm):base(sm,human)
	{
        
	}

	public override void Awake ()
	{
        //Debug.Log("idle awake");
        exit = GameObject.FindObjectOfType<Exit>();
        direction = exit.transform.position+Vector3.up*0.6f - human.transform.position;
        direction = direction.normalized;
    }

	public override void Sleep ()
	{

	}

	public override void Update ()
	{
       if((exit.transform.position.x*Vector3.right+ exit.transform.position.z * Vector3.forward - (human.transform.position.x*Vector3.right+ human.transform.position.z * Vector3.forward)).sqrMagnitude<0.05f)
        {
            human.Rigidbody.velocity = Vector3.zero;
        }
        else
        {
            human.Rigidbody.velocity = direction*human.stats.speed;
        }
	}

	
	public override void OnCollision(Collision col)
	{}
	public override void OnTrigger(Collider col){}
    //public override void ChangeState(Type type)
    //{

    //}
    public override void ChangeState(Type type)
    {
        
    }
}
