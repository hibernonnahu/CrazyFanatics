using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RamCharacterAttributes  {
    private static RamCharacterAttributes Instance;
    public int currentLevel =-2;
    public int gold = 0;
    private int leadershipPoints = 0;
    private int availablePoints = 0;
    public int strengthPoint = 0;
    public int agilityPoint = 0;
    
    public int maxAllyPoint = 1;
    public int playerLevel = 1;
    public int currentXP = 0;
    public List<int> allys = new List<int>{0};
    public int[] consumibleItems = new int[Enum.GetValues(typeof(ConsumibleItems.ConsumibleItemsEnum)).Length];
    public Skill skill;
    public ItemConsumible itemConsumible;
    public InventoryItem rightHand;
    public InventoryItem leftHand;
    public InventoryItem Neck;

    public RamCharacterAttributes ()
    {
        rightHand = GameManager.Instance.MainCharacter.rightHand;
        skill = GameManager.Instance.MainCharacter.skill;
        itemConsumible = GameManager.Instance.MainCharacter.itemConsumible;
        consumibleItems[1] = 1;
        //for (int i = 0; i < consumibleItems.Length; i++)
        //{
        //    consumibleItems[i] = 10;
        //}
    }
    public int AvailablePoints
    {
        get
        {
            return availablePoints;
        }
        set
        {
            availablePoints = value;
        }
    }
    public int LeadershipPoints
    {
        get
        {
            return leadershipPoints;
        }
        set
        {
            leadershipPoints = value;
        }
    }

    public int StrengthPoint
    {
        get
        {
            return strengthPoint;
        }
    }
    
    public int AgilityPoint
    {
        get
        {
            return agilityPoint;
        }
    }
    
    
    public int MaxAllyPoint
    {
        get
        {
            return maxAllyPoint;
        }
    }
    // Use this for initialization
    public static RamCharacterAttributes GetInstance()
    {
        if(Instance==null)
        {
            Instance = new RamCharacterAttributes();
        }
       
            return Instance;
        
    }

}
