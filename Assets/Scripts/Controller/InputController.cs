
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    // 이동 이벤트 헨들러
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;

    // 발사 이벤트 헨들러
    public static event EventHandler<InfoEventArgs<int>> fireEvent;


    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };
    void Update()
    {
        int x = _hor.Update(); // -1, 0, 1 중에 하나가 반환된다.
        int y = _ver.Update();

        // 키 입력이 있다면
        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
            {

                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }
}


class Repeater
{
    const float threshold = 0.5f;
    const float rate = 0.25f;
    float _next;
    bool _hold;
    string _axis;

    // Input 매너저에 매핑된 값 중에 하나를 받아온다.
    // Horizontal, Vertical 같은..
    // 어떤 입력에 대한 처리인지
    public Repeater(string axisName)
    {
        _axis = axisName;
    }

    // MonoBehaviour를 상속받지 않았기에 유니티에서
    // 매프레임마다 호출하지 않는다.
    public int Update()
    {
        int retValue = 0;

        //GetAxisRaw 는 좌우 입력값에 따라 -1~1 사이의 값을 반환.
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

        // 좌 또는 우 입력이 있다면
        if (value != 0)
        {
            // _next 가 대기시간보다 작다면
            if (Time.time > _next)
            {
                retValue = value;

                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }

        // 좌우 입력에 의한 int 값을 반환한다.
        return retValue;
    }
}
