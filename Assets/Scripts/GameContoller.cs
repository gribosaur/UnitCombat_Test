using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameContoller : MonoBehaviour
{
    [Header("Юниты")]
    [SerializeField] private UnitUI firstUnit;
    [SerializeField] private UnitUI secondUnit;

    [Header("Другая информация")]
    [SerializeField] TextMeshProUGUI roundText;

    public static Action OnRoundEnded;

    private int roundNum = 1;
    private bool isFirstTurn = true; //первый ли ход в раунде

    private void OnEnable()
    {
        UnitUI.OnMoveIsOver += ChangeTurn;
        Unit.OnUnitDied += RestartGame;
    }

    private void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        firstUnit.SetActive(true);
        secondUnit.SetActive(false);

        isFirstTurn = true;
        roundNum = 1;
        roundText.text = $"Round {roundNum}";
    }

    private void ChangeTurn()
    {
        if (isFirstTurn)
        {
            firstUnit.SetActive(false);
            secondUnit.SetActive(true);
        }
        else
        {
            OnRoundEnded.Invoke();

            firstUnit.SetActive(true);
            secondUnit.SetActive(false);

            roundNum++;
            roundText.text = $"Round {roundNum}";         
        }

        isFirstTurn = !isFirstTurn;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        UnitUI.OnMoveIsOver -= ChangeTurn;
        Unit.OnUnitDied -= RestartGame;
    }
}
