using UnityEngine;
using System.Collections;
using System;

public class ProtectedRandomWalk : RandomWalk
{
   
    public ProtectedRandomWalk(Human human,StateMachine sm):base(human,sm)
	{

	}

    public override void Awake()
    {
        OnArrive();
        human.SetAnimation("walk", 0, GetSpeed());
    }
    protected override void OnArrive()
    {
        human.InputDestiny = Astar.getInstance().GetClosestNode(
            Vector3.right * (human.protectedHuman.transform.position.x + UnityEngine.Random.Range(-human.stats.protectPatrolWalkRange, human.stats.protectPatrolWalkRange)) + Vector3.forward * (human.protectedHuman.transform.position.z + UnityEngine.Random.Range(-human.stats.protectPatrolWalkRange, human.stats.protectPatrolWalkRange)))
            .transform.position;
        
    }
    public override void Update()
    {
        base.Update();
        //Debug.Log(
        //    (human.protectedHuman.transform.position - human.transform.position).sqrMagnitude +"  "+ human.stats.protectionRange);
       if((human.protectedHuman.transform.position-human.transform.position).sqrMagnitude>human.stats.protectPatrolWalkRange)
        {
            human.Target = human.protectedHuman;
            ChangeState(typeof(GoToPositionTarget));
        }
        else
        {
            human.Alert();
        }
    }
    override protected float GetSpeed()
    {
        return human.stats.protectionSpeed;
    }
    public override void OnHelpWanted()
    {
        human.OnProtectedAttack();
    }
    //public override void ChangeState(Type type)
    //{

    //}
}
