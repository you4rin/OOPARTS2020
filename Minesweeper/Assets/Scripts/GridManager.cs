using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{

    public GameObject CloneBlock;
    public GameObject Border;

    public int WidthBlock = 10;
    public int HeightBlock = 10;
    protected int RemainMines;

    public Tile[,] ElementArray;
    public List<Vector2Int> MineCoords = new List<Vector2Int>();
    public List<Vector2Int> HintList = new List<Vector2Int>();

    public List<Vector2Int> HBorderCoords = new List<Vector2Int>(); //Horizental borders
    public List<Vector2Int> VBorderCoords = new List<Vector2Int>(); //Vertical borders

    public enum GameState
    {
        gaming,
        cleared,
        failed
    }
    public GameState state;


    void Awake()
    {
        ElementArray = new Tile[WidthBlock, HeightBlock];
    }

    void MoveToCenter()
    {
        Vector3 parent_transform = this.transform.parent.position;
        Vector3 center_pivot = new Vector3(-((WidthBlock - 1) / 2.0f), -((HeightBlock - 1) / 2.0f), 0);
        Vector3 parent_scale = this.transform.parent.localScale;
        center_pivot.x *= parent_scale.x;
        center_pivot.y *= parent_scale.y;
        this.transform.position = center_pivot + parent_transform;

    }

    Tile CloneTile(int p_x, int p_y)   //타일 복제
    {
        GameObject CopyObj = GameObject.Instantiate(CloneBlock.gameObject);
        CopyObj.transform.SetParent(this.transform);

        Vector2 temppos = new Vector2(p_x, p_y);
        CopyObj.transform.localPosition = temppos;
        CopyObj.transform.localScale = CopyObj.transform.lossyScale;
        CopyObj.name = "CloneBlock_" + p_x.ToString() + "_" + p_y.ToString();

        return CopyObj.GetComponent<Tile>();
    }

    void CloneVerticalBorder(int x, int y)
    {
        GameObject CopyObj = GameObject.Instantiate(Border.gameObject);
        CopyObj.transform.SetParent(this.transform);

        Vector3 temppos = new Vector3(x + 0.5f, y, 0);
        CopyObj.transform.localPosition = temppos;
        CopyObj.transform.localScale = CopyObj.transform.lossyScale;
        CopyObj.name = "CloneVBorder_" + x.ToString() + "_" + y.ToString();
    }

    void CloneHorizontalBorder(int x, int y)
    {
        GameObject CopyObj = GameObject.Instantiate(Border.gameObject);
        CopyObj.transform.SetParent(this.transform);

        Vector3 temppos = new Vector3(x, y + 0.5f, 0);
        CopyObj.transform.localPosition = temppos;
        CopyObj.transform.localScale = CopyObj.transform.lossyScale;

        Vector3 temprot = new Vector3(0, 0, 90);
        CopyObj.transform.eulerAngles = temprot;
        CopyObj.name = "CloneHBorder_" + x.ToString() + "_" + y.ToString();
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

    void BorderGenerator()
    {
        for (int x=0; x < WidthBlock; ++x)
        {
            CloneHorizontalBorder(x, -1);
            CloneHorizontalBorder(x, HeightBlock - 1);
        }
        for (int y = 0; y < HeightBlock; ++y)
        {
            CloneVerticalBorder(-1, y);
            CloneVerticalBorder(WidthBlock - 1, y);
        }
        foreach (var next_coord in HBorderCoords)
        {
            CloneHorizontalBorder(next_coord.x, next_coord.y);
        }
        foreach (var next_coord in VBorderCoords)
        {
            CloneVerticalBorder(next_coord.x, next_coord.y);
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

            if (ElementArray[p_x, p_y].IsMarked()) //잘못 표시한 깃발은 그대로 놔두기
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
            if (tile.IsCovered()) return false;
            if (tile.IsMarked() && tile.IsMine == false) return false;

        }
        state = GameState.cleared;
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
        state = GameState.gaming;

    }

    void Start()
    {
        MoveToCenter();
        TileGenarator();
        BorderGenerator();
        SettingMines();
        SetHint();
        state = GameState.gaming;
    }

}
