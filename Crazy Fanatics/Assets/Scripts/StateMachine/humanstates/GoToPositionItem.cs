using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPositionItem : GoToPosition {

    public GoToPositionItem(Human human, StateMachine sm) : base(human, sm)
    {
         ARRIVE_SQR_DISTANCE = 0.05f;
}
    public override void Awake()
    {
        base.Awake();
        human.MarkPosition(false);
    }
    protected override void WallObstruction()
    {
        //crear estado de pathfinding hacia el objeto
    }
    protected override void OnArrive()
    {
        ChangeState(typeof(Idle));
        human.Item.OnHumanGrab(human);
        
    }
}
