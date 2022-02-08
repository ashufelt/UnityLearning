using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    [SerializeField] Tile tilePrefab;
    [SerializeField] private GameSession gameSession;
    [SerializeField] float center_x;
    [SerializeField] float center_y;

    [SerializeField] Color incorrectColor;
    [SerializeField] Color correctColor;

    private List<Tile> tileList;
    private List<int> locations;

    private int expectedValue;
    private bool clicksEnabled;
    // Start is called before the first frame update
    void Start()
    {
        clicksEnabled = true;
        if (gameSession == null)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
    }

    public void SetupTiles(int tileCount)
    {
        int sideLength = 4;
        while (tileCount > (sideLength * sideLength) / 2)
        {
            sideLength++;
        }
        expectedValue = 1;
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
            int randomInt = Random.Range(0, locations.Count);
            int locationIndex = locations[randomInt];
            locations.RemoveAt(randomInt);
            
            Tile tmpTile = Instantiate<Tile>(tilePrefab, GetLocation(locationIndex, sideLength), Quaternion.identity);
            tmpTile.SetValueAndLocation(i + 1, locationIndex);
            tmpTile.GetComponent<Transform>().localScale *= (4.0f / sideLength);
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

    public void HideValues()
    {
        foreach (Tile tile in tileList)
        {
            tile.HideValue();
        }
    }

    public void DisplayValues()
    {
        foreach (Tile tile in tileList)
        {
            tile.DisplayValue();
        }
    }

    public void SetClicksEnabled(bool clicksEnabledBool)
    {
        clicksEnabled = clicksEnabledBool;
    }

    public void HandleTileClick(Tile clickedTile)
    {
        if (clicksEnabled)
        {
            clickedTile.DisplayValue();
            if (clickedTile.GetValue() == expectedValue)
            {
                Debug.Log("Tile clicked correctly - TileHandler.cs");
                expectedValue++;
                clickedTile.SetColor(correctColor);
                gameSession.HandleCorrectClick(clickedTile.GetValue());
            }
            else
            {
                Debug.Log("Incorrect tile");
                expectedValue = 1;
                clickedTile.SetColor(incorrectColor);
                gameSession.HandleIncorrectClick();
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
