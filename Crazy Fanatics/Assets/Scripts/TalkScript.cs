using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class TalkScript : MonoBehaviour
{
    public GameObject[] gameObjects;
    public string[] commands;
    public float timeToContinue = 2;
    private float counter = 0;
    private int countLine=0;
    public  TextMeshProUGUI text;
    public Action onUpdate = () => { };
    public string nextSceneName = "LevelGenerator";
    public void Continue() {

    }
    public void ParseLine(int index) {
        if (countLine == commands.Length)
        {
            foreach (var item in gameObjects)
            {
                item.SetActive(false);
            }
            text.text = "";
            gameObjects[3].SetActive(true);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            string line = commands[index];
            text.text = "";
            foreach (var item in gameObjects)
            {
                item.SetActive(false);
            }
            
            if (line.Substring(0, 1) == "-")
            {
                gameObjects[int.Parse(line.Substring(1, line.Length - 1))].SetActive(true);
            }
            else
            {
                text.text = line;
                counter = timeToContinue;
                onUpdate = () =>
                {
                    counter -= Time.deltaTime;
                    if (counter < 0)
                    {
                        onUpdate = () => { };
                        gameObjects[0].SetActive(true);
                    }
                };
            }
            countLine++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ParseLine(countLine);
    }

    // Update is called once per frame
    void Update()
    {
        onUpdate();
        if (Input.GetMouseButtonDown(0) && gameObjects[0].activeInHierarchy) {
            OnClick();
        }
    }
    public void OnClick() {
        ParseLine(countLine);
    }
}
