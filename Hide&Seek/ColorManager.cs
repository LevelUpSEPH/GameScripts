using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    public void SetPlayerColor(Colors.Color color)
    {
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.SetPlayerColor(color);
    }
}
