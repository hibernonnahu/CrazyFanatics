using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyManager : MonoBehaviour
{
    public RectTransform otherCanvas;
    private MixingButton[] mixingButtons;
    private int[] ramItems = new int[Enum.GetValues(typeof(ConsumibleItems.ConsumibleItemsEnum)).Length];
   
    private InputHandler cameraInputHandler;

    public Image[] recipeIcons;
    // Start is called before the first frame update
    void Start()
    {
        if (mixingButtons == null)
        {
            mixingButtons = GetComponentsInChildren<MixingButton>();
        }
        ResetButtons();
    }
    private void MoveCamera() {

    }
    // Update is called once per frame
    void Update()
    {
       
    }
    public void AddElement(MixingButton mixingButton) {
        int count = GetRamItemsCount();
        if (count < 4)
        {
            FxSoundManager.Instance.PlayFx(2);

            recipeIcons[count].gameObject.SetActive(true);
            recipeIcons[count].sprite = mixingButton.iconImg.sprite;
            var main = GameManager.Instance.MainCharacter;
            int id = (int)mixingButton.consumibleItem;
            ramItems[id]++;
            mixingButton.SetButtonText(main.ConsumibleItems[id] - ramItems[id]);
            main.SetAnimation("throw", 0.05f, 1.3f);

            main.SetParticleAmount(main.particlesAlchemy[id], ramItems[id]);
            if (count == 3) {
                Invoke("TryToMix", 1);
            }
        }
    }
    private int GetRamItemsCount() {
        int count = 0;
        foreach (var item in ramItems)
        {
            count += item;
        }
        return count;
    }
    private void OnEnable()
    {
            FxSoundManager.Instance.PlayFx(5);
        FxSoundManager.Instance.PlayFx(7);

        Camera.main.GetComponent<InputHandler>().GoToOffset(-1.12f,-1.8f,-2.9f);
        
        MainCharacter main = GameManager.Instance.MainCharacter;
        main.ChangeState(typeof(NullState));
        main.LookAt(main.transform.position - Vector3.forward+Vector3.right*0.7f);
        DelayAnimation();
        main.cauldrin.SetActive(true);
        Invoke("DelayAnimation" , 1);
        main.Rigidbody.velocity = Vector3.zero;
        otherCanvas.localScale = Vector3.zero;
        main.Weapons.SetActive(false);
        if(mixingButtons!=null)
        ResetButtons();
        main.NullAllys(true);
        GameManager.Instance.MainCharacter.claudrinNewObjectParticles[0].Play();
    }
    private void DelayAnimation() {
        GameManager.Instance.MainCharacter.SetAnimation("alchemy", 0.1f, 1);
    }
    private void OnDisable()
    {
        OnCancel();
        MainCharacter main = GameManager.Instance.MainCharacter;
        main.ChangeState(typeof(Idle));
        main.cauldrin.SetActive(false);
       

        Camera.main.GetComponent<InputHandler>().GoToOffset(0, 0,-2.2f);
        otherCanvas.localScale = Vector3.one;
        main.Weapons.SetActive(true);
        main.NullAllys(false);

    }
    public void TryToMix() {
        RecipeManager.IdAndCount result = RecipeManager.CheckRecipe(ramItems);
        var main = GameManager.Instance.MainCharacter;
        if(result.id!=-1)
        {
            main.claudrinNewObjectParticles[result.id].Play();
            Debug.Log("create");
            OnMix(result.id, result.count);
            WinningManager.Instance.CheckEnemyKill(-result.id);
            FxSoundManager.Instance.PlayFx(4);
        }
        else
        {
            Debug.Log("wrong");
            

            OnCancel();
        }
    }
    private void OnMix(int id,int count) {
        var main = GameManager.Instance.MainCharacter;
        //TODO show result
        for (int i = 0; i < ramItems.Length; i++)
        {

            main.ConsumibleItems[i]-=ramItems[i];
            ramItems[i] = 0;
        }
        foreach (var item in main.particlesAlchemy)
        {
            if (item != null)
                main.SetParticleAmount(item, 0);
        }
        foreach (var item in recipeIcons)
        {
            item.gameObject.SetActive(false);
        }
        main.ConsumibleItems[id] += count;
        if ((int)main.itemConsumible.type == id) {
            main.UpdateConsumibleItemCount(main.ConsumibleItems[id]);
        }
        main.SetAnimation("mix", 0.05f, 1.3f);
    }
    public void OnCancel() {
        //TODO shot cross ui
        GameManager.Instance.MainCharacter.claudrinNewObjectParticles[0].Play();
        ResetButtons();
        FxSoundManager.Instance.PlayFx(7);

    }
    private void ResetButtons(){
        var main = GameManager.Instance.MainCharacter;
        for (int i = 0; i < ramItems.Length; i++)
        {
            ramItems[i] = 0;
        }
        foreach (var button in mixingButtons)
        {
            int id = (int)button.consumibleItem;
            button.SetButtonText(main.ConsumibleItems[id]);
        }
        main.SetAnimation("taunt", 0, 1);
        foreach (var item in main.particlesAlchemy)
        {
            if(item!=null)
            main.SetParticleAmount(item, 0);
        }
        foreach (var item in recipeIcons)
        {
            item.gameObject.SetActive(false);
        }
    }

}
