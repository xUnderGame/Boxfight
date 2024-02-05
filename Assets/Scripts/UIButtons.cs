using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void GoToMainMenu() { ChangeScene("Menu"); }

    // Resets the scene (basically a respawn)
    public void PlayerRespawn() {
        Time.timeScale = 1;
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Changes to a different scene.
    public void ChangeScene(string sceneName) {
        if(sceneName != "Game" && sceneName != "Tutorial") Destroy(GameManager.Instance.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void Exit() { Application.Quit(); }
}
