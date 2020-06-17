
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// 제너릭과 이벤트핸들러
public class InfoEventArgs<T> : EventArgs
{
    public T info;

    // 생성자1
    public InfoEventArgs()
    {
        info = default(T);
        Debug.Log(info.ToString());
    }


    // 생성자2
    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}
