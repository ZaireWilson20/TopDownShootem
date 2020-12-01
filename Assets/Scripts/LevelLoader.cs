using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  public Animator transtion;
  public void PlayGame()
  {
    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
  }

  public void QuitGame()
  {
    Application.Quit();
  }

  IEnumerator LoadLevel(int levelIndex)
  {
    transtion.SetTrigger("Start");
    yield return new WaitForSeconds(1);
    SceneManager.LoadScene(levelIndex);
  }
}
