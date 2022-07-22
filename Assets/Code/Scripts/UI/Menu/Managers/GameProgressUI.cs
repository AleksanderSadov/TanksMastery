using Tanks.Gameplay;
using TMPro;
using UnityEngine;

namespace Tanks.UI
{
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
            EventManager.AddListener<RoundStartedEvent>(OnRoundStarted);
            EventManager.AddListener<RoundEndingEvent>(OnRoundEnding);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<RoundStartingEvent>(OnRoundStarting);
            EventManager.RemoveListener<RoundStartedEvent>(OnRoundStarted);
            EventManager.RemoveListener<RoundEndingEvent>(OnRoundEnding);
        }

        public void OnRoundStarting(RoundStartingEvent evt)
        {
            int roundNumber = gameManager.roundNumber;
            gameMessageText.text = "ROUND " + evt.roundNumber;
        }

        public void OnRoundStarted(RoundStartedEvent evt)
        {
            gameMessageText.text = string.Empty;
        }

        public void OnRoundEnding(RoundEndingEvent evt)
        {
            string message = "DRAW!";

            if (evt.roundWinnerTeam != null)
            {
                message = evt.roundWinnerTeam.coloredTeamText + " WINS THE ROUND!";
            }

            message += "\n\n\n\n";

            foreach (Team team in evt.teams)
            {
                message += team.coloredTeamText + ": " + team.roundsWon + " WINS\n";
            }

            if (evt.gameWinnerTeam != null)
            {
                message = evt.gameWinnerTeam.coloredTeamText + " WINS THE GAME!";
            }

            gameMessageText.text = message;
        }
    }
}
