using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject, ICooldown
{
    public int soundCode = -1;
    public Sprite icon;
    public string text="";
    public string animation= "";
    public float castDuration = 0;
    public float coolDown = 1;
    [Header("OnTargetSelect")]
    public float castDistanse = 1;
    public string particleEffectOnTarget = "";
    public float particleTime = 2;
    public enum Afects
    {
        allys,
        allysonly,
        enemys,
        everyone,
        self
    }
    [SerializeField]
    public Afects afectType;
    public virtual bool OnExecute(ITargeteable victim, Human caster)
    {
        if (soundCode != -1) {
            FxSoundManager.Instance.PlayFx(soundCode);

        }
        return false;
    }
    public virtual bool IsAvailable(Human caster)
    {
        return (caster.SkillCounter <= 0);
    }
    public virtual void Init(Human caster)
    {
        caster.SkillCounter = 0;
    }
    public virtual void UpdateCoolDown(List<ICooldown> list, Human caster)
    {
    }
    public virtual void CancelSkillEffect(Human human)
    {
       
    }
    public virtual bool IACastCondition(Human human) { return false; }
}
