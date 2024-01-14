using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color _fillColor;
        public Color _outlineColor;
    }

    public State _state { get; private set; }
    public char _letter { get; private set; }

    private TextMeshProUGUI _text;
    private Image _image;
    private Outline _outline;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponent<Image>();
        _outline = GetComponent<Outline>();
    }

    public void SetLetter(char letter)
    {
        this._letter = letter;
        _text.text = letter.ToString();
    }

    public void SetState(State state)
    {
        _state = state;
        _image.color = state._fillColor;
        _outline.effectColor = state._outlineColor;
    }
}
