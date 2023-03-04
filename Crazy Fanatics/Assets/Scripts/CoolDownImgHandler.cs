using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CoolDownImgHandler : MonoBehaviour
{
    public GameObject container;
    public Image background;
    public Image item;
    public Image cooldownImage;
    public Button button;
    [Range(0, 1)]
    public float cooldown;
    public Animator readyAnimator;
    private bool ready = true;
    // Start is called before the first frame update
    
    public void UpdateCooldown(float c)//0==ready
    {
        Debug.Log(c +" ready"+ready);
        if (ready && c > 0)
        {
            button.interactable = false;
            if (c > 1)
            {
                c = 1;
            }
            ready = false;
            
        }
        else if (!ready && c > 0)
        {
          
        }
        else if (!ready && c <= 0) {
            TurnOn(true);
            c = 0;
        }
        cooldownImage.fillAmount = c;
    }

    private void ShowReady()
    {
        readyAnimator.CrossFade("ready", 0);
    }

    internal void TurnOn(bool b)
    {
        container.SetActive(b);
        if (b) {
            cooldownImage.fillAmount = 0;
            ready = true;
            
            button.interactable = true;
            ShowReady();
        }
    }
}
