using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LvlXp   {
    public static int GetXPToLvl(int lvl)
    {
        int sum = 6;
        int counter = 1;
        while(counter<=lvl)
        {
            sum += (int)(sum * 0.45f);
            counter++;
        }
        return sum;
    }

}
