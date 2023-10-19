using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartInit : MonoBehaviour
{
    [SerializeField]
    private Transform cartContainer;
    [SerializeField]
    private GameObject cartPrefab;

    private float cartSpacing = 10f;

    public void InitializeCard(int cols, int rows)
    {
        SetCartSize();

        for (int i = 0; i < (cols * rows); i++)
        {
            GameObject cart = Instantiate(cartPrefab, cartContainer);
        }
    }

    private void SetCartSize()
    {
        GridLayoutGroup gridLayoutGroup = cartContainer.GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = cartContainer.GetComponent<RectTransform>();
        float width = rectTransform.rect.width;
        float cellWidth = (width - cartSpacing * (LevelManager.instance.currentLevelColumns + 1)) / LevelManager.instance.currentLevelColumns; //3->4 space, 4->5 space,... -> +1

        gridLayoutGroup.spacing = new Vector2(cartSpacing, cartSpacing);
        gridLayoutGroup.padding = new RectOffset(10, 10, 10, 10);

        gridLayoutGroup.cellSize = new Vector2(cellWidth,cellWidth);
    }
}
