using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    // Start is called before the first frame update

    public Sprite[] ChangeSpriteArray;
    public Sprite MineSprite;
    private SpriteRenderer m_SpriteRender;
    protected GridManager LinkGridManager;
    public Sprite FlagSprite;
    public Sprite QuestionSprite;
    const int NORMAL_SPRITE = 11;

    //지뢰 관련>>
    private bool m_IsMine = false;
    //public bool IsMine { get => m_IsMine; protected set => m_IsMine = value; }
    public bool IsMine
    {
        get { return m_IsMine; }
        set { m_IsMine = value; }
    }
    public void SetElementDatas(bool p_ismine)
    {
        m_IsMine = p_ismine;
    }
    //지뢰 관련<<

    public void SetChangeTexture(int P_index)   //그림 바꾸기
    {
        m_SpriteRender.sprite = ChangeSpriteArray[P_index];
    }

    public void LeftClick()   //좌클릭 후 작동
    {

        if (m_IsMine)   //게임오버
        {
            m_SpriteRender.sprite = MineSprite;

            Debug.LogFormat("게임오버");

        }
        else   //지뢰 갯수 찾기
        {
            int x = (int)this.transform.localPosition.x;
            int y = (int)this.transform.localPosition.y;
            SetChangeTexture( LinkGridManager.NumberOfMines(x, y) );
            //Debug.LogFormat("{0}", LinkGridManager.NumberOfMines(x, y));  //확인용

            LinkGridManager.UncoverNearby(x, y, new bool[LinkGridManager.WidthBlock, LinkGridManager.HeightBlock]);

        }
    }

    public void RightClick() //우클릭 후 작동
    {
        if (m_SpriteRender.sprite == ChangeSpriteArray[NORMAL_SPRITE])
        {
            m_SpriteRender.sprite = FlagSprite;
            return;
        }
        if (m_SpriteRender.sprite == FlagSprite)
        {
            m_SpriteRender.sprite = QuestionSprite;
            return;
        }
        if (m_SpriteRender.sprite == QuestionSprite)
        {
            SetChangeTexture(NORMAL_SPRITE);
            return;
        }
    }

    public bool IsCovered()   //타일 클릭 여부
    {
        return m_SpriteRender.sprite.texture.name == "tile-normal-1";
    }


    void OnEnable()
    {
        m_SpriteRender = this.GetComponent<SpriteRenderer>();
        LinkGridManager = GameObject.FindObjectOfType<GridManager>();

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) //Left button
        {
            LeftClick();
            if (LinkGridManager.IsFinished())   //스테이지 클리어
            {
                LinkGridManager.ShowMine();
                Debug.LogFormat("스테이지 클리어");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClick();
            if (LinkGridManager.IsFinished())   //스테이지 클리어
            {
                LinkGridManager.ShowMine();
                Debug.LogFormat("스테이지 클리어");
            }
        }
    }

}
