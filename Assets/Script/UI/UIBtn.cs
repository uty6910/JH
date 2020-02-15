using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using UniRx.Triggers;
using System.Collections;
using UnityEngine.Serialization;

public class UIBtn : MonoBehaviour
{
    public Button btn;
    public Text btnText;

    public void Start()
    {
        this.ObserveEveryValueChanged(_ => UIManager.instance.m_bTouch).Subscribe(_ =>
        {
            btn.interactable = _;
        }).AddTo(this);

        btn.OnClickAsObservable().Where(_ => SoundManager.instance.m_bFxSoundOn).Subscribe(_ =>
        {
            SoundManager.instance.Fx_Play(FXINDEX.FX_BTNCLICK);
        }).AddTo(this);
    }

    public void set_Btn(string _text)
    {
        btnText.text = _text;
    }
}
