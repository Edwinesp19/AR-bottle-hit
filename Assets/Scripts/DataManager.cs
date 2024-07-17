using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private ItemButtonManager itemButtonManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnItemsMenu += CreateButtons;
    }

    private void CreateButtons()
    {
        foreach (var item in items)
        {
            ItemButtonManager itemButton;
            itemButton= Instantiate(itemButtonManager, buttonContainer.transform);
            itemButton.item3DModel = item.Item3DModel; 
        }

        GameManager.instance.OnItemsMenu -= CreateButtons;
    }
}
