using UnityEngine;
using System;
using System.Collections;
using UniRx;
using System.Collections.Generic;

public enum FXINDEX
{
    FX_BTNCLICK,
    FX_MAX
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance = null;

    public static SoundManager instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
            }
            return _instance;
        }
    }
    public bool m_bFxSoundOn;

    public bool m_bBgSoundOn;

    private List<ReactiveProperty<bool>> m_pFxList = new List<ReactiveProperty<bool>>();

    private List<IDisposable> _subscriptions = new List<IDisposable>();
    // Use this for initialization
    public void Init()
    {
        for (int i = 0; i < (int)FXINDEX.FX_MAX; i++)
        {
            FXINDEX fxIdx = (FXINDEX)i;
            m_pFxList.Add(new ReactiveProperty<bool>(false));
            m_pFxList[i].Where(_ => _).Subscribe(_ =>
            {
                Debug.Log(fxIdx);
                m_pFxList[(int)fxIdx].Value = false;
            }).AddTo(_subscriptions);
        }
    }


    public void Release()
    {
        _subscriptions.DisposeAll();
        m_pFxList.Clear();
    }

    public void Fx_Play(FXINDEX sound)
    {
        if (m_bFxSoundOn == false) return;

        // FX 사운드 끝나면 (사운드 중첩 불가)
        m_pFxList[(int)sound].Value = true;
    }
}
