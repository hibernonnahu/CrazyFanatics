using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new skill")]
public class SkillAoE : Skill
{
    
    public enum FunctionalType
    {
        wind,
        damage
    }
    [SerializeField]
    public FunctionalType functionalType;
    [Header("EffectRadius")]
    public float sqrRadius = 10;
    public float tickTime = 0.5f;
    private float tickCounter;
    public GameObject effectGameObject;
    Action<Human,Human> onUpdate;

    [Header("Stats")]
    public float strength = 1f;
    public int damage = 0;

    private string lastCancelInvokeName;
    private Action<Human> skillEffect;
    public override bool OnExecute(ITargeteable victim,Human caster)
    {
        base.OnExecute(victim,caster);

        if (caster.SkillCounter <= 0)
        {
            caster.SkillCounter = coolDown;
            tickCounter = -1;
            skillEffect = OnSkillEffect;
            switch (functionalType)
            {
                case FunctionalType.wind:
                    onUpdate = Wind;
                    caster.SkillEffectDuration = castDuration;
                    caster.SetInteractionFeedBack("wind", castDuration);
                    lastCancelInvokeName = "";
                    caster.UpdateSkillUI(0);
                    break;
                case FunctionalType.damage:
                    if(effectGameObject)
                    {
                        if(caster.Effect==null)
                        {
                            caster.Effect = Instantiate(effectGameObject);
                        }
                        caster.Effect.SetActive(true);
                        caster.Effect.transform.position = caster.transform.position;
                        caster.CancelInvoke("TurnOffEffect");
                        lastCancelInvokeName = "TurnOffEffect";
                        caster.Invoke("TurnOffEffect", castDuration-0.1f);
                    }
                    onUpdate = Damage;
                    caster.SkillEffectDuration = castDuration;
                    //caster.SetInteractionFeedBack("wind", castDuration);
                    caster.UpdateSkillUI(0);
                    break;
            }
            return true;
        }
        return false;
    }
    public override void CancelSkillEffect(Human human)
    {
        if(lastCancelInvokeName!="")
        {
            human.CancelInvoke(lastCancelInvokeName);
            human.Invoke("TurnOffEffect", 0);
        }
        skillEffect = (Human) => { };
       
    }
    public override void UpdateCoolDown(List<ICooldown> list, Human caster)
    {
        caster.SkillCounter -= Time.deltaTime;
        if (caster.SkillCounter < 0)
        {
            caster.UpdateSkillUI(1);
            list.Remove(this);
        }
        else
        {
            caster.UpdateSkillUI(1 - (caster.SkillCounter / coolDown));
            caster.SkillEffectDuration -= Time.deltaTime;
            skillEffect(caster);
            
        }
    }
    void OnSkillEffect(Human caster)
    {
        if (caster.SkillEffectDuration > 0)
        {
            tickCounter -= Time.deltaTime;
            if (tickCounter < 0)
            {
                tickCounter = tickTime;
                if (afectType == Afects.allys || afectType == Afects.allysonly || afectType == Afects.everyone)
                {
                    for (int i = caster.allyList.Count - 1; i >= 0; i--)
                    {
                        if ((caster.transform.position - caster.allyList[i].transform.position).sqrMagnitude < sqrRadius)
                        {
                            if (!(afectType == Afects.allysonly && caster == caster.allyList[i]))
                                onUpdate(caster.allyList[i], caster);
                        }

                    }
                }
                if (afectType == Afects.enemys || afectType == Afects.everyone)
                {
                    for (int i = caster.enemyList.Count - 1; i >= 0; i--)
                    {
                        if ((caster.transform.position - caster.enemyList[i].transform.position).sqrMagnitude < sqrRadius)
                            onUpdate(caster.enemyList[i], caster);
                    }
                }
            }
        }
    }
    void Damage(Human victim, Human caster)
    {
         victim.ModifyLife(-damage);
    }
    void Wind(Human victim,Human caster)
    {
        State victimLastState = victim.GetCurrentState();
        if (victimLastState is Hit)
        {
            ((Hit)(victimLastState)).Reset();
        }
        if (victimLastState is WaitAnimation)
        {
            WaitAnimation wa2 = (WaitAnimation)(victim.GetCurrentState());
            wa2.SetTime(tickTime);
        }
        else
        { 
            victim.Rigidbody.velocity += caster.currentSkin.transform.forward * strength;
            victim.GetCurrentState().ChangeState(typeof(WaitAnimation));
            if (victim.GetCurrentState() is WaitAnimation)
            {
                WaitAnimation wa2 = (WaitAnimation)(victim.GetCurrentState());
                wa2.SetParameters(tickTime, victimLastState.GetType(), "");
            }
        }
    }
}