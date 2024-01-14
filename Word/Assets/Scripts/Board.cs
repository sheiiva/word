using UnityEngine;

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

    private void Awake() 
    {
        _rows = GetComponentsInChildren<Row>();
    }

    private void Start()
    {
        LoadData();
        SetRandomSolution();
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load<TextAsset>("en_word");

        _validWords = textFile.text.Split('\n');
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
        for (int i = 0; i < row._tiles.Length; i++)
        {
            Tile tile = row._tiles[i];

            if (tile._letter == _solution[i])
            {
                tile.SetState(_correctState);
            }
            else if(_solution.Contains(tile._letter.ToString()))
            {
                tile.SetState(_wrongPositionState);
            }
            else
            {
                tile.SetState(_wrongState);
            }
        }
        // Reset indexes
        _rowI++;
        _tilesI = 0;
    }
}
