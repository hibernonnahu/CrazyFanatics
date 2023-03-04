using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyAssetWithMovement : MonoBehaviour {

    // Use this for initialization
    public Animator[] animator;
    //public Runner runner;
    private Action onUpdate = () => { };
	public void LoadAnimators () {
         List<Animator> animatorList;
        animatorList = new List<Animator>();
        Animator localAnimator = GetComponent<Animator>();
        Animator[] aux = transform.GetComponentsInChildren<Animator>();
        foreach (var item in aux)
        {
            if (item != localAnimator)
            {
                animatorList.Add(item);
            }
        }

        animator = new Animator[animatorList.Count];
        animator = animatorList.ToArray();

        onUpdate = UpdateIdle;
        
	}
    private void Start()
    {
        //runner = GetComponentInParent<Runner>();


        //if (runner == null || runner.Rigidbody == null)
        //{
        //    onUpdate = UpdateFindRunner;
        //}
    }
    // Update is called once per frame
    void Update () {
        
        onUpdate();
	}
    void UpdateFindRunner()
    {
        //if (runner == null)
        //{
        //    runner = GetComponentInParent<Runner>();
        //}
        //else if(runner.Rigidbody!=null)
        //{
        //    onUpdate = UpdateIdle;
        //}
    }
    void UpdateIdle()
    {
        //if (!CheckFalling())
        //{
        //    if (Mathf.Abs( runner.Speed) > 1)
        //    {
        //        foreach (var item in animator)
        //        {
        //            item.CrossFade("speed1", 1);

        //        }
        //        onUpdate = UpdateSpeed1;
               
        //    }
        //}
    }
    void UpdateSpeed1()
    {
        //if (!CheckFalling())
        //{
        //    if (Mathf.Abs(runner.currentspeed) > 10)
        //    {
        //        foreach (var item in animator)
        //        {
        //            item.CrossFade("speed2", 1);
        //        }
        //        onUpdate = UpdateSpeed2;

        //    }
            
        //}
    }
    void UpdateSpeed2()
    {
    //    if (!CheckFalling())
    //    {
    //         if (Mathf.Abs(runner.currentspeed) < 10)
    //        {
    //            foreach (var item in animator)
    //            {
    //                item.CrossFade("speed1", 1);
    //            }
    //            onUpdate = UpdateSpeed1;

    //        }
    //    }
    //}
    //void UpdateFalling()
    //{
    //    if (runner.Rigidbody.velocity.y > -1)
    //    {
    //        foreach (var item in animator)
    //        {
    //            item.CrossFade("speed1", 0.5f);
    //        }
    //        onUpdate = UpdateSpeed1;
            
    //    }
    }
    bool CheckFalling()
    {
        //if(runner.Rigidbody!=null&& runner.Rigidbody.velocity.y<-1)
        //{
        //    foreach (var item in animator)
        //    {
        //        item.CrossFade("falling", 1);
        //    }
        //    onUpdate = UpdateFalling;
        //    return true;
        //}
        return false;
    }
    public void SetIdle()
    {
        onUpdate = UpdateIdle;
        foreach (var item in animator)
        {
            item.CrossFade("idle", 0);
        }
    }
}
