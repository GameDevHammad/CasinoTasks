using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum CoinSide { None, Head, Tail }
    public CoinSide playerChoice = CoinSide.None;

    [Header("Balance & Bet")]
    public float playerBalance = 100f;
    public float currentBet = 1f;

    [Header("UI References")]
    public TMP_Text balanceText;
    public TMP_Text betText;
    public TMP_Text resultText;

    [Header("Coin")]
    public CoinController coinController;

    private void Start()
    {
        UpdateUI();
        resultText.gameObject.SetActive(false);
    }

    public void SetPlayerChoice(int choice) // 1 = Head, 2 = Tail
    {
        playerChoice = (CoinSide)choice;
    }

    public void IncreaseBet()
    {
        if (currentBet < 20f)
        {
            currentBet++;
            UpdateUI();
        }
    }

    public void DecreaseBet()
    {
        if (currentBet > 1f)
        {
            currentBet--;
            UpdateUI();
        }
    }

    public void FlipCoin()
    {
        if (playerChoice == CoinSide.None || playerBalance < currentBet)
            return;

        playerBalance -= currentBet;
        UpdateUI();
        coinController.Flip(Random.Range(1, 3), OnFlipComplete); // 0 = Head, 1 = Tail
    }

    private void OnFlipComplete(int outcome) // 0 = Head, 1 = Tail
    {
        CoinSide result = (CoinSide)outcome;

        if (playerChoice == result)
        {
            float winAmount = currentBet * 1.98f;
            playerBalance += winAmount;
            ShowResult("You won $" + winAmount.ToString("F2"));
        }
        else
        {
            ShowResult("You lost!");
        }

        UpdateUI();
        playerChoice = CoinSide.None;
    }

    private void UpdateUI()
    {
        balanceText.text = "$" + playerBalance.ToString("F2");
        betText.text = "$" + currentBet.ToString("F2");
    }

    private void ShowResult(string msg)
    {
        resultText.text = msg;
        resultText.gameObject.SetActive(true);
        Invoke(nameof(HideResult), 2f); // auto-hide after 2 seconds
    }

    private void HideResult()
    {
        resultText.gameObject.SetActive(false);
    }


}
