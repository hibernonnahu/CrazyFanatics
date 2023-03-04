using UnityEngine;
using System.Collections;
using System;

public class GoToPosition : HumanState
{
    protected const float Y_OFFSET= 0.1f;
    private  int mask = 1 << LayerMask.NameToLayer("wall");
    private int maskHuman = 1 << LayerMask.NameToLayer("human");
    protected  float ARRIVE_SQR_DISTANCE = 0.1f;

    private const float RAYCAST_TICK = 0.2f;
    private float raycastCounter = 0;
	// Use this for initialization
	public GoToPosition(Human human,StateMachine sm):base(sm,human)
	{
	
	}

	public override void Awake ()
	{
        //Debug.Log("gotoposition awake");
        human.Target = null;
        human.MarkPosition(true);
        human.SetAnimation("walk", 0, GetSpeed());
    }

	public override void Sleep ()
	{
        //Debug.Log("gotoposition leave");
        human.MarkPosition(false);
    }

	public override void Update ()
	{
        Vector3 vec = (human.Destiny - (human.Transform.position.x*Vector3.right+Vector3.forward*human.Transform.position.z));
        //Debug.Log(vec.sqrMagnitude);
        if (vec.sqrMagnitude<ARRIVE_SQR_DISTANCE)
        {
            OnArrive();
        }
        else
        {
            if(human.debug)
            {

            }
            human.LookAt(human.Destiny);
            human.Rigidbody.velocity = CustomMath.XZNormalize(vec) * GetSpeed();
            //Debug.Log("velocity "+human.rigidbody.velocity);
            //Debug.Log("position " + human.transform.position);
            //Debug.Log("destiny " + human.Destiny);
            //Debug.Log("vec " + vec);
        }
        CheckRaycastToPosition();
        human.decitionBehaviour();
	}
    virtual protected float GetSpeed()
    {
        return human.stats.speed;
    }
    virtual protected void CheckRaycastToPosition()
    {
        raycastCounter -= Time.deltaTime;
        if(raycastCounter<0)
        {
            raycastCounter = RAYCAST_TICK;
            CastRayToPosition();
        }
    }
    Vector3 origin;
     protected void CastRayToPosition()
    {
        //if (human.debug)
        {
            RaycastHit hit;
            origin = human.transform.position.x * Vector3.right + human.transform.position.z * Vector3.forward + Vector3.up * Y_OFFSET;
            //Debug.Log("origin " + origin);
            //Debug.Log("destiny " + human.Destiny);
            //Debug.DrawRay(origin, GetRealTargetPosition()-origin, Color.green);
            if (Physics.Raycast(origin,
                (GetRealTargetPosition() - (origin)),
                out hit, 10f, mask))
            {
                //Debug.Log("hit wall at " + hit.point);
                if (IsWallCloseThanTarget(origin, hit.point))
                {
                    WallObstruction();

                }
                else
                {
                    NotWallObstruction();
                }
            }
            else
            {

                NotWallObstruction();
            }
        }
    }
    virtual protected Vector3 GetRealTargetPosition()
    {
        return human.Destiny.x * Vector3.right + human.Destiny.z * Vector3.forward + Vector3.up * Y_OFFSET+human.Dodge;
    }
    virtual  protected void WallObstruction()
    {
        //Debug.Log("wall obstruction");
    }
    virtual protected void NotWallObstruction()
    {
        human.Dodge = Vector3.zero;

        foreach (var ally in human.allyList)
        {
            float dist = (human.stats.Radius + ally.stats.Radius);
            dist = 3;
            if (ally!=human&&(ally.transform.position-human.transform.position).sqrMagnitude<dist)
        
        
                {
                    Vector3 guyInTheWay = CustomMath.Normalize(ally.transform.position.x * Vector3.right + ally.transform.position.z * Vector3.forward - (human.transform.position.x * Vector3.right + human.transform.position.z * Vector3.forward));
                    Vector3 toTarget = GetRealTargetPosition() - (origin);
                    Vector3 toTargetRight = CustomMath.Normalize(Vector3.right * toTarget.z + Vector3.back * toTarget.x);
                    bool goRight = Vector3.Dot(toTargetRight, guyInTheWay) < 0;
                    //Debug.Log("goRight " + goRight);
                    if (!goRight)
                    {
                        guyInTheWay = guyInTheWay.x * Vector3.forward + guyInTheWay.z * Vector3.left;

                    }
                    else
                    {
                        guyInTheWay = guyInTheWay.x * Vector3.back + guyInTheWay.z * Vector3.right;
                    }
                    //Debug.DrawRay(origin, guyInTheWay*4 , Color.red);
                    //Debug.DrawRay(hit.collider.transform.position+Vector3.up*Y_OFFSET, guyInTheWay*dodge, Color.red);
                    //Debug.Break();
                    human.Dodge += guyInTheWay * 2;

                }
            
        }
        //Debug.Log("not wall obstruction");
    }
    virtual protected bool IsWallCloseThanTarget(Vector3 origin,Vector3 wallPoint)
    {
        return ((wallPoint-origin).sqrMagnitude<
            (human.Destiny-origin).sqrMagnitude);
    }
    virtual protected void OnArrive() {
        ChangeState(typeof(Idle));
            }
	public override void OnCollision(Collision col)
	{}
    public override void OnCollisionStay(Collision col)
    { }
    public override void OnTrigger(Collider col){}
    
}
