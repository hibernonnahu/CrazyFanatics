using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsButton : MonoBehaviour {
    public GameObject stats;
    //private MainCharacter mainCharacter;
    // Use this for initialization
    public Image shine;
    public Text availablePoints;
    public Text leadershipPoints;
    public Text strengthPoints;
    public Text agilityPoints;
    public Text charmPoints;
    public Text maxAllyPoints;
    public Text buttonText;
    void Start () {
        //mainCharacter = FindObjectOfType<MainCharacter>();
        stats.SetActive(false);
        UpdateButtonText();
    }
	public void OnClick()
    {
        stats.SetActive(!stats.active);
        
    }
    public void UpdatePointsText()
    {
        availablePoints.text = "Skill Points: " + RamCharacterAttributes.GetInstance().AvailablePoints.ToString();
        leadershipPoints.text = "Leadership Points: " + RamCharacterAttributes.GetInstance().LeadershipPoints.ToString();
        strengthPoints.text = "Strength: " + RamCharacterAttributes.GetInstance().StrengthPoint.ToString();
        agilityPoints.text = "Agility: " + RamCharacterAttributes.GetInstance().AgilityPoint.ToString();
       
        maxAllyPoints.text = "Max Allys " + RamCharacterAttributes.GetInstance().MaxAllyPoint.ToString();
        UpdateButtonText();
    }
	// Update is called once per frame
	void Update () {
		if(buttonText.text != "Stats")
        {
            if(Random.Range(0,1f)<0.5f)
            {
                shine.color = Color.cyan;
            }
            else
            {
                shine.color = Color.blue;
            }
            
        }
	}
    private void UpdateButtonText()
    {
        if(RamCharacterAttributes.GetInstance().AvailablePoints+ RamCharacterAttributes.GetInstance().LeadershipPoints > 0)
        {
            buttonText.text = (RamCharacterAttributes.GetInstance().AvailablePoints + RamCharacterAttributes.GetInstance().LeadershipPoints).ToString();
            shine.gameObject.SetActive(true);
        }
        else
        {
            buttonText.text = "Stats";
            shine.gameObject.SetActive(false);
        }
            
    }
}
