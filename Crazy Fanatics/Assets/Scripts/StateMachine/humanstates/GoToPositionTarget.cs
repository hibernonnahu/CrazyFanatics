using UnityEngine;
using System.Collections;
using System;

public class GoToPositionTarget : GoToPositionInput
{
    private float currentSpeed;
    private bool skillActivated;
    // Use this for initialization
    public GoToPositionTarget(Human human,StateMachine sm):base(human,sm)
	{
        
	}

	public override void Awake ()
	{
        if(human.debug)
        {

        }
        skillActivated = false;
        human.ActivateSelected(true);
        GameObject targetGO = null;
        if (human.Target != null)
        {
            targetGO = human.Target.GetTargetGameObject();
        }
        if (targetGO == human.protectedHuman.gameObject|| (human.Target == human.protectedHuman.Target && human != human.protectedHuman))
        {
            currentSpeed = human.stats.protectFarSpeed; 
        }
        else
        {
            currentSpeed = human.stats.speed;
        }
        human.SetAnimation("walk", 0, GetSpeed());
    }

	public override void Sleep ()
	{
        human.ActivateSelected(false);
        human.ActivateSkillUI(false);
        //Debug.Log("gotopositiontarget leave");
    }

	public override void Update ()
	{
        
        if(human.Target != null)
        {
            human.Destiny = human.Target.GetTargetGameObject().transform.position.x * Vector3.right + human.Target.GetTargetGameObject().transform.position.z * Vector3.forward;
           
            if (human.skill!=null&&skillActivated&&human.skill.castDistanse>(human.Destiny-CustomMath.Vector3NotNew(human.transform.position.x,0,human.transform.position.z)).sqrMagnitude)
            {
                human.CastSkill();
            }
            else
            {
                ARRIVE_SQR_DISTANCE = (human.Target.GetRadius() + human.stats.Radius);
                if(human.Target.GetHuman()!=null)
                {
                    if (human.Target.GetHuman().enemyLayer != human.enemyLayer)
                    {
                        ARRIVE_SQR_DISTANCE += human.stats.AttackDistance + human.GetWeaponAttackDistance();
                    }
                }
                
                base.Update();
            }
            
        }
        else
        {
            human.OnCancelPersecution();
        }
       

    }

    
    protected override void OnArrive()
    {
        if (human.Target != null)
        {
            
            if(human.Target.GetTargetGameObject().layer==human.enemyLayer)
            {
                if (human.skill != null && skillActivated )
                {
                    human.CastSkill();
                }
                else
                {
                    ChangeState(typeof(Hit));
                }
                
            }
            else if (human.Target.GetItem()!=null)
            {
                human.Target.GetItem().Grab();
                human.OnCancelPersecution();
            }
            else
            {
                human.Target = null;
                ChangeState(typeof(ProtectedRandomWalk));
            }  
            
        }
        else
        {
            human.OnCancelPersecution();
        }
        

    }
    protected override void WallObstruction()
    {
       if(human.Target==null)
        {
            human.OnCancelPersecution();
        }
        else
        {
            human.RenewPathToPosition(human.Target.GetTargetGameObject().transform.position);

            ChangeState(typeof(GoThroughtPathTarget));
            if (skillActivated)
            {
                HumanState hs = (HumanState)(stateMachine.currentState);
                hs.OnSkillActivated();
            }
        }
        
    }
   
    public override void OnCollision(Collision col)
	{
       
    }
    public override void OnCollisionStay(Collision col)
    { }
    public override void OnTrigger(Collider col){}
    override protected float GetSpeed()
    {
        return currentSpeed;
    }
    public override void OnHelpWanted()
    {
        
        human.OnProtectedAttack();
    }
    public override void OnSkillActivated()
    {
        skillActivated = true;
        human.ActivateSkillUI(true);
    }
}
