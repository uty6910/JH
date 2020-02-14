using UnityEngine;
using System;
using UniRx;

public class PoolManager : MonoBehaviour
{
    public UnityEngine.UI.Button m_btn;
    public CubeObj m_pCubePrefab;
    // Start is called before the first frame update
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
}
