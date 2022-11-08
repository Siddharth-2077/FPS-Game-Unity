
using UnityEngine;

public class SettingsMenu : MonoBehaviour {

    public bool gamePaused;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            gamePaused = !gamePaused;
            PauseGame();
        }
    }

    private void PauseGame() {
        if (gamePaused) {
            Time.timeScale = 0f;

        } else {
            Time.timeScale = 1f;
        }
    }

}
