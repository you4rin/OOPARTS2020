using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite[] ChangeSpriteArray;
    private SpriteRenderer m_SpriteRender;
    protected GridManager LinkGridManager;

    //지뢰 관련>>
    [SerializeField]
    private bool m_IsMine = false;
    //public bool IsMine { get => m_IsMine; protected set => m_IsMine = value; }
    public bool IsMine
    {
        get { return m_IsMine; }
        protected set { m_IsMine = value; }
    }
    public void SetElementDatas(bool p_ismine)
    {
        m_IsMine = p_ismine;
    }
    //지뢰 관련<<

    void SetChangeTexture(int P_index)   //그림 바꾸기
    {
        m_SpriteRender.sprite = ChangeSpriteArray[P_index];
    }

    void OnMouseDown()   //클릭 후 작동
    {

        if (m_IsMine)   //게임오버
        {
            
        }
        else   //지뢰 갯수 찾기
        {
            int x = (int)this.transform.localPosition.x;
            int y = (int)this.transform.localPosition.y;
            SetChangeTexture( LinkGridManager.NumberOfMines(x, y) );
            //Debug.LogFormat("{0}", LinkGridManager.NumberOfMines(x, y));  //확인용
        }
    }

    void Start()
    {
        m_SpriteRender = this.GetComponent<SpriteRenderer>();
        LinkGridManager = GameObject.FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
