using UnityEngine;
using System.Collections;
using System;

public class Hit : HumanState
{

   
    private float counter;
    private bool toAlternate = true;
	public Hit(Human human,StateMachine sm):base(sm,human)
	{
        counter = human.GetCastTime();
    }

	public override void Awake ()
	{
        //Debug.Log("Hit awake");

        Init();
       
	}
    private void Init()
    {
        counter = human.GetCastTime();
        human.Rigidbody.velocity = Vector3.zero;
        if(human.Target!=null)
        {
            if ( human. CanConvert(human.Target.GetHuman()))
            {


                GameManager.Instance.MainCharacter.CastItemConsumible();
               

            }
            else
            {
                
                human.SetAnimation("attack", human.GetCastTime() - counter, 1 / human.GetCastTime());
                human.Invoke("PlaySwordSound", 0.5f);
            }
        }
        else
        {
            human.OnCancelPersecution();
        }
        
    }
	public override void Sleep ()
	{
        human.CancelInvoke();
        //Debug.Log("Hit sleep");
    }

	public override void Update ()
	{
        if(human.Target!=null)
        {
            counter -= Time.deltaTime;
            human.LookAt(human.Target.GetTargetGameObject().transform.position);
            if (human.Target.GetTargetGameObject().layer != human.enemyLayer || human.Target.GetHuman().Dead)
            {
                human.Target = null;
                human.OnCancelPersecution();
            }
            else if (counter < 0)
            {
                counter = human.GetCastTime();
                float minDistance = human.stats.AttackDistance + human.GetWeaponAttackDistance() + (human.Target.GetRadius() + human.stats.Radius) + 1.4f;
                if ((human.protectedHuman.transform.position - human.transform.position).sqrMagnitude > human.stats.protectPatrolWalkRange)
                {
                    human.Target = human.protectedHuman;
                    ChangeState(typeof(GoToPositionTarget));
                }
                else if(human.skill!=null&& (human.skill.afectType==Skill.Afects.allys|| human.skill.afectType == Skill.Afects.allysonly)&&toAlternate)
                {
                    toAlternate = false;
                    human.OnCancelPersecution();
                }
                else if (!human.Target.GetHuman(). Dead&& (human.Target.GetTargetGameObject(). transform.position - human.transform.position).sqrMagnitude <= minDistance)
                {
                    FxSoundManager.Instance.PlayFx(6);

                    human.Target.GetHuman().TakeHit(human, human.GetDamage());
                    human.OnMakeHitEffect();
                    toAlternate = true;
                    Init();
                }
                else
                {
                    //Debug.Log((human.Target.transform.position - human.transform.position).sqrMagnitude + " " + (human.stats.AttackDistance + human.Target.stats.Radius + human.stats.Radius));
                    human.OnCancelPersecution();
                }
            }
            human.decitionBehaviour();
        }
        else
        {
            human.OnCancelPersecution();
        }
        
	}

	
	public override void OnCollision(Collision col)
	{}
	public override void OnTrigger(Collider col){}
    //public override void ChangeState(Type type)
    //{
    //    if(type != type)
    //}
    public override void Reset()
    {
        counter = human.GetCastTime();
    }
}
