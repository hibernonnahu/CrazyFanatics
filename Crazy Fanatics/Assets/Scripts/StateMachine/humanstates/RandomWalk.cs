using UnityEngine;
using System.Collections;
using System;

public class RandomWalk : GoToPosition
{
    
    private int LAYER_WALL = LayerMask.NameToLayer("wall");
    public RandomWalk(Human human,StateMachine sm):base(human,sm)
	{
       
	}

	public override void Awake ()
	{
        base.Awake();
        if(human.debug)
        { }
        if(human.stats.patrolWalkRange==0)
        {
            if((human.transform.position-human.initialPosition).sqrMagnitude<1f)
            {
                ChangeState(typeof(Idle));
            }
            else
            {
                human.InputDestiny = human.initialPosition;
                ChangeState(typeof(GoToPositionInput));
            }
        }
        else
        {
            OnArrive();
        }
       
	}

	public override void Sleep ()
	{

	}

	public override void Update ()
	{
        if(human.debug)
        {

        }
        base.Update();
        human.decitionBehaviour();
        //Debug.Log("vel " + human.Rigidbody.velocity);
    }
    protected override void OnArrive()
    {
        human.InputDestiny = Astar.getInstance().GetClosestNode(
            Vector3.right * (human.initialPosition.x + UnityEngine.Random.Range(-human.stats.patrolWalkRange, human.stats.patrolWalkRange)) + Vector3.forward * (human.initialPosition.z + UnityEngine.Random.Range(-human.stats.patrolWalkRange, human.stats.patrolWalkRange)))
            .transform.position;
        //Debug.Log(human.Destiny);
    }
    protected override void WallObstruction()
    {
        OnArrive();
    }
    public override void OnCollision(Collision col)
	{
       // Debug.Log("collision " + col.gameObject.layer + "  " + LAYER_WALL);
        if(col.gameObject.layer == LAYER_WALL)
        {
            OnArrive();
        }
    }
    public override void OnCollisionStay(Collision col)
    {
        OnCollision(col);
    }
    public override void OnTrigger(Collider col){}
    //public override void ChangeState(Type type)
    //{
        
    //}
}
