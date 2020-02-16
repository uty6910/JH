using System;
using AnimeRx;
using UniRx;
using UnityEngine;
using Motion = AnimeRx.Motion;

public enum PopUpInfo
{
    
}


public class PopUpManager : MonoBehaviour
{
    private static PopUpManager _instance = null;

    public static PopUpManager  instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PopUpManager)) as PopUpManager;
            }
            return _instance;
        }
    }

    public GameObject m_PopBackGround;

    private string[] m_popUpArr = new String[]
    {

    };

    public void addPopUpDialog(PopUpInfo _info,bool _bBackGround, PopUpObj _obj = null)
    {
        make_PopUp(_info, _bBackGround, _obj);
    }

    public void addTimePop(PopUpInfo _info,bool _bBackGround, PopUpObj _obj = null)
    {
        PopUpBase popUpBase = make_PopUp(_info, _bBackGround, _obj);

        GameObject popObj = popUpBase.gameObject;
        
        Anime.Play(DefineClass.Vector3One3, DefineClass.Vector3One, Motion.Uniform(DefineClass.PopUpPunchAnimVel))
            .Subscribe(_ => popObj.transform.localScale = _).AddTo(this);
        
        Observable.Interval(TimeSpan.FromSeconds(DefineClass.PopUpTime)).First().Subscribe(_ =>
        {
            m_PopBackGround.SetActive(false);
            popUpBase.BackPopUp();
        }).AddTo(this);
    }

    private PopUpBase make_PopUp(PopUpInfo _info, bool _bBackGround, PopUpObj _obj = null)
    {
        GameObject popObj = CreatePopUp(_info);
        m_PopBackGround.SetActive(_bBackGround);

        PopUpBase popUpBase = popObj.GetComponent<PopUpBase>();
        popUpBase.Set_Base(_info,_obj);
        popUpBase.BringIn();
        return popUpBase;
    }


    private GameObject CreatePopUp(PopUpInfo _info)
    {
        int _nPopUpIdx = (int) _info;

        if (_nPopUpIdx < m_popUpArr.Length)
        {
            return  Instantiate(Resources.Load(m_popUpArr[_nPopUpIdx]),transform) as GameObject;
        }
#if UNITY_EDITOR
        Debug.LogWarning("popuparr 등록 안됨");  
#endif
        return null;
    }
}
