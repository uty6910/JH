using UnityEngine;
using UniRx;
using System.Collections;

public class Main : MonoBehaviour
{
    private static Main _instance;

    // 리셋
    public ReactiveProperty<bool> m_bRestart = new ReactiveProperty<bool>(false);

    public static Main instance {
        get {
            Main tmpInst = FindObjectOfType(typeof(Main)) as Main;
            if (tmpInst != null)
                tmpInst.Awake();
            _instance = tmpInst;
            return _instance;
        }
    }

    private void Awake()
    {
        // 리셋
        m_bRestart.Where(_ => _).Subscribe(_ =>
        {
            m_bRestart.Value = false;
        });
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        SoundManager.instance.Init();
    }

}
