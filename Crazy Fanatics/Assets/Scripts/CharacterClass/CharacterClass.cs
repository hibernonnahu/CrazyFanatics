using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass  {
    protected MainCharacter mainCharacter;
    public CharacterClass(MainCharacter mc)
    {
        mainCharacter = mc;
    }
	virtual public void OnLevelUp()
    {

    }
}
