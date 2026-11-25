using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip clickSound;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject difficultyPanel;

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
        PlayClick();
        StartCoroutine(PlayPlayerVsPlayer());
    }

    public void OnPvCClicked()
    {
        PlayClick();
        mainPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void OnDifficultyClicked(int depth)
    {
        PlayClick();
        GameSettings.AiDepth = depth;
        StartCoroutine(PlayPlayerVsComputer());
    }

    public void OnBackClicked()
    {
        PlayClick();
        difficultyPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OnQuitClicked()
    {
        PlayClick();
        StartCoroutine(QuitGame());
    }

    private void PlayClick()
    {
        if (sfxSource != null && clickSound != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }
}
