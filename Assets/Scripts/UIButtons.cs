using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void GoToMainMenu() { ChangeScene("Main Menu"); }

    // Resets the scene (basically a respawn)
    public void PlayerRespawn() {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Changes to a different scene.
    public void ChangeScene(string sceneName) { SceneManager.LoadScene(sceneName); }
}
