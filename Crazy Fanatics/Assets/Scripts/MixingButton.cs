using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixingButton : MonoBehaviour
{
    public ConsumibleItems.ConsumibleItemsEnum consumibleItem;
    private Button button;
    public AlchemyManager alchemyManager;
    private Text text;
    public Image iconImg;
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        
        text = GetComponentInChildren<Text>();
    }

    public void SetButtonText(int count) {
        text.text = count.ToString();
        button.interactable = count != 0;
       
    }
    public void OnClick() {
        alchemyManager.AddElement(this);
    }
}
