using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balanced : CharacterClass {

	public Balanced(MainCharacter mc):base(mc)
    {

    }
    public override void OnLevelUp()
    {
        mainCharacter.AddAgility(true);
        mainCharacter.AddStrength(true);
       
        mainCharacter.AddMaxAlly(true);


    }
}
