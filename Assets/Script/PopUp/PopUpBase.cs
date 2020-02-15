using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBase : MonoBehaviour
{

    public PopUpInfo m_popUpInfo;

    protected Text m_title;

    protected Text m_body;

    protected UIBtn m_yesBtn;

    protected UIBtn m_noBtn;

    protected PopUpResultObj m_result;

    private PopUpObj _popUpObj;

    protected bool m_bInit;

    public virtual void Set_Base(PopUpInfo _info, PopUpObj _obj)
    {
        m_popUpInfo = _info;
        _popUpObj = _obj;
    }


    public void BringIn()
    {
        if (m_bInit == false)
        {
            m_yesBtn.btn.OnClickAsObservable().Subscribe(_ => TouchYesButton());
            m_noBtn.btn.OnClickAsObservable().Subscribe(_ => TouchNoButton());

            m_title.text = _popUpObj.title;
            m_body.text = _popUpObj.body;
            m_bInit = true;
        }
        gameObject.SetActive(true);
    }

    protected virtual void SendPopUpResult()
    {
        _popUpObj.subject.OnNext(m_result);
        _popUpObj.subject.OnCompleted();
    }

    public virtual void BackPopUp() { Destroy(gameObject);}

    protected virtual void TouchYesButton()
    {
        SendPopUpResult();
        BackPopUp();
    }

    protected virtual void TouchNoButton()
    {
        SendPopUpResult();
        BackPopUp();
    }

}