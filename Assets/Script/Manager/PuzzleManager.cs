using System.Collections.Generic;
using UnityEngine;

public enum PuzzleItemType
{
    
}

public enum CheckPuzzleDir
{
    NONE,
    Horizontal,  // x
    Vertical,    // y
}

public class PuzzleManager : MonoBehaviour
{
    private static PuzzleManager _instance = null;

    public static PuzzleManager instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PuzzleManager)) as PuzzleManager;
            }
            return _instance;
        }
    }

    public PuzzleBaseItemObj[] m_itemList;

    public PuzzleBaseItemObj[,] puzzleArray;

    public List<PuzzleBaseItemObj> m_pSelectPuzzleObj = new List<PuzzleBaseItemObj>();

    private int PuzzleWidth;
    private int PuzzleHeight;

    private void Start()
    {
        PoolManager.instance.CreatePuzzleItemPool(transform, m_itemList);
    }

    public void CreatePuzzleStage(int x, int y)
    {
        PuzzleWidth = x;
        PuzzleHeight = y;
        puzzleArray = new PuzzleBaseItemObj[999,999];
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                puzzleArray[y,x] = make_PuzzleItemObj(x,y);
            }
        }
    }

    private PuzzleBaseItemObj make_PuzzleItemObj(int x, int y)
    {
        int _type = Random.Range(0, m_itemList.Length);
        PuzzleBaseItemObj puzzleObj = PoolManager.instance.puzzlePool.Rent();
        puzzleObj.transform.localPosition = new Vector3(x * DefineClass.PuzzleXSize, y * DefineClass.PuzzleYSize, 0);
        PuzzleBaseItemObj _itemObj = puzzleObj.GetComponent<PuzzleBaseItemObj>();
        _itemObj.set_Item((PuzzleItemType)_type,x,y);
        _itemObj.BringIn();
        return _itemObj;
    }
    public bool CheckMovePuzzle(int _x, int _y)
    {
        if (m_pSelectPuzzleObj != null)
        {
            int x = Mathf.Abs(m_pSelectPuzzleObj[0].x - _x);
            int y = Mathf.Abs(m_pSelectPuzzleObj[0].y - _y);

            if (x > 1 || y > 1) return false;

            return true;
        }
        return false;
    }

    public void DestroyPuzzleItem()
    {
        List<PuzzleBaseItemObj> _pList = new List<PuzzleBaseItemObj>();
        for(int i = 0 ; i < m_pSelectPuzzleObj.Count; i++)
            CheckSamePuzzleItem(_pList, m_pSelectPuzzleObj[i],CheckPuzzleDir.NONE);
        m_pSelectPuzzleObj.Clear();
        if (_pList.Count > 2)
        {
            for (int i = 0; i < _pList.Count; i++) _pList[i].bDestroy.Value = true;
        }
    }

    private void CheckSamePuzzleItem(List<PuzzleBaseItemObj> _pList,PuzzleBaseItemObj _checkItem ,CheckPuzzleDir dir)
    {
        int x = _checkItem.x;
        int y = _checkItem.y;
        PuzzleItemType type = _checkItem.puzzleType;

        if (dir == CheckPuzzleDir.NONE || dir == CheckPuzzleDir.Horizontal)
        {
            if (_checkItem.x > 0 && _checkItem.x < PuzzleWidth - 1)
            {
                if (type == puzzleArray[y, x + 1].puzzleType && type == puzzleArray[y, x - 1].puzzleType)
                {
                    if (_pList.Contains(puzzleArray[y, x + 1]) == false)
                    {
                        if (y < PuzzleHeight - 1)
                            puzzleArray[y + 1, x + 1].MovePos.Value = puzzleArray[y, x + 1].GetPos();
                        else
                        {
                            make_PuzzleItemObj(y + 1, x + 1).MovePos.Value = puzzleArray[y, x + 1].GetPos();
                        }
                        _pList.Add(puzzleArray[y, x + 1]);
                        CheckSamePuzzleItem(_pList, puzzleArray[y, x + 1], CheckPuzzleDir.Horizontal);
                    }

                    if (_pList.Contains(puzzleArray[y, x - 1]) == false)
                    {
                        if (y < PuzzleHeight - 1)
                            puzzleArray[y + 1, x - 1].MovePos.Value = puzzleArray[y, x - 1].GetPos();
                        else
                        {
                            make_PuzzleItemObj(y + 1, x -1).MovePos.Value = puzzleArray[y, x - 1].GetPos();
                        }
                        _pList.Add(puzzleArray[y, x - 1]);
                        CheckSamePuzzleItem(_pList, puzzleArray[y, x - 1], CheckPuzzleDir.Horizontal);
                    }
                }
            }
        }

        if (dir == CheckPuzzleDir.NONE || dir == CheckPuzzleDir.Vertical)
        {
            if (_checkItem.y > 0 && _checkItem.y < PuzzleHeight - 1)
            {
                if (type == puzzleArray[y + 1, x].puzzleType && type == puzzleArray[y - 1, x].puzzleType)
                {
                    bool bAdd = false;
                    
                    if (_pList.Contains(puzzleArray[y - 1, x]) == false)
                    {
                        if (y + 2 < PuzzleHeight - 1)
                        {
                            bAdd = true;
                            puzzleArray[y + 2, x].MovePos.Value = puzzleArray[y - 1, x].GetPos();
                        }
                        else
                        {
                            make_PuzzleItemObj(y + 2, x).MovePos.Value = puzzleArray[y - 1, x].GetPos();
                        }
                        _pList.Add(puzzleArray[y - 1, x]);
                        CheckSamePuzzleItem(_pList, puzzleArray[y - 1, x], CheckPuzzleDir.Vertical);
                    }
                    
                    if (_pList.Contains(puzzleArray[y + 1, x]) == false)
                    {
                        if (y + (bAdd ? 3 : 2) < PuzzleHeight - 1)
                        {
                            puzzleArray[y + (bAdd ? 3 : 2), x].MovePos.Value = puzzleArray[y + 1, x].GetPos();
                        }
                        else
                        {
                            make_PuzzleItemObj(y + (bAdd ? 3 : 2),x).MovePos.Value = puzzleArray[y + 1, x].GetPos();
                        }
                        _pList.Add(puzzleArray[y + 1, x]);
                        CheckSamePuzzleItem(_pList, puzzleArray[y + 1, x], CheckPuzzleDir.Vertical);
                    }
                }
            }
        }
    }

    public PuzzleBaseItemObj GetPuzzleItemObj(int x, int y)
    {
        return puzzleArray[y, x];
    }
    
    

}
