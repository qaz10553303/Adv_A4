using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    public int gridSize;
    public int movesLeft;
    public float timeLeft;
    public int skillLevel;

    public Tile emptyTile;
    public int levelDifficulty;//1-3

    public Material[] easyMats;
    public Material[] normalMats;
    public Material[] hardMats;

    public GameObject tilePrefab;

    public List<Tile> tiles= new List<Tile>();

    public bool isGameOver=false;

    public GameObject[] levels;
    public GameObject resultScreen;
    public Text resultText;
    public Text[] skillTexts;
    public Text[] TimeTexts;
    public Text[] movesTexts;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (isGameOver) return;
        isGameOver = WinLoseCheck();

    }

    private void CreateTiles()
    {
        GameObject tileContainer = GameObject.FindGameObjectWithTag("TileContainer");
        Transform parent = tileContainer.transform;
        

        for (int y = 0; y < gridSize; y++)//set transform values
        {
            for (int x = 0; x < gridSize; x++)
            {
                Tile tile = Instantiate(tilePrefab,parent).GetComponent<Tile>();
                RectTransform tileRect = tile.GetComponent<RectTransform>();
                float borderSize = 1080 / gridSize;
                tileRect.sizeDelta = new Vector2(borderSize, borderSize);//set border size
                tile.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(borderSize - 10, borderSize - 10);//set pic size
                tileRect.anchoredPosition = new Vector2(x* borderSize, -y* borderSize);
                tiles.Add(tile);
            }
        }
        for (int i = 0; i < tiles.Count; i++)//set script values
        {
            tiles[i].mapIndex = i + 1;
            if (i == tiles.Count - 1)//set last special tile
            {
                tiles[i].currentMat = 0;
                emptyTile = tiles[i];
                break;
            }
            tiles[i].currentMat = i + 1;
        }
    }

    public void RandomizeTilePos()
    {
        for (int i = 0; i < 1000; i++)
        {
            tiles[Random.Range(0, tiles.Count)].SwapTile(false);
        }
    }

    bool WinLoseCheck()
    {
        bool isWin=true;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].currentMat != tiles[i].mapIndex)
            {
                if (tiles[i].currentMat != 0 || tiles[i].mapIndex != tiles.Count)
                {
                    isWin = false;
                    break;
                }
            }
        }
        if (isWin)
        {
            //win
            tiles[tiles.Count - 1].currentMat = tiles.Count;
            resultText.text = "You Win!";
            resultScreen.SetActive(true);
            return true;
        }
        else if (movesLeft==0||timeLeft<=0)
        {
            //lose
            resultText.text = "You Lose!";
            resultScreen.SetActive(true);
            return true;
        }
        return false;
    }

    public void IncreaseSkillLv(int increaseAmount)
    {
        skillLevel += increaseAmount;
        skillLevel = Mathf.Clamp(skillLevel,0, 3);
    }

    public void SelectDifficulty(int difficulty)
    {
        levelDifficulty = difficulty;
        levels[0].SetActive(false);//select level page
        levels[levelDifficulty].SetActive(true);
        Init();
    }

    void UpdateUI()
    {
        if (!isGameOver)
        {
            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Max(timeLeft, 0);
        }
        for (int i = 0; i < skillTexts.Length; i++)
        {
            skillTexts[i].text = "SkillLevel: " + skillLevel;
        }
        for (int i = 0; i < TimeTexts.Length; i++)
        {
            TimeTexts[i].text = "Time: " + Mathf.Floor(timeLeft);
        }
        for (int i = 0; i < movesTexts.Length; i++)
        {
            movesTexts[i].text = "MovesLeft: " + movesLeft;
        }
    }

    void Init()
    {
        isGameOver = false;

        gridSize = levelDifficulty < 3 ? 3 : 4;
        timeLeft = 100+100 * (skillLevel+levelDifficulty);
        Debug.Log(timeLeft);
        movesLeft = 0+100 * (skillLevel + levelDifficulty);
        CreateTiles();
        RandomizeTilePos();

    }


}
