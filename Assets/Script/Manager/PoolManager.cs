using UnityEngine;
using System;
using UniRx;

public class PoolManager : MonoBehaviour
{
    
    private static PoolManager _instance = null;

    public static PoolManager instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PoolManager)) as PoolManager;
            }
            return _instance;
        }
    }
    
    public UnityEngine.UI.Button m_btn;
    public CubeObj m_pCubePrefab;
    // Start is called before the first frame update

    private PuzzleBaseItemPool _puzzlePool;

    public PuzzleBaseItemPool puzzlePool => _puzzlePool;
    
    void Start()
    {
        #region Sample
        var cubePool = new CubePool(transform, m_pCubePrefab);

        m_btn.OnClickAsObservable().Subscribe(_ =>
        {
            var cube = cubePool.Rent();
            Observable.Interval(TimeSpan.FromSeconds(5)).First().Subscribe(x =>
            {
                cubePool.Return(cube);
            });

        }).AddTo(this);
        #endregion

    }

    public void CreatePuzzleItemPool(Transform _trans, PuzzleBaseItemObj[] _array)
    {
       _puzzlePool = new PuzzleBaseItemPool(_trans, _array);   
    }
    
    
}
