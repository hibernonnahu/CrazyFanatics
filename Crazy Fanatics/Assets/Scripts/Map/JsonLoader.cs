using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonLoader : MonoBehaviour {
    
    
	// Use this for initialization
	void Start () {
        string fileName=  RamCharacterAttributes.GetInstance().currentLevel.ToString();
        TextAsset ta = Resources.Load("level/level-" + fileName) as TextAsset;
       // Debug.Log(ta);// as TextAsset;
        //string path = Application.streamingAssetsPath + "/level-" + fileName+".json";
        //string tempString = File.ReadAllText(path);
       // Debug.Log("----------------");
        //Debug.Log(tempString);
        ParseString(ta.text);
	}
	
	virtual protected void ParseString(string tempString)
    {

    }
}
