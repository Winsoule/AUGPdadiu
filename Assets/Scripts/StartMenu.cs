using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;

[RequireComponent (typeof(UnitManager))]
public class StartMenu : MonoBehaviour
{
    UnitManager manager;
    public InputDevice controller;
    public Transform button;

    void Start()
    {
        manager = GetComponent<UnitManager>();
        controller = InputManager.ActiveDevice;
    }

    public void StartGame()
    {
        manager.NewSave();
        Application.LoadLevel("Jensarea");
    }

    public void MainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    void Update()
    {
        print(controller.LeftStickX);
        if (controller.Action1.WasPressed)
        {
            print("lol!");
            button.GetComponentInChildren<Button>().onClick.Invoke();
        }
    }

}
