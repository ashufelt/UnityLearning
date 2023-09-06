using System;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    [SerializeField] Tile tilePrefab;
    [SerializeField] float center_x;
    [SerializeField] float center_y;

    [SerializeField] Color incorrectColor;
    [SerializeField] Color correctColor;

    private List<Tile> tileList;
    private List<int> locations;

    private int correctNextSelection;
    private bool clicksEnabled;

    public event Action<bool, int> clickResultDetermined;

    // Start is called before the first frame update
    void Start()
    {
        clicksEnabled = true;
    }

    public void SetupTiles(int tileCount)
    {
        int sideLength = 4;
        while (tileCount > (sideLength * sideLength) / 2)
        {
            sideLength++;
        }
        correctNextSelection = 1;
        tileList = new List<Tile>();
        locations = new List<int>();
        for (int j = 0; j < (sideLength * sideLength); j++)
        {
            locations.Add(j);
        }

        //Debug.Log("SetupTiles called");
        for (int i = 0; i < tileCount; i++)
        {
            //Debug.Log(locations.ToString());
            int randomInt = UnityEngine.Random.Range(0, locations.Count);
            int locationIndex = locations[randomInt];
            locations.RemoveAt(randomInt);

            Tile tmpTile = Instantiate<Tile>(tilePrefab, GetLocation(locationIndex, sideLength), Quaternion.identity);
            tmpTile.SetValueAndLocation(i + 1, locationIndex);
            tmpTile.GetComponent<Transform>().localScale *= (4.0f / sideLength);
            tmpTile.onTileClickedAction += HandleTileClick;
            tileList.Add(tmpTile);
        }
    }

    private Vector3 GetLocation(int locationIndex, int sideLength)
    {
        int x_index = locationIndex % sideLength;
        int y_index = locationIndex / sideLength;

        float x_location = center_x - 3.75f + ((float)(x_index) / (float)(sideLength - 1) * 7.5f);
        float y_location = center_y + 3.75f - ((float)(y_index) / (float)(sideLength - 1) * 7.5f);

        return new Vector3(x_location, y_location, 0);
    }

    public void HideTileValues()
    {
        foreach (Tile tile in tileList)
        {
            tile.HideTileValue();
        }
    }

    public void DisplayValues()
    {
        foreach (Tile tile in tileList)
        {
            tile.RevealTileValue();
        }
    }

    public void SetClicksEnabled(bool clicksEnabledSet)
    {
        clicksEnabled = clicksEnabledSet;
    }

    public void HandleTileClick(Tile clickedTile)
    {
        if (!clicksEnabled)
        {
            return;
        }

        clickedTile.RevealTileValue();
        if (clickedTile.GetValue() == correctNextSelection)
        {
            Debug.Log("Tile clicked correctly - TileHandler.cs");
            correctNextSelection++;
            clickedTile.SetColor(correctColor);
            if (clickResultDetermined != null)
            {
                clickResultDetermined(true, clickedTile.GetValue());
            }
        }
        else
        {
            Debug.Log("Incorrect tile");
            correctNextSelection = 1;
            clickedTile.SetColor(incorrectColor);
            if (clickResultDetermined != null)
            {
                clickResultDetermined(false, clickedTile.GetValue());
            }
        }
    }

    public void ResetTiles()
    {
        foreach (Tile tile in tileList)
        {
            Debug.Log("Destroying Tile " + tile.GetValue());
            tile.RemoveTile();
        }
        Debug.Log("Clearing tileList");
        tileList.Clear();
    }
}
