using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new consumible")]
public class ItemConsumible : Skill
{
    public ConsumibleItems.ConsumibleItemsEnum type;
    public bool autoCasting = false;
    public int charm = 0;
    public int heal = 0;
    //public void Add(MainCharacter human,int x) {
    //    human.ConsumibleItems[(int)item] += x;
    //}
    public override bool OnExecute(ITargeteable victim, Human human)
    {
        base.OnExecute(victim,human);
        if (particleEffectOnTarget != "")
        {
            Pool p2 = GameManager.Instance.GetPoolDictionary()[particleEffectOnTarget];
            ParticleSystem particle = p2.GetElement(victim.GetHuman().transform.position+Vector3.up* victim.GetHuman().stats.height*0.5f, true, particleTime).GetComponentInChildren<ParticleSystem>();
            particle.Stop();
            particle.Play();

            //human.Target.GetHuman().SetInteractionFeedBack(particleEffectOnTarget);
        }
        human.ItemConsumibleCounter = coolDown;
        human.SetAnimation(animation, 0, 1);
        GameManager.Instance.MainCharacter.ConsumibleItems[(int)type]--;
        GameManager.Instance.MainCharacter.UpdateConsumibleItemCount(GameManager.Instance.MainCharacter.ConsumibleItems[(int)type]);
        GameManager.Instance.MainCharacter.ChangeState(typeof(NullState));
        GameManager.Instance.MainCharacter.Rigidbody.velocity = Vector3.zero;
        GameManager.Instance.MainCharacter.ChangeState(typeof(WaitAnimation));
        WaitAnimation wa = (WaitAnimation)(GameManager.Instance.MainCharacter.GetCurrentState());
        GameManager.Instance.MainCharacter.UpdateItemConsumibleUI(0);
        wa.SetParameters(GameManager.Instance.MainCharacter.itemConsumible.castDuration, typeof(Idle), "");
        victim.GetHuman().Convert(victim.GetHuman().stats, GameManager.Instance.MainCharacter);
        victim.GetHuman().Rigidbody.velocity = Vector3.zero;
        GameManager.Instance.MainCharacter.Target = null;
        return true;
    }
    public override bool IsAvailable(Human caster)
    {
        return (caster.ItemConsumibleCounter <= 0);
    }
    public override void Init(Human caster)
    {
        caster.ItemConsumibleCounter = 0;
    }
    public override void UpdateCoolDown(List<ICooldown> list, Human caster)
    {
        caster.ItemConsumibleCounter -= Time.deltaTime;
        if (caster.ItemConsumibleCounter < 0)
        {
            GameManager.Instance.MainCharacter.UpdateItemConsumibleUI(1);
            list.Remove(this);
        }
        else
        {
            GameManager.Instance.MainCharacter.UpdateItemConsumibleUI(1 - (caster.ItemConsumibleCounter / coolDown));
        }
    }

}
