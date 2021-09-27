using UnityEngine;
using UnityEngine.UI;

public class WelcomeScreen : MonoBehaviour
{
    [SerializeField] private Text nameInput;
    [SerializeField] private Text errorMessage;

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void ConfirmPlayerName()
    {
        if (!string.IsNullOrWhiteSpace(nameInput.text))
        {
            PlayerPrefs.SetString("name", nameInput.text);
            gameObject.SetActive(false);
        }
        else
        {
            DisplayErrorMessage();
        }
    }

    private void DisplayErrorMessage()
    {
        errorMessage.gameObject.SetActive(true);
    }
}
