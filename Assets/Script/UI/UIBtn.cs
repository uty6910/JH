using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using UniRx.Triggers;
using System.Collections;
public class UIBtn : MonoBehaviour
{
    public Button m_btn;
    public Text m_text;

    public void Start()
    {
        this.ObserveEveryValueChanged(_ => UIManager.instance.m_bTouch).Subscribe(_ =>
        {
            m_btn.interactable = _;
        }).AddTo(this);

        m_btn.OnClickAsObservable().Where(_ => SoundManager.instance.m_bFxSoundOn).Subscribe(_ =>
        {
            SoundManager.instance.Fx_Play(FXINDEX.FX_BTNCLICK);
        }).AddTo(this);
    }

    public void set_Btn(string _text)
    {
        m_text.text = _text;
    }
}
