using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI headText;
    public GameObject headButton;

    public void ChangeHeadButtonVisibility()
    {
        if (GameManager.Instance.currentPlayer == Players.Giraffe)
        {
            headButton.SetActive(true);
        }
        else
        {
            headButton.SetActive(false);
        }
    }
    
    public void ChangePlayerButtonText()
    {
        var nextPlayer = GameManager.Instance.currentPlayer.Next();

        buttonText.text = nextPlayer.ToString();
    }

    public void ChangeHeadButtonText()
    {
        if (GameManager.Instance.isHeadActive)
        {
            headText.text = "No head";
        }
        else
        {
            headText.text = "Head";
        }
    }
}
