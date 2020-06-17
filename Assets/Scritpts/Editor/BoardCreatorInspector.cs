
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Editor를 상속받기 위한 네임스페이스
using UnityEditor;

// Editor 타겟 대상
[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor
{
    public BoardCreator current
    {
        get
        {
            // target 은 BoardCreator를 의미한다.
            return (BoardCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        // Inspector 에 버튼 몇개를 추가하고 싶을 때
        // 호출하는 유니티에서 지원하는 함수.
        DrawDefaultInspector();

        // BoardCreator 컴포넌트의 Inpector 뷰에
        // GUI 버튼이 생성된다.
        if (GUILayout.Button("Clear"))
            // BoardCreator 가 current 이다.
            // 예) Clear 버튼을 누르면 BoardCreator()가 호출된다.
            current.Clear();
        if (GUILayout.Button("Grow"))
            current.Grow();
        if (GUILayout.Button("Shrink"))
            current.Shrink();
        if (GUILayout.Button("Grow Area"))
            current.GrowArea();
        if (GUILayout.Button("Shrink Area"))
            current.ShrinkArea();
        if (GUILayout.Button("Save"))
            current.Save();
        if (GUILayout.Button("Load"))
            current.Load();

        if (GUI.changed)
            current.UpdateMarker();
    }
}

