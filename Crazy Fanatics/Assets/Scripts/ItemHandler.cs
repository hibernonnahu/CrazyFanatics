
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler  {
    private static ItemHandler instance;
    private Dictionary<string, string> collectableItems;
   

    public ItemHandler()
    {
        

    }
    
    public static ItemHandler GetInstance()
    {
        if (instance == null)
        {
            instance = new ItemHandler();
        }
        return instance;
    }
    public bool IsAvailable(string k)
    {
        
        return true ;
        
    }
}
