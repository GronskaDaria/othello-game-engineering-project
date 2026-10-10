using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour

{
    [SerializeField] private AudioSource sfxSource; 
    [SerializeField] private AudioClip clickSound;

    public IEnumerator PlayPlayerVsPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PlayerVSPlayer");
    }

    public IEnumerator PlayPlayerVsComputer()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PlayerVSComp");
    }

    public IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void OnPvPClicked()
    {
        if (sfxSource!=null&&clickSound!=null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
        StartCoroutine(PlayPlayerVsPlayer());
    }

    public void OnPvCClicked()
    {
        if (sfxSource!=null&&clickSound!=null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
        StartCoroutine(PlayPlayerVsComputer());
    }

    public void OnQuitClicked()
    {
        if (sfxSource!=null&&clickSound!=null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
        StartCoroutine(QuitGame());
    }
}