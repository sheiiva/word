using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
    };

    private Row[] _rows;

    private string[] _validWords;
    [SerializeField]
    private string _solution = "";

    private int _rowI = 0;
    private int _tilesI = 0;

    [Header("States")]
    public Tile.State _defaultState;
    public Tile.State _correctState;
    public Tile.State _wrongPositionState;
    public Tile.State _wrongState;
    public Tile.State _selectedState;

    [Header("UI")]
    public TextMeshProUGUI _invalidWordText;
    public Button _newWordButton;

    private void Awake() 
    {
        _rows = GetComponentsInChildren<Row>();
    }

    private void Start()
    {
        LoadData();
        SetRandomSolution();
    }

    public void NewWord()
    {
        SetRandomSolution();
        ClearBoard();
        enabled = true; // Enable this script
    }

    private void ClearBoard()
    {
        foreach (Row row in _rows)
        {
            for (int i = 0; i < row._tiles.Length; i++)
            {
                row._tiles[i].SetState(_defaultState);
                row._tiles[i].SetLetter('\0');
            }
        }
        _rowI = 0;
        _tilesI = 0;
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load<TextAsset>("en_word");

        _validWords = textFile.text.Split('\n');
        for (int i = 0; i < _validWords.Length; i++)
        {
            _validWords[i] = _validWords[i].ToLower().Trim();
        }
    }

    private void SetRandomSolution()
    {
        _solution = _validWords[Random.Range(0, _validWords.Length)];
        _solution = _solution.ToLower().Trim();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            DeleteLetter();
            _invalidWordText.gameObject.SetActive(false);
        }
        else if (_tilesI >= _rows[_rowI]._tiles.Length)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SubmitRow(_rows[_rowI]);
            }
        }
        else
        {
            foreach (KeyCode key in SUPPORTED_KEYS)
            {
                if (Input.GetKeyUp(key))
                {
                    AddLetter((char)key);
                    break; // Only one key can be pressed at a time
                }
            }
        }
    }

    private void AddLetter(char letter)
    {
        _rows[_rowI]._tiles[_tilesI].SetState(_selectedState);
        _rows[_rowI]._tiles[_tilesI].SetLetter(letter);
        _tilesI++;
    }

    private void DeleteLetter()
    {
        _tilesI = Mathf.Max(0, _tilesI - 1);
        _rows[_rowI]._tiles[_tilesI].SetState(_defaultState);
        _rows[_rowI]._tiles[_tilesI].SetLetter('\0');
    }

    private void SubmitRow(Row row)
    {
        if (!IsValidWord(row.word))
        {
            _invalidWordText.gameObject.SetActive(true);
            return;
        }

        string remainingLetters = _solution;

        // Check for bad and correct letters
        for (int i = 0; i < row._tiles.Length; i++)
        {
            Tile tile = row._tiles[i];

            if (tile._letter == _solution[i])
            {
                tile.SetState(_correctState);
                remainingLetters = remainingLetters.Remove(i, 1);
                remainingLetters = remainingLetters.Insert(i, " ");
            }
            else if (!_solution.Contains(tile._letter))
            {
                tile.SetState(_wrongState);
            }
        }

        // Check for wrong position
        for (int i = 0; i < row._tiles.Length; i++)
        {
            Tile tile = row._tiles[i];

            if (tile._state != _correctState && tile._state != _wrongState)
            {
                if (remainingLetters.Contains(tile._letter))
                {
                    tile.SetState(_wrongPositionState);
                    int index = remainingLetters.IndexOf(tile._letter);
                    remainingLetters = remainingLetters.Remove(index, 1);
                    remainingLetters = remainingLetters.Insert(index, " ");
                }
                else
                {
                    tile.SetState(_wrongState);
                }
            }
        }

        if (HasWon(row))
        {
            enabled = false; // Disable this script
        }

        // Reset indexes
        _rowI++;
        _tilesI = 0;

        if (_rowI >= _rows.Length)
        {
            enabled = false; // Disable this script
        }
    }

    private bool IsValidWord(string word)
    {
        return System.Array.IndexOf(_validWords, word) != -1;
    }

    private bool HasWon(Row row)
    {
        for (int i = 0; i < row._tiles.Length; i++)
        {
            if (row._tiles[i]._state != _correctState)
            {
                return false;
            }
        }
        return true;
    }

    private void OnEnable()
    {
        _newWordButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _newWordButton.gameObject.SetActive(true);
    }
}
