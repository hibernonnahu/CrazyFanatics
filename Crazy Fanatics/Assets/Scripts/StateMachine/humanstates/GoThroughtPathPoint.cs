using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoThroughtPathPoint : GoThroughtPath {
    private float currentSpeed;
    public GoThroughtPathPoint(Human human, StateMachine sm) : base(human, sm)
    {
        // mainCharacter = (MainCharacter)human;
    }
    public override void Awake()
    {
        base.Awake();
        if(!(human is MainCharacter))
        {
            Debug.Log("ia error ");
        }
        else
        {
            human.MarkPosition(true);
            currentSpeed = human.stats.speed;
        }
        human.SetAnimation("walk", 0, GetSpeed());
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Sleep()
    {
        base.Sleep();
        human.MarkPosition(false);
    }
    override protected Vector3 GetRealTargetPosition()
    {
        return human.InputDestiny.x * Vector3.right + human.InputDestiny.z * Vector3.forward + Vector3.up * Y_OFFSET + human.Dodge;
    }
    protected override void CheckRaycastToPosition()
    {
       
    }
    override protected float GetSpeed()
    {
        return currentSpeed;
    }
    protected override void OnArrive()
    {
        base.OnArrive();
        CastRayToPosition();
    }
    protected override void NotWallObstruction()
    {
        //Debug.Log("NotWallObstruction " + human.InputDestiny);
        human.Destiny = human.InputDestiny;
        ChangeState(typeof(GoToPositionInput));
    }
    protected override void OnDestiny()
    {
        ChangeState(human.OnPointArrive());
    }
    override protected bool IsWallCloseThanTarget(Vector3 origin, Vector3 wallPoint)
    {
        return ((wallPoint - origin).sqrMagnitude <
            (human.InputDestiny - origin).sqrMagnitude);
    }
    public override void ChangeState(Type type)
    {
        if(type!=typeof(GoThroughtPathPoint))
        {
            base.ChangeState(type);
        }
       
    }
}
