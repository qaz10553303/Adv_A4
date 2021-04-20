using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int currentMat;//0-9,0=null
    public int mapIndex;
    GameManager gm;

    private void Awake()
    {
        gm = GameManager.Instance;
    }
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        UpdateMaterial();
    }

    public void SwapTile(bool isPlayerOperation)
    {
        if (gm.isGameOver) return;
        Tile emptyTile = GameManager.Instance.emptyTile;
        float tileSize = GetComponent<RectTransform>().rect.width;
        if (Vector2.Distance(emptyTile.transform.position, transform.position) > tileSize) return;
        if (isPlayerOperation && emptyTile != this) gm.movesLeft -= 1;
        emptyTile.currentMat = currentMat;
        currentMat = 0;
        GameManager.Instance.emptyTile = this;
    }

    void UpdateMaterial()
    {
        switch (gm.levelDifficulty)
        {
            case 1:
                transform.GetChild(0).GetComponent<Image>().material = gm.easyMats[currentMat];
                break;
            case 2:
                transform.GetChild(0).GetComponent<Image>().material = gm.normalMats[currentMat];
                break;
            case 3:
                transform.GetChild(0).GetComponent<Image>().material = gm.hardMats[currentMat];
                break;
        }
    }




    
}
