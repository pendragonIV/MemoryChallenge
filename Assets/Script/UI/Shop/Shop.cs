using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    [SerializeField]
    private Transform popupPanel;

    [SerializeField]
    private Transform characterContainer;
    [SerializeField]
    private Transform characterPrefab;

    [SerializeField]
    public PlayerData playerData;

    [SerializeField]
    private Text coinText;

    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color disabledColor;


    private void Awake()
    {
        HidePopup();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        ShopInitialize();
        SetCoin();
    }

    public void ShopInitialize()
    {
        foreach(Transform child in characterContainer)
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        playerData.GetAnimals().ForEach(animal =>
        {
            Transform character = Instantiate(characterPrefab, characterContainer);
            character.name = index++.ToString();
            //Set data for holder
            SetCharacterShop(character, animal);
        });
    }
    
    private void SetCharacterShop(Transform character, Animal animalData)
    {
        Transform characterHolder = character.GetChild(0);

        Transform tick = characterHolder.GetChild(0);
        Image characterImg = characterHolder.GetChild(2).GetComponent<Image>();
        Text characterPrice = characterHolder.GetChild(3).GetComponent<Text>();

        Transform optionButton = character.GetChild(1);
        Text optionText = optionButton.GetChild(0).GetComponent<Text>();

        TickIfCharacterUsing(animalData, tick);
        SetCharacterOption(animalData, characterPrice, optionButton, optionText);
        SetCharacterImage(animalData, characterImg);
    }

    private void SetCharacterImage(Animal animalData, Image characterImage)
    {
        characterImage.sprite = animalData.animalImage;
    }

    private void TickIfCharacterUsing(Animal animalData, Transform tick)
    {

        if (animalData.isUsing)
        {
            tick.gameObject.SetActive(true);
        }
        else
        {
            tick.gameObject.SetActive(false);
        }
    }

    private void SetCharacterOption(Animal animalData, Text characterPrice, Transform optionButton, Text optionText)
    {

        if (animalData.isBought)
        {
            characterPrice.gameObject.SetActive(false);
            optionButton.GetComponent<Image>().color = defaultColor;
            optionText.text = "USE";
        }
        else
        {
            characterPrice.text = animalData.price.ToString();
            optionText.text = "BUY";
            
            if(playerData.GetGold() < animalData.price)
            {
                optionButton.GetComponent<Button>().interactable = false;
                optionButton.GetComponent<Image>().color = disabledColor;
            }
            else
            {
                optionButton.GetComponent<Button>().interactable = true;
                optionButton.GetComponent<Image>().color = defaultColor;
            }
        }

    }

    public void SetCoin()
    {
        coinText.text = playerData.GetGold().ToString();
    }

    public void Buy(int index)
    {
        playerData.SetGold(playerData.GetGold() - playerData.GetAnimalAt(index).price);
        playerData.SetAnimalStatus(index, true, false);
    }

    public void ShowPopup(int index)
    {
        popupPanel.gameObject.SetActive(true);

        Image characterImg = popupPanel.GetChild(0).GetChild(0).GetComponent<Image>();

        characterImg.sprite = playerData.GetAnimalAt(index).animalImage;
    }

    public void HidePopup()
    {
        popupPanel.gameObject.SetActive(false);
    }
}
