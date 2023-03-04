using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skinner : MonoBehaviour {
    public Transform headNode;
    public Transform backNode;
    public Transform leftHandNode;
    public Transform rightHandNode;
    public Transform leftFootNode;
    public Transform rightFootNode;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    //******************************
    public GameObject[] headAssets;
    public GameObject[] backAssets;
    public GameObject[] leftHandAssets;
    public GameObject[] rightHandAssets;
    public Material[] bodyAssets;
    public GameObject[] leftFootAssets;
    public GameObject[] rightFootAssets;
    //+++++++++++++++++++++++++++
    private int indexHead = -1;
    public int IndexHead
    {
        get
        {
            return indexHead;
        }
    }
    private int indexHands = -1;
    public int IndexHands
    {
        get
        {
            return indexHands;
        }
    }
    private int indexBody = -1;
    public int IndexBody
    {
        get
        {
            return indexBody;
        }
    }
    private int indexBack = -1;
    public int IndexBack
    {
        get
        {
            return indexBack;
        }
    }
    private int indexFoots = -1;
    public int IndexFoots
    {
        get
        {
            return indexFoots;
        }
    }
    // Use this for initialization
    void Start () {
        
	}
    private int code = -1;
    public void LoadNumberData(int code)
    {
        Debug.Log("LoadNumberData "+code);
        this.code = code;
        SetHead(PlayerPrefs.GetInt("headnumber"+code.ToString(), -1));
        SetBack(PlayerPrefs.GetInt("backnumber" + code.ToString(), -1));
        SetBody(PlayerPrefs.GetInt("bodynumber" + code.ToString(), 0));
        SetHands(PlayerPrefs.GetInt("handsnumber" + code.ToString(), -1));
        SetFoots(PlayerPrefs.GetInt("footsnumber" + code.ToString(), -1));
    }
    public void SaveData(int code=-1)
    {
        string c = "";
        if (code != -1)
        {
            c = code.ToString();
        }
        Debug.Log("save data");
        Debug.Log("headnumber" + c+" "+ IndexHead);

        PlayerPrefs.SetInt("headnumber"+c  , IndexHead);
        PlayerPrefs.SetInt("backnumber" + c  ,indexBack);
        PlayerPrefs.SetInt("bodynumber" + c  , IndexBody);
        PlayerPrefs.SetInt("handsnumber" + c ,  indexHands);
        PlayerPrefs.SetInt("footsnumber" + c , IndexFoots);
    }
    public void LoadLocalData()
    {
        Debug.Log("LoadLocalData");

        if (this.code == -1)
        {
            SetHead(PlayerPrefs.GetInt("headnumber", -1));
            SetBack(PlayerPrefs.GetInt("backnumber", -1));
            SetBody(PlayerPrefs.GetInt("bodynumber", 0));
            SetHands(PlayerPrefs.GetInt("handsnumber", -1));
            SetFoots(PlayerPrefs.GetInt("footsnumber", -1));
        }
    }
    public void LoadAssets(int head,int back,int body,int hands,int foots)
    {
        SetHead(head);
        SetBack(back);
        SetBody(body);
        SetHands(hands);
        SetFoots(foots);
    }
    // Update is called once per frame
    void Update () {
		
	}
    //head
    public void HeadAdd(bool all)
    {
        
            do
            {
                indexHead++;
                if (indexHead >= headAssets.Length)
                {
                    indexHead = -1;
                }
                
            }
            while (indexHead != -1 && !ItemHandler.GetInstance().IsAvailable(headAssets[indexHead].gameObject.name)&&!all);
        
        SetHead(indexHead);
    }
    public void HeadLess(bool all)
    {
        do { 
            indexHead--;
            if (indexHead <= -2)
            {
                indexHead = headAssets.Length-1;
            }
        }
        while (indexHead != -1 && !ItemHandler.GetInstance().IsAvailable(headAssets[indexHead].gameObject.name) && !all);
        SetHead(indexHead);
    }
    public void SetHead(int index)
    {
        foreach (Transform child in headNode)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (index > -1)
        {
            Instantiate<GameObject>(headAssets[index], headNode);
        }
        indexHead = index;
    }
    //back
    public void BackAdd(bool all)
    {
        do { 
            indexBack++;
            if (indexBack >= backAssets.Length)
            {
                indexBack = -1;
            }
            
        }
        while (indexBack != -1&&!ItemHandler.GetInstance().IsAvailable(backAssets[indexBack].gameObject.name) && !all);
        SetBack(indexBack);
    }
    public void BackLess(bool all)
    {
        do { 
            indexBack--;
            if (indexBack <= -2)
            {
                indexBack = backAssets.Length - 1;
            }
        }
        while (indexBack != -1 && !ItemHandler.GetInstance().IsAvailable(backAssets[indexBack].gameObject.name) && !all);
        SetBack(indexBack);
    }
    public void SetBack(int index)
    {
        foreach (Transform child in backNode)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (index > -1&& backAssets[index]!=null)
        {
            Instantiate<GameObject>(backAssets[index], backNode);
        }
        indexBack = index;

    }
    //hands
    public void HandsAdd(bool all)
    {
        do { 
            indexHands++;
            if (indexHands >= leftHandAssets.Length)
            {
                indexHands = -1;
            }
            
        }
        while (indexHands != -1 && !ItemHandler.GetInstance().IsAvailable(rightHandAssets[indexHands].gameObject.name) && !all);
        SetHands(indexHands);
    }
    public void HandsLess(bool all)
    {
        do { 
            indexHands--;
            if (indexHands <= -2)
            {
                indexHands = leftHandAssets.Length - 1;
            }
        }
        while (indexHands != -1 && !ItemHandler.GetInstance().IsAvailable(rightHandAssets[indexHands].gameObject.name) && !all);
        SetHands(indexHands);
    }
    public void SetHands(int index)
    {
        foreach (Transform child in leftHandNode)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (index > -1&& leftHandAssets[index]!=null)
        {
            Instantiate<GameObject>(leftHandAssets[index], leftHandNode);
        }

        foreach (Transform child in rightHandNode)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (index > -1)
        {
            Instantiate<GameObject>(rightHandAssets[index], rightHandNode);
        }
        indexHands = index;

    }
    //body
    public void BodyAdd(bool all)
    {
        do { 
            indexBody++;
            if (indexBody >= bodyAssets.Length)
            {
                indexBody = 0;
            }
            else if (indexBody == 0)
            {
                indexBody = 1;
            }
        }
        while ( !ItemHandler.GetInstance().IsAvailable(bodyAssets[IndexBody].name) && !all);
        SetBody(indexBody);
    }
    public void BodyLess(bool all)
    {
        do { 
            indexBody--;
            if (indexBody <= -1)
            {
                indexBody = bodyAssets.Length - 1;
            }
        }
        while (IndexBody != -1 && !ItemHandler.GetInstance().IsAvailable(bodyAssets[IndexBody].name) && !all);
        SetBody(indexBody);
    }
    public void SetBody(int index)
    {
        Debug.Log("SetBody" + index);
        if (index > -1)
        {
            //Instantiate<GameObject>(bodyAssets[index], bodyNode);
            skinnedMeshRenderer.material = bodyAssets[index];
        }
        indexBody = index;

    }
    //foots
    public void FootsAdd(bool all)
    {
        do { 
            indexFoots++;
            if (indexFoots >= leftFootAssets.Length)
            {
                indexFoots = -1;
            }
            else if (indexBody == 0)
            {
                indexBody = 1;
            }
        }
        while (indexFoots != -1 && !ItemHandler.GetInstance().IsAvailable(leftFootAssets[indexFoots].gameObject.name) && !all);
        SetFoots(indexFoots);
    }
    public void FootsLess(bool all)
    {
        do {
            indexFoots--;
            if (indexFoots <= -2)
            {
                indexFoots = leftFootAssets.Length - 1;
            }
        }
        while (indexFoots != -1 && !ItemHandler.GetInstance().IsAvailable(leftFootAssets[indexFoots].gameObject.name) && !all);
        SetFoots(indexFoots);
    }
    public void SetFoots(int index)
    {
        foreach (Transform child in leftFootNode)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (index > -1&& leftFootAssets[index]!=null)
        {
            Instantiate<GameObject>(leftFootAssets[index], leftFootNode);
        }
        foreach (Transform child in rightFootNode)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (index > -1)
        {
            Instantiate<GameObject>(rightFootAssets[index], rightFootNode);
        }
        indexFoots = index;

    }
}
