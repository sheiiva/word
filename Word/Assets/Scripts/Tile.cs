using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public char _letter { get; private set; }

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetLetter(char letter)
    {
        this._letter = letter;
        _text.text = letter.ToString();
    }
}
