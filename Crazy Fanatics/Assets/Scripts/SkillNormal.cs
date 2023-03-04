using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new skill")]
public class SkillNormal :Skill {
    
    public enum FunctionalType
    {
       stun,
       damage
    }
    
    [SerializeField]
    public FunctionalType functionalType;

    public enum CastingIACondition
    {
        healUnder25,
        healUnder50,
        healUnder75
        
    }

    [SerializeField]
    public CastingIACondition castingIACondition;

    public int damage = 0;
    [Range(0, 1f)]
    public float leechLife = 0;
    public float effectDuration = 0;
    private Func<Human,bool> castingCondition;
    public GameObject rayEffect;
    public float rayYPosition = 1;
    public float rayCenterDistance=1;
    
    public float rayDuration;
    public override void Init(Human caster)
    {
        base.Init(caster);
        if(caster.RayEffect!=null)
        {
            Destroy(caster.RayEffect.gameObject);
        }
        if(rayEffect!=null)
        {
            caster.RayEffect = Instantiate(rayEffect);
            caster.RayEffect.transform.position = Vector3.zero;
            
            caster.RayEffect.SetActive(false);
        }
       
        switch (castingIACondition)
        {
            case CastingIACondition.healUnder25:
                castingCondition = healUnder25;
                break;
            case CastingIACondition.healUnder50:
                castingCondition = healUnder50;
                break;
            case CastingIACondition.healUnder75:
                castingCondition = healUnder75;
                break;
            default:
                break;
        }
    }
    private bool healUnder25(Human human)
    {
        return human.GetLifePercent() < 0.25;
    }
    private bool healUnder50(Human human)
    {
        //Debug.Log(human.GetLifePercent());
        return human.GetLifePercent() < 0.50;
    }
    private bool healUnder75(Human human)
    {
        return human.GetLifePercent() < 0.75;
    }
    public override bool OnExecute(ITargeteable victim,Human caster)
    {
        base.OnExecute(victim,caster);

        if (caster.SkillCounter<=0)
        {
            caster.SkillCounter = coolDown;
            if(caster.LineEffect!=null)
            {
                caster.RayEffect.SetActive(true);
                
                caster.LineEffect.SetPosition(0, caster.transform.position+Vector3.up*rayYPosition);
                Vector3 initialPosition = caster.transform.position + Vector3.up * rayYPosition*caster.stats.height;
                initialPosition += CustomMath.Normalize(victim.GetHuman().transform.position - caster.transform.position) * rayCenterDistance;
                caster.LineEffect.transform.parent.transform.position = initialPosition;
                caster.LineEffect.SetPosition(0, initialPosition);
                caster.LineEffect.SetPosition(1, victim.GetHuman().transform.position + Vector3.up * rayYPosition*victim.GetHuman().stats.height);
                caster.CancelInvoke("TurnOffRayEffect");
                caster.Invoke("TurnOffRayEffect", rayDuration);
            }
            switch (functionalType)
            {
                case FunctionalType.stun:
                    int dmg=victim.GetHuman().TakeHit(caster, damage);
                    victim.GetHuman().CancelSkill();
                    caster.ModifyLife((int)(dmg * leechLife));
                    caster.ChangeState(typeof(NullState));
                    caster.Rigidbody.velocity = Vector3.zero;
                    caster.ChangeState(typeof(WaitAnimation));
                    WaitAnimation wa = (WaitAnimation)(caster.GetCurrentState());
                    
                    wa.SetParameters(castDuration, typeof(GoToPositionTarget), "");

                    State victimLastState = victim.GetHuman().GetCurrentState();
                    if(victimLastState is Hit)
                    {
                        ((Hit)(victimLastState)).Reset();
                    }
                    victim.GetHuman().SetInteractionFeedBack("stun", effectDuration);
                    victim.GetHuman().ChangeState(typeof(WaitAnimation));
                    if(victim.GetHuman().GetCurrentState() is WaitAnimation)
                    {
                        victim.GetHuman().Rigidbody.velocity = Vector3.up * 3;
                        WaitAnimation wa2 = (WaitAnimation)(victim.GetHuman().GetCurrentState());
                        wa2.SetParameters(effectDuration, victimLastState.GetType(), "");
                        victim.GetHuman().SetAnimation("hittobody", 0.1f, 1);
                    }
                    
                    caster.UpdateSkillUI(0);
                    break;
                case FunctionalType.damage:
                     dmg = victim.GetHuman().ModifyLife( -damage);

                    caster.ModifyLife((int)(dmg * leechLife));
                    
                    caster.ChangeState(typeof(NullState));
                    caster.Rigidbody.velocity = Vector3.zero;
                    caster.ChangeState(typeof(WaitAnimation));
                     wa = (WaitAnimation)(caster.GetCurrentState());
                    wa.SetParameters(castDuration, typeof(GoToPositionTarget), "");

                   
                   
                   

                    caster.UpdateSkillUI(0);
                    break;
                default:
                    break;
            }
            return true;
        }
        return false;
    }

    

    public override void UpdateCoolDown(List<ICooldown> list,Human caster)
    {
        caster.SkillCounter -= Time.deltaTime;
        if(caster.SkillCounter < 0)
        {
            caster.UpdateSkillUI(1);
            list.Remove(this);
        }
        else
        {
            caster.UpdateSkillUI(1-(caster.SkillCounter / coolDown));
        }
    }
    
    public override bool IACastCondition(Human human)
    {
        return castingCondition(human);
    }

}
