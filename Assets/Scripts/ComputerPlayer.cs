using UnityEngine;
using System;

public class ComputerPlayer
{
    private readonly int depth;

    public ComputerPlayer(int depth)
    {
        this.depth = depth;
    }
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
       
        if (depth == 0 || state.GameOver)
        {
            return Evaluate(state, me);
        }

       
        bool myTurn = state.CurrentPlayer == me;
        int best = myTurn ? int.MinValue : int.MaxValue;

         
        foreach (Position move in state.LegalMoves.Keys)
        {
            GameState copy = state.Clone();                  
            copy.MakeMove(move, out _);                     
            int score = Minimax(copy, depth - 1, me);        

            if (myTurn)                                      
                best = Math.Max(best, score);
            else
                best = Math.Min(best, score);
        }

        return best;
    }
    public Position FindBestMove(GameState state)
    {
        Player me = state.CurrentPlayer;
        Position bestMove = null;
        int bestScore = int.MinValue;

        foreach (Position move in state.LegalMoves.Keys)
        {
            GameState copy = state.Clone();
            copy.MakeMove(move, out _);
            int score = Minimax(copy, depth - 1, me);
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }
}