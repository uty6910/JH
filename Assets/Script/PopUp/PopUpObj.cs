using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;

public enum PopUpResult
{
    YES,
    NO
}
public class PopUpResultObj
{
    public PopUpResult result;
    public List<object> objList =new List<object>();
}
public class PopUpObj
{
    public ISubject<PopUpResultObj> subject;

    public string title;

    public string body;

    public List<object> popUpObj;
}
