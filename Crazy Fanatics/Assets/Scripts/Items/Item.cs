using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : PooleableObject,ITargeteable {
    public int soundCode = -1;
    public GameObject selectable;
    private float distToEngage = 1;
    public bool chaseCharacter=false;
    private float chaseSpeed= 1.4f;
    Action onUpdate = () => { };
    public int id = -1;
    public int hpAdd = 0;
    public int xpAdd = 0;
    public int strenghtAdd = 0;
    public int agilityAdd = 0;
    
    public int maxAllyAdd = 0;
    public int goldAdd = 0;
    private float distanceAdd = 0;
    private bool used = false;
    public bool canBeTrigger = true;
    public InventoryItem inventoryItem;
    public Skill skill;
    public bool addItemConsumible = false;
    public ConsumibleItems.ConsumibleItemsEnum itemConsumible;
    
    private Action onItem = () => { };
    private void Start()
    {
        if(chaseCharacter)
        {
            onUpdate = OnChase;
        }
        if(selectable!=null)
        {
            selectable.SetActive(false);
        }
    }
    public void Renew()
    {
        used = false;
        distToEngage = 0f;
        distanceAdd = 3.5f;
    }
    void OnChase()
    {
        Vector3 vec = (GameManager.Instance.MainCharacter.transform.position - transform.position);
        float dist = vec.sqrMagnitude;
        if(dist<distToEngage)
        {
            //Debug.Log((distToEngage - dist));
            float speed = (distToEngage - dist) * chaseSpeed;
            if(speed>3f)
            {
                speed = 3f;
            }
            transform.position += CustomMath.XZNormalize( vec) * speed * Time.deltaTime;
        }
        else if(distanceAdd>0)
        {
            distToEngage += Time.deltaTime * distanceAdd;
        }
    }
   
    virtual public void OnHumanGrab(Human human)
    {
        if(!used)
        {
            if (soundCode != -1)
            {
                FxSoundManager.Instance.PlayFx(soundCode);

            }
            used = true;
            human.Item = null;
            if(inventoryItem!=null)
            {
                human.LoadInventoryItem(inventoryItem);
                transform.parent= GameManager.Instance.vault;
            }
            if(skill!=null)
            {
                human.LoadSkill(skill);
                Pool p = GameManager.Instance.GetPoolDictionary()["text"];
                GameObject go = p.GetElement(transform.position + Vector3.up ,true,100,0.05f);
                TextMesh tm = go.GetComponent<TextMesh>();
               
                    tm.color = CustomMath.Vector4NotNew(0.35f,0.35f,0.35f,1);
                

                tm.text = skill.text;
            }
            if ( addItemConsumible)
            {
                //GameManager.Instance.MainCharacter.LoadItemConsumible(itemConsumible);
                GameManager.Instance.MainCharacter.ConsumibleItems[(int)itemConsumible]++;
                //Pool p = GameManager.Instance.GetPoolDictionary()["text"];
                //GameObject go = p.GetElement(transform.position + Vector3.up, true, 100, 0.05f);
                //TextMesh tm = go.GetComponent<TextMesh>();

                //tm.color = CustomMath.Vector4NotNew(0.35f, 0.35f, 0.35f, 1);


                //tm.text = skill.text;
            }
            if (hpAdd != 0)
            {
                human.ModifyLife(hpAdd);
            }
            if (xpAdd > 0)
            {
                human.ReceiveXP(xpAdd);
            }
            if (goldAdd > 0)
            {
                human.ReceiveGold(goldAdd);
            }
            if (strenghtAdd > 0)
            {
                MainCharacter mc = (MainCharacter)(human);
                mc.AddStrength(true);
            }
            if (agilityAdd > 0)
            {
                MainCharacter mc = (MainCharacter)(human);
                mc.AddAgility(true);
            }
           
            if (maxAllyAdd > 0)
            {
                MainCharacter mc = (MainCharacter)(human);
                mc.AddMaxAlly(true);
            }
            currentTime = -1;
            this.transform.position = Vector3.up * -10000;
            //Destroy(gameObject);
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(canBeTrigger&&other.gameObject== GameManager.Instance.MainCharacter.gameObject)
            OnHumanGrab(GameManager.Instance.MainCharacter);

    }
    private void Update()
    {
        onUpdate();
    }
    public Human GetHuman() { return null; }
    public void SetSelected(bool b) {
        if (!canBeTrigger)
        {
            selectable.SetActive(b);
        }
    }
    public GameObject GetTargetGameObject()
    {
        return gameObject;
    }
    public float GetRadius()
    {
        return 0f;
    }
    public Item GetItem() { return this; }
    public void Grab()
    {
        OnHumanGrab(GameManager.Instance.MainCharacter);
    }
}
