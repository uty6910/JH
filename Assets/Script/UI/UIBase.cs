using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBase : MonoBehaviour
{
    private UI_InfoType m_InfoType;

    public UI_InfoType uIInfoType => m_InfoType;

    protected bool m_bInit;
    protected List<object> m_objList = new List<object>(); 

    public virtual void Set_BaseData(UI_InfoType _InfoType, List<object> obj = null)
    {
        m_InfoType = _InfoType;
        m_objList = obj;
    }

    public virtual void BringIn() { gameObject.SetActive(true); }

    public virtual void Dismiss() { gameObject.SetActive(false); }

    public virtual void refreshValue() { }

    public virtual void BackUI() { }

    public virtual bool Check_BackKeyEvent() { return true; }
}
