using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;

public class NetManager : MonoBehaviour
{

    private static NetManager _instance = null;

    public static NetManager instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(NetManager)) as NetManager;
            }
            return _instance;
        }
    }

    public ReactiveProperty<bool> m_bConnecting = new ReactiveProperty<bool>(false);

    public Queue<NetData> waitQueue = new Queue<NetData>();

    private NetData sendData;

    private void Start()
    {
        waitQueue.ObserveEveryValueChanged(_ => _.Count).Where(_ => _ > 0 && m_bConnecting.Value == false).Subscribe(_ =>
        {
            sendData = waitQueue.Dequeue();
            sendData.data.MakeJsonData();
            connectServer(sendData);
        });
    }

    /// <summary>
    /// subject 콜백
    /// </summary>
    private void connectServer(NetData netData)
    {
        m_bConnecting.Value = true;

        var stream = ObservableWWW.PostWWW(URLData.BaseServer, netData.data.form()).CatchIgnore((WWWErrorException ex) =>
         {
             Debug.LogError(ex.RawErrorMessage);
             if (ex.HasResponse)
             {
                 Debug.LogError(ex.StatusCode);
             }
             foreach (var item in ex.ResponseHeaders)
             {
                 Debug.LogError(item.Key + ":" + item.Value);
             }
             netData.Destroy();
         });

        stream.Subscribe(_ =>
        {
            m_bConnecting.Value = false;
            object dic_reciveObject = Json.Deserialize(_.text);

            Dictionary<string, object> data = (Dictionary<string, object>)dic_reciveObject;
            netData.subject.OnNext(data);
            netData.subject.OnCompleted();
        }).Dispose();
    }
}


public class PacketForm
{
    private string jsonData;

    private Hashtable dic_data = new Hashtable();

    //서버 재전송 문제 해결 시퀀스
    public static int gNSeqIndex = 1;

    WWWForm wwwForm;

#if UNITY_EDITOR
    private string oriJsonData;
#endif

    public PacketForm()
    {
    }

    public void AddData(string key, object pValue)
    {
        if (dic_data.ContainsKey(key))
        {
            dic_data[key] = pValue;
        }
        else
        {
            dic_data.Add(key, pValue);
        }
    }

    public void MakeJsonData(bool _bProtect = false)
    {
        if (dic_data.Count > 0)
        {
            jsonData = Json.Serialize(dic_data);
#if UNITY_EDITOR
            oriJsonData = jsonData;
#endif
        }
    }
    public WWWForm form()
    {
        if (dic_data.Count > 0)
        {
            if (wwwForm == null)
            {
                wwwForm = new WWWForm();
             
                wwwForm.AddField("Params", jsonData);
            }
        }

        return wwwForm;
    }

    public bool isPostData {
        get { return dic_data.Count > 0; }
    }

    public string sendJsonData {
        get { return jsonData; }
    }



}
public class NetData
{
    private PacketForm m_data;
    private NetRequest m_type;
    private ISubject<Dictionary<string, object>> _subject;

    public NetData(NetRequest type, PacketForm form)
    {
        m_data = form;
        m_type = type;

    }
    public NetData(NetRequest type, PacketForm form, ISubject<Dictionary<string, object>> del)
        : this(type, form)
    {
        _subject = del;
        //Debug.Log("del : NULL : " + (del == null));
        //Debug.Log("m_del : NULL : " + (m_del == null));
    }

    public PacketForm data {
        get { return m_data; }
    }

    public NetRequest type {
        get { return m_type; }
    }

    public ISubject<Dictionary<string, object>> subject {
        get { return _subject; }
        set { _subject = value; }
    }

    public void Destroy()
    {
        m_data = null;
        subject.OnCompleted();
    }
}
