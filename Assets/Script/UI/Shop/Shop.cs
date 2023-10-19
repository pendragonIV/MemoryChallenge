using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private Transform popupPanel;

    [SerializeField]
    private Transform characterContainer;
    [SerializeField]
    private Transform characterPrefab;

    [SerializeField]
    private PlayerData playerData;

    private void Awake()
    {
        HidePopup();
    }

    private void Start()
    {
        ShopInitialize();
    }

    private void ShopInitialize()
    {
        playerData.GetAnimals().ForEach(animal =>
        {
            Transform character = Instantiate(characterPrefab, characterContainer);
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
        SetCharacterOption(animalData, characterPrice, optionText);
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

    private void SetCharacterOption(Animal animalData, Text characterPrice, Text optionText)
    {

        if (animalData.isBought)
        {
            characterPrice.gameObject.SetActive(false);
            optionText.text = "USE";
        }
        else
        {
            characterPrice.text = animalData.price.ToString();
            optionText.text = "BUY";
        }

    }

    public void ShowPopup()
    {
        popupPanel.gameObject.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.gameObject.SetActive(false);
    }
}
