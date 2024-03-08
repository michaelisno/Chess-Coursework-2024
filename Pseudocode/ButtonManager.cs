class ButtonManager
    public procedure LoadScene(sceneName)
        SceneManager.LoadScene(sceneName)
    endprocedure

    public procedure Quit()
        Application.Quit()
    endprocedure

    public procedure Withdraw(
        GameManager().whiteTimerText.SetText("0s")
    endprocedure
endclass


