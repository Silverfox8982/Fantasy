
using UnityEngine;
using System.Collections;

public abstract class State : MonoBehaviour
{

    // 상태가 시작될 때 호출한다.
    public virtual void Enter()
    {
        AddListeners();
    }

    // 상태가 종료될 때 호출한다.
    public virtual void Exit()
    {
        RemoveListeners();
    }

    // 안전하게 Listener를 제거하기 위한 용도.
    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }


    // 이벤트핸들러에 이벤트를 추가한다.
    protected virtual void AddListeners()
    {

    }

    // 이벤트핸들러에 이벤트를 제거한다.
    protected virtual void RemoveListeners()
    {

    }
}
