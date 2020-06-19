
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(LayoutAnchor))]
public class Panel : MonoBehaviour
{
    // Panel 클래스에서만 사용되는 Position 클레스
    [Serializable]
    public class Position
    {
        public string name;

        // 정렬 기준
        public TextAnchor myAnchor;
        public TextAnchor parentAnchor;

        // 위치값
        public Vector2 offset;

        public Position(string name)
        {
            this.name = name;
        }

        public Position(string name, TextAnchor myAnchor, TextAnchor parentAnchor) : this(name)
        {
            this.myAnchor = myAnchor;
            this.parentAnchor = parentAnchor;
        }

        public Position(string name, TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset) : this(name, myAnchor, parentAnchor)
        {
            this.offset = offset;
        }
    } // Position


    // 애니메이션 처리할 UI의 위치값들 저장
    [SerializeField]
    List<Position> positionList;

    // positionList 을 담는 배열
    Dictionary<string, Position> positionMap;
    LayoutAnchor anchor;

    public Position CurrentPosition { get; private set; }
    public Tweener Transition { get; private set; }
    public bool InTransition { get { return Transition != null; } }

    void Awake()
    {
        anchor = GetComponent<LayoutAnchor>();
        positionMap = new Dictionary<string, Position>(positionList.Count);
        for (int i = positionList.Count - 1; i >= 0; --i)
            AddPosition(positionList[i]);
    }

    void Start()
    {
        if (CurrentPosition == null && positionList.Count > 0)
            SetPosition(positionList[0], false);
    }

    public Position this[string name]
    {
        get
        {
            if (positionMap.ContainsKey(name))
                return positionMap[name];
            return null;
        }
    }

    // 애니메이션 이동 좌표 하나 추가
    public void AddPosition(Position p)
    {
        positionMap[p.name] = p;
    }

    // 애니메이션 이동 좌표 하나 없앰
    public void RemovePosition(Position p)
    {
        if (positionMap.ContainsKey(p.name))
            positionMap.Remove(p.name);
    }


    // 키값으로 좌표이동할 때 호출
    public Tweener SetPosition(string positionName, bool animated)
    {
        return SetPosition(this[positionName], animated);
    }

    // Positon 클래스로 좌표이동할 때 호출
    public Tweener SetPosition(Position p, bool animated)
    {
        // positionList 의 첫번째 녀석의 Position값 저장
        CurrentPosition = p;

        if (CurrentPosition == null) return null;

        // 이동 중인 UI 가 있으면
        // 멈춘다.
        if (InTransition) Transition.easingControl.Stop();


        if (animated)
        {
            // 이동하면서 좌표 변경
            Transition = anchor.MoveToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return Transition;
        }
        else
        {
            // 순간적으로 좌표 변경
            anchor.SnapToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return null;
        }
    }
}

