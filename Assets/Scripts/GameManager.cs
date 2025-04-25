using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum CoinSide { None, Head, Tail }
    public CoinSide playerChoice = CoinSide.None;

    [Header("Balance & Bet")]
    //public float playerBalance = 100f;
    public float playerBalance;
    public float currentBet = 1f;
    private const string BalanceKey = "PlayerBalance";

    [Header("UI References")]
    public TMP_Text balanceText;
    public TMP_Text betText;
    public TMP_Text resultText;
    public GameObject instructionsPanel;

    [Header("Coin")]
    public CoinController coinController;

    private void Start()
    {
        UpdateUI();
        resultText.gameObject.SetActive(false);
    }

    public void SetPlayerChoice(int choice) // 1 = Head, 2 = Tail
    {
        SoundManager.Instance.PlayButtonClick();
        playerChoice = (CoinSide)choice;
    }

    public void IncreaseBet()
    {
        if (currentBet < 20f)
        {
            currentBet++;
            SoundManager.Instance.PlayButtonClick();
            UpdateUI();
        }
    }

    public void DecreaseBet()
    {
        if (currentBet > 1f)
        {
            currentBet--;
            SoundManager.Instance.PlayButtonClick();
            UpdateUI();
        }
    }

    public void FlipCoin()
    {
        if (playerChoice == CoinSide.None || playerBalance < currentBet)
        {
            if (playerChoice == CoinSide.None)
                ShowResult("Please Choose Head or Tail");
            else
                ShowResult("Insufficent Balance");

            SoundManager.Instance.PlayButtonClick();
            return;
        }

        SoundManager.Instance.PlayButtonClick();
        playerBalance -= currentBet;
        SaveBalance();
        UpdateUI();
        coinController.Flip(Random.Range(1, 3), OnFlipComplete); // 1 = Head, 2 = Tail
    }

    private void OnFlipComplete(int outcome) // 1 = Head, 2 = Tail
    {
        CoinSide result = (CoinSide)outcome;

        if (playerChoice == result)
        {
            float winAmount = currentBet * 1.98f;
            playerBalance += winAmount;
            SaveBalance();
            SoundManager.Instance.PlayWin();
            ShowResult("You won $" + winAmount.ToString("F2"));
        }
        else
        {
            SoundManager.Instance.PlayLose();
            ShowResult("You lost!");
        }

        UpdateUI();
        playerChoice = CoinSide.None;
    }

    private void UpdateUI()
    {
        LoadBalance();
        balanceText.text = "$" + playerBalance.ToString("F2");
        betText.text = "$" + currentBet.ToString("F2");
    }

    private void ShowResult(string msg)
    {
        resultText.text = msg;
        resultText.gameObject.SetActive(true);
        Invoke(nameof(HideResult), 2f); // auto-hide after 2 seconds
    }

    private void LoadBalance()
    {
        // Default to 100 if no saved value
        playerBalance = PlayerPrefs.GetFloat(BalanceKey, 100.00f);
    }

    private void SaveBalance()
    {
        PlayerPrefs.SetFloat(BalanceKey, playerBalance);
        PlayerPrefs.Save();
    }

    private void HideResult()
    {
        resultText.gameObject.SetActive(false);
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
        #else
                Application.Quit(); // Quit the actual build
        #endif
    }


}
