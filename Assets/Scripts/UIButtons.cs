using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void GoToMainMenu() { ChangeScene("Main Menu"); }

    // Resets the scene (basically a respawn)
    public void PlayerRespawn() {
        Time.timeScale = 1;
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Changes to a different scene.
    public void ChangeScene(string sceneName) { SceneManager.LoadScene(sceneName); }
}
