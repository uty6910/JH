using UniRx.Toolkit;
using UnityEngine;

public class CubePool : ObjectPool<CubeObj>
{
    private CubeObj _Prefab;
    private Transform _Parent;

    public CubePool(Transform _Trans, CubeObj _obj)
    {
        _Prefab = _obj;
        _Parent = _Trans;
    }

    protected override CubeObj CreateInstance()
    {
        var e = GameObject.Instantiate(_Prefab);

        e.transform.localPosition = new Vector3(Random.Range(1,5), Random.Range(1,5),0);
        e.transform.SetParent(_Parent);

        return e;
    }
}
