using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject titleScreen;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        titleScreen.SetActive(false);
    }





}
