using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Disc discBlackUp;

    [SerializeField]
    private Disc discWhiteUp;

    [SerializeField]
    private GameObject highlightPrefab;

    [SerializeField]
    private UIManager uiManager;

    [Header("Ustawienia Trybu")]
    [SerializeField] private bool isVsComputer;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip placeSound;
    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip clickSound;

    private Dictionary<Player, Disc> discPrefabs = new Dictionary<Player, Disc>();
    private GameState gameState = new GameState();
    private Disc [,] discs = new Disc [8, 8];
    private List<GameObject> highlights = new List<GameObject>();

    void Start()
    {
        discPrefabs [Player.Black]=discBlackUp;
        discPrefabs [Player.White]=discWhiteUp;

        uiManager.SetGameMode(isVsComputer);

        AddStartDiscs();
        ShowLegalMoves();
        uiManager.SetPlayerText(gameState.CurrentPlayer);

       
    }

    private void Update()
    {
        UpdateScores();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (isVsComputer&&gameState.CurrentPlayer==Player.White)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (sfxSource!=null&&clickSound!=null)
                {
                    sfxSource.PlayOneShot(clickSound);
                }

                Vector3 impact = hitInfo.point;
                Position boardPos = SceneToBoardPos(impact);
                OnBoardClicked(boardPos);
            }
        }
    }

    private void ShowLegalMoves()
    {
        if (isVsComputer&&gameState.CurrentPlayer==Player.White)
        {
            return;
        }

        foreach (Position boardPos in gameState.LegalMoves.Keys)
        {
            Vector3 scenePos = BoardToScenePos(boardPos)+Vector3.up*0.01f;
            GameObject highlight = Instantiate(highlightPrefab, scenePos, Quaternion.identity);
            highlights.Add(highlight);
        }
    }

    private void HideLegalMoves()
    {
        highlights.ForEach(Destroy);
        highlights.Clear();
    }

    private void OnBoardClicked(Position boardPos)
    {
        if (gameState.MakeMove(boardPos, out MoveInfo moveInfo))
        {
            StartCoroutine(OnMoveMade(moveInfo));
        }
    }

    private IEnumerator OnMoveMade(MoveInfo moveInfo)
    {
        HideLegalMoves();
        yield return ShowMove(moveInfo);
        yield return ShowTurnOutcome(moveInfo);
        ShowLegalMoves();
        if (isVsComputer&&gameState.CurrentPlayer==Player.White&&!gameState.GameOver)
        {
            yield return new WaitForSeconds(0.5f); 
            MakeComputerMove();
        }
    }

    private void MakeComputerMove()
    {
        if (gameState.LegalMoves.Count>0)
        {
            Position bestMove = null;
            int maxFlips = -1;

            foreach (var move in gameState.LegalMoves)
            {
                if (move.Value.Count>maxFlips)
                {
                    maxFlips=move.Value.Count;
                    bestMove=move.Key;
                }
            }

            if (bestMove!=null)
            {
                OnBoardClicked(bestMove);
            }
        }
    }

    private Position SceneToBoardPos(Vector3 scenePos)
    {
        int col = (int)(scenePos.x-0.25f);
        int row = 7-(int)(scenePos.z-0.25f);
        return new Position(row, col);
    }

    private Vector3 BoardToScenePos(Position boardPos)
    {
        return new Vector3(boardPos.Col+0.75f, 0, 7-boardPos.Row+0.75f);
    }

    private void SpawnDisc(Disc prefab, Position boardPos)
    {
        Vector3 scenePos = BoardToScenePos(boardPos)+Vector3.up*0.1f;
        discs [boardPos.Row, boardPos.Col]=Instantiate(prefab, scenePos, Quaternion.identity);
    }

    private void AddStartDiscs()
    {
        if (sfxSource!=null&&placeSound!=null)
        {
            sfxSource.PlayOneShot(placeSound);
        }
        
        foreach (Position boardPos in gameState.OccupiedPositions())
        {
            Player player = gameState.Board [boardPos.Row, boardPos.Col];
            SpawnDisc(discPrefabs [player], boardPos);
        }
    }

    private void FlipDiscs(List<Position> positions)
    {
        foreach (Position boardPos in positions)
        {
            discs [boardPos.Row, boardPos.Col].Flip();
            if (sfxSource!=null&&flipSound!=null)
            {
                sfxSource.PlayOneShot(flipSound);
            }
             new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator ShowMove(MoveInfo moveInfo)
    {
        SpawnDisc(discPrefabs [moveInfo.Player], moveInfo.Position);
        yield return new WaitForSeconds(0.33f);
        FlipDiscs(moveInfo.Outflanked);
        yield return new WaitForSeconds(0.83f);
    }

    private IEnumerator ShowTurnSkipped(Player skippedPlayer)
    {
        uiManager.SetSkippedText(skippedPlayer);
        yield return uiManager.AnimateCurrentPlayerText();
    }
    private void UpdateScores()
    {
        uiManager.SetBlackScore(gameState.DiscCount [Player.Black]);
        uiManager.SetWhiteScore(gameState.DiscCount [Player.White]);
    }

    private void ShowGameOver(Player winner)
    {
        uiManager.SetWinnerText(winner, gameState.DiscCount [Player.White], gameState.DiscCount [Player.Black]);
        uiManager.ShowEndGameUI();
    }

    private IEnumerator ShowTurnOutcome(MoveInfo moveInfo)
    {
        if (gameState.GameOver)
        {
            yield return new WaitForSeconds(2.5f);
            ShowGameOver(gameState.Winner);
            yield break;
        }

        Player currentPlayer = gameState.CurrentPlayer;

        if (currentPlayer==moveInfo.Player)
        {
            yield return ShowTurnSkipped(currentPlayer.Opponent());
        }

        uiManager.SetPlayerText(currentPlayer);
    }

    private IEnumerator RestartGame()
    {
        yield return uiManager.HideEndScreen();
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Menu");
    }

    public void OnMenuClicked()
    {
        StartCoroutine(BackToMenu());
    }

    public void OnPlayAgainClicked()
    {
        StartCoroutine(RestartGame());
    }
}
