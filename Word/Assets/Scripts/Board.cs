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

    private int _rowI = 0;
    private int _tilesI = 0;

    private void Awake() 
    {
        _rows = GetComponentsInChildren<Row>();
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
                SubmitRow();
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
        _rows[_rowI]._tiles[_tilesI].SetLetter(key);
        _tilesI++;
    }

    private void DeleteLetter()
    {
        _tilesI = Mathf.Max(0, _tilesI - 1);
        _rows[_rowI]._tiles[_tilesI].SetLetter('\0');
    }

    private void SubmitRow()
    {
        // _rowI++;
        // _tilesI = 0;
    }
}
