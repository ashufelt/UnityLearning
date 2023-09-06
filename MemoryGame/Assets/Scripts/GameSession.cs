using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] TileHandler tileHandlerPrefab;
    [SerializeField] Text timerText;

    [SerializeField] int level = 1;
    [SerializeField] float timePerTile = 1.5f;
    [SerializeField] float endGameDelayTime = 2.0f;
    [SerializeField] float nextLevelDelayTime = 1.0f;

    TileHandler tileHandler;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (tileHandler == null)
        {
            tileHandler = FindObjectOfType<TileHandler>();
        }
        tileHandler.clickResultDetermined += HandleClickResult;
        SetupLevel(level);
    }

    private void SetupLevel(int newLevel)
    {
        tileHandler.SetClicksEnabled(false);
        tileHandler.SetupTiles(newLevel + 2);
        StartCoroutine(LevelCountdown(newLevel));
    }

    IEnumerator LevelCountdown(int newLevel)
    {
        int countdownValue = Mathf.CeilToInt((level + 2) * timePerTile);
        timerText.enabled = true;
        timerText.text = countdownValue.ToString();
        for (int i = countdownValue; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerText.text = (i - 1).ToString();
        }
        tileHandler.HideTileValues();
        tileHandler.SetClicksEnabled(true);
        yield return new WaitForSeconds(1);
        timerText.enabled = false;
    }

    public void HandleClickResult(bool clickWasCorrect, int recentValueClicked)
    {
        if (clickWasCorrect)
        {
            Debug.Log("Tile clicked correctly - GameSession.cs");
            Debug.Log("Level = " + level + " and Tile clicked = " + recentValueClicked);
            if (recentValueClicked >= level + 2)
            {
                tileHandler.SetClicksEnabled(false);
                level++;
                StartCoroutine(StartNextLevelAfterDelay());
            }
        }
        else
        {
            tileHandler.SetClicksEnabled(false);
            level = 1;
            StartCoroutine(ResetGameAfterDelay());
        }
    }

    IEnumerator StartNextLevelAfterDelay()
    {
        yield return new WaitForSeconds(nextLevelDelayTime);
        tileHandler.ResetTiles();
        SetupLevel(level);
    }


    IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(endGameDelayTime);
        tileHandler.ResetTiles();
        SetupLevel(level);
    }
}
