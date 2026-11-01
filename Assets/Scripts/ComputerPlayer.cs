using UnityEngine;
using System;

public class ComputerPlayer
{
    private static readonly int[,] Weights =
    {
        { 100, -20,  10,   5,   5,  10, -20, 100 },
        { -20, -50,  -2,  -2,  -2,  -2, -50, -20 },
        {  10,  -2,   1,   1,   1,   1,  -2,  10 },
        {   5,  -2,   1,   0,   0,   1,  -2,   5 },
        {   5,  -2,   1,   0,   0,   1,  -2,   5 },
        {  10,  -2,   1,   1,   1,   1,  -2,  10 },
        { -20, -50,  -2,  -2,  -2,  -2, -50, -20 },
        { 100, -20,  10,   5,   5,  10, -20, 100 },
    };

    private int Evaluate(GameState state, Player me)
    {
        int score = 0;
        for (int r = 0; r < GameState.Rows; r++)
        {
            for (int c = 0; c < GameState.Cols; c++)
            {
                if (state.Board[r, c] == me)
                {
                    score += Weights[r, c];
                }
                else if (state.Board[r,c] == me.Opponent())
                {
                    score -= Weights[r, c];
                }
            }
        }
        return score;
    }
    private int Minimax(GameState state, int depth, Player me)
    {
        // 1. Warunek koñca: g³êbokoœæ 0 albo koniec gry
        if (depth == 0 || state.GameOver)
        {
            return Evaluate(state, me);
        }

        // 2. Kto teraz ma ruch?
        bool myTurn = state.CurrentPlayer == me;
        int best = myTurn ? int.MinValue : int.MaxValue;

        // 3. SprawdŸ ka¿dy mo¿liwy ruch
        foreach (Position move in state.LegalMoves.Keys)
        {
            GameState copy = state.Clone();                  // a)
            copy.MakeMove(move, out _);                      // b)
            int score = Minimax(copy, depth - 1, me);        // c)

            if (myTurn)                                      // d)
                best = Math.Max(best, score);
            else
                best = Math.Min(best, score);
        }

        return best;
    }
}