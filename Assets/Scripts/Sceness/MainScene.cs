using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public void OnClickBattle()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}
