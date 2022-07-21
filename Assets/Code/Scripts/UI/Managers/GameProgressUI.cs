using Tanks.Gameplay;
using Tanks.Shared;
using TMPro;
using UnityEngine;

public class GameProgressUI : MonoBehaviour
{
    public TextMeshProUGUI gameMessageText;
    public Color teamOneColor;
    public Color teamTwoColor;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        EventManager.AddListener<RoundStartingEvent>(OnRoundStarting);
        EventManager.AddListener<RoundEndingEvent>(OnRoundEnding);
    }

    public void OnRoundStarting(RoundStartingEvent evt)
    {
        int roundNumber = gameManager.roundNumber;
        gameMessageText.text = "ROUND " + evt.roundNumber;
    }

    public void OnRoundEnding(RoundEndingEvent evt)
    {
        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

        string message = "DRAW!";

        if (roundWinner != null)
        {
            message = roundWinner.coloredPlayerText + " WINS THE ROUND!";
        }

        message += "\n\n\n\n";

        for (int i = 0; i < playersTanks.Length; i++)
        {
            message += playersTanks[i].coloredPlayerText + ": " + playersTanks[i].wins + " WINS\n";
        }

        if (gameWinner != null)
        {
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";
        }

        gameMessageText.text = message;
    }
}
