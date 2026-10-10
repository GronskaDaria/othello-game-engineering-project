using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerTurnText;

    [SerializeField]
    private TextMeshProUGUI blackScore;

    [SerializeField]
    private TextMeshProUGUI whiteScore;

    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private Image blackOverlay;

    [SerializeField]
    private RectTransform slider;

    [SerializeField]
    private RectTransform playAgainButton;

    private bool isVsComputer;

    public void SetGameMode(bool vsComputer)
    {
        isVsComputer=vsComputer;
    }

    private string GetPlayerName(Player player)
    {
        if (player==Player.White)
            return isVsComputer ? "Komputer" : "Gracz 1";

        return isVsComputer ? "Gracz" : "Gracz 2";
    }

    public void SetPlayerText(Player currentPlayer)
    {
        string icon = (currentPlayer==Player.White) ? "<sprite name=DiscWhiteUp>" : "<sprite name=DiscBlackUp>";
        playerTurnText.text=$"{icon} {GetPlayerName(currentPlayer)}";
    }

    public void SetSkippedText(Player skippedPlayer)
    {
        string icon = (skippedPlayer==Player.White) ? "<sprite name=DiscWhiteUp>" : "<sprite name=DiscBlackUp>";
        playerTurnText.text=$"{icon} {GetPlayerName(skippedPlayer)} Brak Ruchu";
    }

    public void SetWhiteScore(int score)
    {
      whiteScore.text=score.ToString();
    }

    public void SetBlackScore(int score)
    {
        blackScore.text=score.ToString();
    }

    public void ShowEndGameUI()
    {
        blackOverlay.gameObject.SetActive(true);
        winnerText.gameObject.SetActive(true);
        winnerText.rectTransform.LeanMoveLocalY(0f, 0.5f);
        slider.gameObject.SetActive(false);
        new WaitForSeconds(0.5f);

       
        playAgainButton.gameObject.SetActive(true);
    }

    public void SetWinnerText(Player winner, int whiteScoreCount, int blackScoreCount)
    {
        string p1 = isVsComputer ? "Komputer" : "Gracz 1";
        string p2 = isVsComputer ? "Gracz" : "Gracz 2";

        string scoreText = $"\n<size=70%> <sprite name=DiscWhiteUp> {p1}: {whiteScoreCount}    <sprite name=DiscBlackUp> {p2}: {blackScoreCount}</size>";

        if (winner==Player.White)
            winnerText.text=$"Wygrywa {p1} <sprite name=DiscWhiteUp> {scoreText}";
        else if (winner==Player.Black)
            winnerText.text=$"Wygrywa {p2} <sprite name=DiscBlackUp> \n {scoreText}";
        else
            winnerText.text=$"Remis!{scoreText}";
    }
    public IEnumerator AnimateCurrentPlayerText() 
    { 
        playerTurnText.transform.LeanScale(Vector3.one*1.2f,0.5f).setLoopPingPong(3);
        yield return new WaitForSeconds(3.5f);
    }

    public IEnumerator HideEndScreen()
    {
        winnerText.rectTransform.LeanMoveLocalY(800f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        blackOverlay.gameObject.SetActive(false);
        winnerText.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
    }
}
