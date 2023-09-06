using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] int locationIndex;

    Text valueText;
    Image squareTileImage;

    //I think Tile shouldn't need a reference to the TileHandler that is holding and handling it
    //TileHandler tileHandler;

    public void SetValue(int val) { value = val; }
    public int GetValue() { return value; }
    public void SetLocation(int index) { locationIndex = index; }
    public int GetLocation() { return locationIndex; }
    public void SetValueAndLocation(int val, int index) { value = val; locationIndex = index; }

    public event Action<Tile> onTileClickedAction;

    // Start is called before the first frame update
    void Start()
    {
        valueText = GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
        squareTileImage = GetComponentInChildren<Canvas>().GetComponentInChildren<Image>();
        if (valueText == null)
        {
            Debug.Log("Couldn't get Text component of Tile");
        }
        else
        {
            //Debug.Log(valueText.ToString());
            valueText.text = value.ToString();
        }
    }

    public void OnTileClicked()
    {
        Debug.Log("OnTileClicked called");
        Debug.Log("Tile with value " + value + " clicked");
        if (onTileClickedAction != null)
        {
            onTileClickedAction(this);
        }
    }

    public void RevealTileValue()
    {
        valueText.text = value.ToString();
    }

    public void HideTileValue()
    {
        valueText.text = "?";
    }

    public void RemoveTile()
    {
        Destroy(gameObject);
    }

    public void SetColor(Color newColor)
    {
        squareTileImage.color = newColor;
    }
}
