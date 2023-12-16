using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameManager : MonoBehaviour
{
    [SerializeField] GameObject CreditBoard;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitTheApplication()
    {
        Application.Quit();
    }
    public void Credit()
    {
        CreditBoard.SetActive(true);
    }
    public void CreditBack()
    {
        CreditBoard.SetActive(false);
    }
}
