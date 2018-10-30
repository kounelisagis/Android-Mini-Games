using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ΑΙmove
{
    private static char player1, player2;
    private const char tie = 'T';
    private const char notYet = 'N';
    private const char EMPTY = ' ';
    private static int choice;


    public static int getAImove(TileTic.TileType pcType, TileTic.TileType playerType, BoardTic board)
    {
        player2 = createPlayer(pcType);
        player1 = createPlayer(playerType);

        if (menuTic.mode == 1)
        {
            choice = Random.Range(0, 8);
            while (!board.getTile(choice).isEmpty())
                choice = Random.Range(0, 8);
        }

        else if (menuTic.mode == 2)
        {
            int randomOrNot = Random.Range(0, 10);

            if (randomOrNot <= 7)
            {
                if (!board.allFilled())
                    miniMax(createPlayer(pcType), createBoard(board), 0);

                else
                {
                    int[] squares = new int[4] { 0, 2, 6, 8 };
                    int index = Random.Range(1, 4); // creates a number between 1 and 12
                    choice = squares[index];
                }

            }

            else //random choice
            {
                choice = Random.Range(0, 8);
                while (!board.getTile(choice).isEmpty())
                    choice = Random.Range(0, 8);
            }
        }


        else
        {
            if (!board.allFilled())
                miniMax(createPlayer(pcType), createBoard(board), 0);

            else
            {
                int[] squares = new int[4] { 0, 2, 6, 8 };
                int index = Random.Range(1, 4); // creates a number between 1 and 12
                choice = squares[index];
            }

        }

        return choice;
    }

    static char[] createBoard(BoardTic myBoard)
    {
        char[] newBoard = new char[9];

        for(int i=0;i<9;i++)
            newBoard[i] = myBoard.getChar(i);

        return newBoard;
    }

    static char createPlayer (TileTic.TileType myPlayer)
    {
        return myPlayer == TileTic.TileType.X ? 'X' : 'O';
    }

    private const int squaresNum = 9;

    static char checkForWinner(char[] myBoard)
    {
        int[,] winnerCombinations = new int[8, 3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };

        for (int i = 0; i < 8; ++i)
            if (myBoard[winnerCombinations[i, 0]] != EMPTY && myBoard[winnerCombinations[i, 0]] == myBoard[winnerCombinations[i, 1]] && myBoard[winnerCombinations[i, 1]] == myBoard[winnerCombinations[i, 2]])
                return myBoard[winnerCombinations[i, 0]];

        int emptiesCounter = 0;

        for (int i = 0; i < squaresNum; ++i)
            if (myBoard[i] == EMPTY)
                ++emptiesCounter;

        if (emptiesCounter == 0)
        {
            return tie;
        }

        return notYet;

    }




    private static int miniMax(char player, char[] myBoard, int depth)
    {

        /*
		 * DON'T CREATE LAST BOARD!============================
		 */

        int empties = 0;
        for (int i = 0; i < 9; i++)
            if (myBoard[i] == EMPTY)
                empties++;

        if (empties == 1)
            for (int i = 0; i < 9; i++)
                if (myBoard[i] == EMPTY)
                {
                    myBoard[i] = player;
                    break;
                }

        /*
		 * ====================================================
		 */

        if (menuTic.mode == 4)
        {
            if (checkForWinner(myBoard) == player2)
                return (10 + depth);
            if (checkForWinner(myBoard) == player1)
                return (-10 - depth);
            if (checkForWinner(myBoard) == tie)
                return 0;
        }

        else
        {
            if (checkForWinner(myBoard) == player2)
                return (10 - depth);
            if (checkForWinner(myBoard) == player1)
                return (-10 + depth);
            if (checkForWinner(myBoard) == tie)
                return 0;
        }



        char[] newBoard = new char[9];
        for (int j = 0; j < squaresNum; j++)
            newBoard[j] = myBoard[j];


        List<int> scoresList = new List<int>();
        List<int> movesList = new List<int>();


        for (int i = 0; i < squaresNum; i++)
        {

            if (myBoard[i] == EMPTY)
            {

                newBoard[i] = player;

                if (player == player1)
                    scoresList.Add(miniMax(player2, newBoard, depth + 1));

                else
                    scoresList.Add(miniMax(player1, newBoard, depth + 1));

                movesList.Add(i);

                newBoard[i] = EMPTY;

            }

        }

        if (player == player2)    //computer
        {
            int MaxScoreIndex = scoresList.IndexOf(scoresList.Max());
            choice = movesList[MaxScoreIndex];
            return scoresList.Max();
        }
        else
        {
            int MinScoreIndex = scoresList.IndexOf(scoresList.Min());
            choice = movesList[MinScoreIndex];
            return scoresList.Min();
        }


    }
}
