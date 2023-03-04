using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoThroughtPathTarget : GoThroughtPath {
    private float currentSpeed;
    private bool skillActivated;
    public GoThroughtPathTarget(Human human, StateMachine sm) : base(human, sm)
    {
        // mainCharacter = (MainCharacter)human;
    }
    public override void Awake()
    {
        base.Awake();
        human.ActivateSelected(true);
        skillActivated = false;
        GameObject targetGO = null;
        if (human.Target != null)
        {
            targetGO = human.Target.GetTargetGameObject();
        }
       else
        {
            ChangeState(typeof(Idle));
        }
         if ( targetGO == human.protectedHuman.gameObject || (human.Target == human.protectedHuman.Target&&human!=human.protectedHuman))
        {
            currentSpeed = human.stats.protectFarSpeed;
        }
        else
        {
            currentSpeed = human.stats.speed;
        }
        human.SetAnimation("walk", 0, GetSpeed());
    }
    public override void Sleep()
    {
        base.Sleep();
        human.ActivateSelected(false);
        human.ActivateSkillUI(false);
    }
    override protected Vector3 GetRealTargetPosition()
    {
        if(human.Target==null)
        { }
        return human.Target.GetTargetGameObject().transform.position.x * Vector3.right + human.Target.GetTargetGameObject().transform.position.z * Vector3.forward + Vector3.up * Y_OFFSET + human.Dodge;
    }
    public override void Update()
    {
        base.Update();
        
    }
    protected override void WallObstruction()
    {
        //if (human.Target == human.protectedHuman)
        //{
        //    currentSpeed = human.stats.protectFarSpeed;
        //}
        //else
        //{
        //    currentSpeed = human.stats.speed;
        //}
    }
    protected override void NotWallObstruction()
    {
        human.Destiny = human.Target.GetTargetGameObject().transform.position;
        ChangeState(typeof(GoToPositionTarget));
        if(skillActivated)
        {
            HumanState hs = (HumanState)(stateMachine.currentState);
            hs.OnSkillActivated();
        }
        
    }
    protected override void OnDestiny()
    {
        NotWallObstruction();
    }
    override protected bool IsWallCloseThanTarget(Vector3 origin, Vector3 wallPoint)
    {
        return ((wallPoint - origin).sqrMagnitude <
            (human.Target.GetTargetGameObject().transform.position - origin).sqrMagnitude);
    }
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
