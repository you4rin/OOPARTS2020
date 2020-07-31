using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{

    public GameObject CloneBlock;

    public int WidthBlock = 10;
    public int HeightBlock = 10;
    public int CountOfMines = 0;
    protected int RemainMines;

    public Tile[,] ElementArray;
    public List<Vector2Int> MineCoords = new List<Vector2Int>();
    public List<Vector2Int> HintList = new List<Vector2Int>();
 

    void Awake()
    {
        ElementArray = new Tile[WidthBlock, HeightBlock];
    }

    Tile CloneTile(int p_x, int p_y)   //타일 복제
    {
        GameObject CopyObj = GameObject.Instantiate(CloneBlock.gameObject);
        CopyObj.transform.SetParent(this.transform);

        Vector2 temppos = new Vector2(p_x, p_y);
        CopyObj.transform.localPosition = temppos;
        CopyObj.name = "CloneBlock_" + p_x.ToString() + "_" + p_y.ToString();

        return CopyObj.GetComponent<Tile>();
    }

    void TileGenarator()   //타일 생성
    {
        for (int yy=0; yy< HeightBlock; ++yy)
        {
            for (int xx=0; xx< WidthBlock; ++xx)
            {
                ElementArray[xx, yy] = CloneTile(xx, yy);
            }
        }
    }

    bool GetMineAt(int p_x, int p_y)   //지뢰인지 확인
    {
        if ( (p_x >= 0 && p_x < WidthBlock)
            && (p_y >= 0 && p_y < HeightBlock) )
        {
            return ElementArray[p_x, p_y].IsMine;
        }
        return false;
    }

    public int NumberOfMines(int p_x, int p_y)   //지뢰 갯수 세기
    {
        int outcount = 0;

        //상단
        if (GetMineAt(p_x - 1, p_y + 1)) { ++outcount; }
        if (GetMineAt(p_x, p_y + 1)) { ++outcount; }
        if (GetMineAt(p_x + 1, p_y + 1)) { ++outcount; }
        //중단
        if (GetMineAt(p_x - 1, p_y)) { ++outcount; }
        if (GetMineAt(p_x + 1, p_y)) { ++outcount; }
        //하단
        if (GetMineAt(p_x - 1, p_y - 1)) { ++outcount; }
        if (GetMineAt(p_x, p_y - 1)) { ++outcount; }
        if (GetMineAt(p_x + 1, p_y - 1)) { ++outcount; }

        return outcount;
    }

    void SettingMines()   //지뢰 세팅
    {
        foreach (Vector2Int nextMine in MineCoords)
        {
            ElementArray[nextMine.x, nextMine.y].IsMine = true;
        }
    }

    void SetHint()
    {
        foreach (Vector2Int nextHint in HintList)
        {
            ElementArray[nextHint.x, nextHint.y].LeftClick();
        }
    }

    /*
    public void SetFlag()   //남은 지뢰 갯수
    {
        RemainMines = CountOfMines;
        
    }
    */

    public void UncoverNearby(int p_x, int p_y, bool[,] p_visited)   //지뢰가 아닌 주변 타일 공개
    {
        if ((p_x >= 0 && p_x < WidthBlock)
            && (p_y >= 0 && p_y < HeightBlock))
        {
            if (p_visited[p_x, p_y])
                return;

            int aroundcount = NumberOfMines(p_x, p_y);
            ElementArray[p_x, p_y].SetChangeTexture(aroundcount);
            if (aroundcount > 0)
                return;

            p_visited[p_x, p_y] = true;

            UncoverNearby(p_x + 1, p_y, p_visited);
            UncoverNearby(p_x - 1, p_y, p_visited);
            UncoverNearby(p_x, p_y + 1, p_visited);
            UncoverNearby(p_x, p_y - 1, p_visited);
        }

    }

    public bool IsFinished()   //게임 끝났는지 확인
    {
        foreach (Tile tile in ElementArray)
        {
            if (tile.IsCovered() && tile.IsMine)
            {
                return false;
            }
        }
        return true;
    }

    public void ShowMine()   //게임 클리어 시 지뢰 공개
    {
        foreach (Tile tile in ElementArray)
        {
            if (tile.IsMine)
            {
                tile.SetChangeTexture(10);
            }
        }
    }

    public void ResetGame()   //게임 초기화
    {
        foreach (Tile tile in ElementArray)
        {
            tile.ResetTile();
        }
        SetHint();

    }

    void Start()
    {
        TileGenarator();
        SettingMines();
        SetHint();
    }


}
