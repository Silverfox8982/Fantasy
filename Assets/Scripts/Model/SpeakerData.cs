
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class SpeakerData
{
    public List<string> messages; // 대화 내용
    public Sprite speaker;        // 대화자 이미지
    public TextAnchor anchor;     // 대화 상자의 앵커값
}
