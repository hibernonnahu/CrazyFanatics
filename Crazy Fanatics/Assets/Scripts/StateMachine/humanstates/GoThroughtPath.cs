using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoThroughtPath : GoToPosition {
    private int mask = 1 << LayerMask.NameToLayer("wall");
    public GoThroughtPath(Human human, StateMachine sm) : base(human, sm)
    {
        // mainCharacter = (MainCharacter)human;
        ARRIVE_SQR_DISTANCE = 0.05f;
    }
    public override void Awake()
    {
        if(human.path==null||human.path.Count<=1)
        {
            WallObstruction();
        }
        else if(human.path.Count>1)
        {
            RaycastHit hit;
            Vector3 origin = human.transform.position.x * Vector3.right + human.transform.position.z * Vector3.forward + Vector3.up * Y_OFFSET;
            //Debug.Log("origin " + origin);
            //Debug.Log("destiny " + human.Destiny);
            //Debug.DrawRay(origin, GetRealTargetPosition() - origin, Color.green);
            if (Physics.Raycast(origin,
                ((human.path[1].transform.position + Vector3.up * Y_OFFSET )- (origin)),
                out hit, 10f, mask))
            {
                //Debug.Log("hit wall at " + hit.point);
                if (!IsWallCloseThanPoint(origin, hit.point,human.path[1].transform.position))
                {
                    human.path.RemoveAt(0);
                    //Debug.Log("removeinitialnode");
                }
               
            }
            else
            {

                human.path.RemoveAt(0);
                //Debug.Log("removeinitialnode");
            }
        }
        human.SetAnimation("walk", 0, GetSpeed());
        GetNewNodeDirection();

    }
    private bool IsWallCloseThanPoint(Vector3 origin, Vector3 wallPoint,Vector3 point)
    {
        return ((wallPoint - origin).sqrMagnitude <
            (point - origin).sqrMagnitude);
    }
    protected override void OnArrive()
    {
        if(human.path!=null)
        {
            human.path.RemoveAt(0);

            GetNewNodeDirection();
        }
        else
        {
            human.OnCancelPersecution();
        }
       
        
        
    }
    virtual protected void OnDestiny()
    {

    }
    private void GetNewNodeDirection()
    {
        if ( human.path == null|| human.path.Count == 0 )
        {
            OnDestiny();
        }
        else
        {
            human.Destiny = human.path[0].transform.position.x * Vector3.right + human.path[0].transform.position.z * Vector3.forward;
        }
    }

}
