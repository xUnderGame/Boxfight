using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public void GoToMainMenu() { GameManager.Instance.ChangeScene("Main Menu"); }
}
