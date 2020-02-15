using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public enum UI_InfoType
{

}

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;

    public static UIManager instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            }
            return _instance;
        }
    }

    private string[] m_UIObjArr = new string[]
    {
        
    };

    public bool m_bTouch;

    private List<UIBase> m_pUIBaseList = new List<UIBase>();
    private Stack<UI_InfoType> m_pUIStack = new Stack<UI_InfoType>();

    public void Awake()
    {
        m_bTouch = true;
        NetManager.instance.m_bConnecting.ToReadOnlyReactiveProperty().Subscribe(_ => m_bTouch = !_).AddTo(this);
    }

    public void addUIMenu(UI_InfoType _infoType,Transform _trans, List<object> _obj = null)
    {
        make_UI(_infoType, _trans, _obj);
    }

    private void make_UI(UI_InfoType _InfoType, Transform _trans, List<object> _obj = null)
    {
        UIBase uIBase = null;
        if (m_pUIStack.Contains(_InfoType))
        {
            uIBase = FindUIBase(_InfoType);
            m_pUIStack.Push(_InfoType);
            uIBase.BringIn();
            return;
        }

        GameObject tempUI = create_UIObj(_InfoType, _trans);
        uIBase = tempUI.GetComponent<UIBase>();

        m_pUIBaseList.Add(uIBase);
        m_pUIStack.Push(_InfoType);

        uIBase.Set_BaseData(_InfoType, _obj);
        uIBase.BringIn();
    }

    private GameObject create_UIObj(UI_InfoType _infoType, Transform _parent)
    {
        if ((int)_infoType < m_UIObjArr.Length)
        {
            string _uiobjectPos = m_UIObjArr[(int)_infoType];
            return Instantiate(Resources.Load(_uiobjectPos),_parent == null ? this.transform : _parent) as GameObject;
        }
        return null;
    }

    public UIBase FindUIBase(UI_InfoType _infoType)
    {
        for(int i =0; i < m_pUIBaseList.Count; i++)
        {
            if(m_pUIBaseList[i].uIInfoType == _infoType)
            {
                return m_pUIBaseList[i];
            }
        }
#if UNITY_EDITOR
        Debug.LogWarning("uiarr 등록 안됨");        
#endif

        return null;
    }

}
