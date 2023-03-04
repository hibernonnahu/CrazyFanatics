using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public struct IdAndCount{
        public int id;
        public int count;
    }
    private static List<List<int>> recipes = new List<List<int>>()
    {
        new List<int>(){ (int)ConsumibleItems.ConsumibleItemsEnum.alchemyRed,3,(int)ConsumibleItems.ConsumibleItemsEnum.alchemyWhite,1,(int)ConsumibleItems.ConsumibleItemsEnum.lovePotion1,5}//love potion 1
    };


    public static IdAndCount CheckRecipe(int[] ramList) {
        IdAndCount result = new IdAndCount();
        result.id = -1;

        foreach (var recipe in recipes)
        {
            var clone=CloneList(recipe);
            for (int i = 0; i < ramList.Length ; i++)
            {
                int foundId = -1;
                for (int j = 0; j < clone.Count-1; j+=2)
                {
                    if (clone[j] == i && clone[j + 1] == ramList[i]) {
                        foundId = j;
                    }
                }
                if (foundId != -1) {
                    clone.RemoveRange(foundId, 2);
                }
            }
            if (clone.Count == 0) {
                //recipe found
                result.id = recipe[recipe.Count - 2];
                result.count = recipe[recipe.Count - 1];
                break;
            }
        }


        return result;
    }

    private static List<int> CloneList(List<int> recipe)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < recipe.Count-2; i++)
        {
            list.Add(recipe[i]);
        }
        return list;
    }
}
