class DifficultyMenu
    public procedure SaveDifficulty(difficulty)
        PlayerPrefs.SetInt("ai_difficulty", difficulty)
        SceneManager.LoadScene("Singleplayer")
    endprocedure
endclass

