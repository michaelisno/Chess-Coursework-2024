using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyMenu : MonoBehaviour
{
    public void SaveDifficulty(int difficulty)
    {
        // Save difficulty setting with playerprefs and load singleplayer scene
        PlayerPrefs.SetInt("ai_difficulty", difficulty);
        SceneManager.LoadScene("Singleplayer");
    }
}
