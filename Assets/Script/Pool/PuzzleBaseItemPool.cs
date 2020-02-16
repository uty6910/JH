using UniRx.Toolkit;
using UnityEngine;

public class PuzzleBaseItemPool : ObjectPool<PuzzleBaseItemObj>
{
    private PuzzleBaseItemObj[] _PrefabArray;

    private Transform _Parent;

    public PuzzleBaseItemPool(Transform _parent, PuzzleBaseItemObj[] _objArray)
    {
        _PrefabArray = _objArray;
        _Parent = _parent;
    }

    protected override PuzzleBaseItemObj CreateInstance()
    {
        var obj = GameObject.Instantiate(_PrefabArray[Random.Range(0, _PrefabArray.Length)]);
        obj.transform.SetParent(_Parent);
        return obj;
    }
}
