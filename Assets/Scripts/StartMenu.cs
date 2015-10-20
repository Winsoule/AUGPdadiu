using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UnitManager))]
public class StartMenu : MonoBehaviour
{
    UnitManager manager;

    void Start()
    {
        manager = GetComponent<UnitManager>();
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

}
