using UnityEngine;
using System.Collections;
using System;

public class GoToPositionInput : GoToPosition
{
   // private MainCharacter mainCharacter;
    public GoToPositionInput(Human human, StateMachine sm):base(human,sm)
	{
       // mainCharacter = (MainCharacter)human;
    }

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        human.SetAnimation("walk", 0, GetSpeed());
    }
    protected override void OnArrive()
    {
        ChangeState(human.OnPointArrive());
    }

    public override void Update ()
	{
        base.Update();
        human.decitionBehaviour();
	}
    protected override void WallObstruction()
    {
        human.RenewPathToPosition(human.InputDestiny);
        
        ChangeState(typeof(GoThroughtPathPoint));
    }


}
