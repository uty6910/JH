using System;
using AnimeRx;
using UniRx;
using UnityEngine;
using Motion = AnimeRx.Motion;

public class PuzzleBaseItemObj : MonoBehaviour
{
    public ReactiveProperty<bool> bDestroy = new ReactiveProperty<bool>(false);
    public Vector3ReactiveProperty MovePos; 

    public PuzzleItemType puzzleType;

    public UIBtn m_btn;

    public GameObject m_selectObj;

    protected bool m_bInit;

    public int x;
    
    public int y;

    public virtual void set_Item(PuzzleItemType _type, int _x, int _y)
    {
        puzzleType = _type;
        x = _x;
        y = _y;
    }
    
    public virtual void BringIn()
    {
        if (m_bInit == false)
        {
            MovePos = new Vector3ReactiveProperty(GetPos());
            m_btn.btn.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    if (PuzzleManager.instance.m_pSelectPuzzleObj == null)
                    {
                        m_selectObj.SetActive(true);
                        PuzzleManager.instance.m_pSelectPuzzleObj.Add(this);
                    }
                    else if (PuzzleManager.instance.m_pSelectPuzzleObj.Contains(this) || PuzzleManager.instance.CheckMovePuzzle(x,y) == false)
                    {
                        m_selectObj.SetActive(false);
                        if(PuzzleManager.instance.m_pSelectPuzzleObj.Contains(this))
                            PuzzleManager.instance.m_pSelectPuzzleObj.Remove(this);
                    }
                    else if (PuzzleManager.instance.CheckMovePuzzle(x,y))
                    {
                        PuzzleManager.instance.m_pSelectPuzzleObj.Add(this);
                        m_selectObj.SetActive(false);
                        PuzzleChangeAnim(PuzzleManager.instance.GetPuzzleItemObj(x,y));
                    }
                }).AddTo(this);

            bDestroy.ToReadOnlyReactiveProperty().Where(_ => _).Subscribe(_ => DestroyPuzzleItem()).AddTo(this);
            MovePos.Where(_ => GetPos() != _).Subscribe(_ => PuzzleMoveAnim()).AddTo(this);
            m_bInit = true;
        }
        
        gameObject.SetActive(true);
    }

    public Vector3 GetPos(bool _bWorld = false)
    {
        return _bWorld ? transform.position : transform.localPosition;
    }

    protected virtual void PuzzleMoveAnim()
    {
        float _targetX = MovePos.Value.x / DefineClass.PuzzleXSize;
        float _targetY = MovePos.Value.y / DefineClass.PuzzleYSize;
        Vector3 targetPos = MovePos.Value;
        targetPos.x = x;
        targetPos.y += DefineClass.PuzzleYSize;
        if (targetPos.y != GetPos().y)
        {
            Anime.Play(GetPos(), targetPos, Motion.Uniform(DefineClass.PuzzleDropAnimVel * (y - _targetY)))
                .SubscribeToLocalPosition(gameObject).AddTo(this);
        }

        Observable.Interval(TimeSpan.FromSeconds(DefineClass.PuzzleDropAnimVel * (_targetY - y))).First().Subscribe(x =>
        {
            Anime.Play(GetPos(), MovePos.Value, Motion.Uniform(DefineClass.PuzzleDropAnimVel))
                .Subscribe(_ =>
                {
                    transform.localPosition = _;
                    if (GetPos() == MovePos.Value)
                    {
                        PuzzleManager.instance.puzzleArray[y, x] = null;
                        x = (int)_targetX;
                        y = (int)_targetY;
                        PuzzleManager.instance.puzzleArray[y, x] = this;
                    }
                }).AddTo(this);
        }).AddTo(this);


    }
    protected virtual void PuzzleChangeAnim(PuzzleBaseItemObj _target)
    {
        Anime.Play(transform.localPosition, _target.GetPos(), Motion.Uniform(DefineClass.PuzzleItemChangeAnimVel))
            .Subscribe(_ =>
            {
                transform.localPosition = _;
                if (transform.localPosition == _target.transform.localPosition)
                {
                    PuzzleManager.instance.DestroyPuzzleItem();
                }
            });
    }

    public virtual void DestroyPuzzleItem()
    {
        Anime.Play(transform.localScale, DefineClass.Vector3Zero, Motion.Uniform(DefineClass.PuzzleItemDestroyAnimVel))
            .Subscribe(_ =>
            {
                transform.localScale = _;
                if (transform.localScale == DefineClass.Vector3Zero)
                {
                    PuzzleManager.instance.puzzleArray[y, x] = null;
                    PoolManager.instance.puzzlePool.Return(this);
                }
            });
    }
}
