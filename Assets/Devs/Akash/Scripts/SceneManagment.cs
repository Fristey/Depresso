using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
