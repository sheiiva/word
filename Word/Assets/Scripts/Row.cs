using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] _tiles { get; private set; }

    public string word
    {
        get
        {
            string word = "";

            foreach (Tile tile in _tiles)
            {
                word += tile._letter;
            }

            return word;
        }
    }

    private void Awake()
    {
        _tiles = GetComponentsInChildren<Tile>();
    }
}
