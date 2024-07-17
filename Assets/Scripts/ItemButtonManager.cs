using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonManager : MonoBehaviour
{
    [SerializeField] public GameObject item3DModel;
    private ARInteractionManager arInteractionManager;


    // Start is called before the first frame update
    void Start()
    {
        //para manejar el evento de click en el boton y mostrar el modelo 3D
        var button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.instance.ARPosition);
        button.onClick.AddListener(Create3DModel);

        arInteractionManager = FindObjectOfType<ARInteractionManager>();
    }

    private void Create3DModel()
    {
    arInteractionManager.Item3DModel = Instantiate(item3DModel); 
    }


}
