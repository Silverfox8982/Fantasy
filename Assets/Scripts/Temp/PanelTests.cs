
using UnityEngine;
using System.Collections;
public class PanelTests : MonoBehaviour
{
    Panel panel;
    const string Show = "Show";
    const string Hide = "Hide";
    const string Center = "Center";


    void Start()
    {
        panel = GetComponent<Panel>();
        // 처음 UI 가 있을 위치를 설정한다.
        Panel.Position centerPos = new Panel.Position(Center, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter);
        panel.AddPosition(centerPos);
    }


    void OnGUI()
    {
        // OnGUI 버튼
        if (GUI.Button(new Rect(10, 10, 100, 30), Show))
            panel.SetPosition(Show, true);
        if (GUI.Button(new Rect(10, 50, 100, 30), Hide))
            panel.SetPosition(Hide, true);

        // 센터로 이동할 때는 특수한 동작을 취하게 한다.
        if (GUI.Button(new Rect(10, 90, 100, 30), Center))
        {
            Tweener t = panel.SetPosition(Center, true);
            t.easingControl.equation = EasingEquations.EaseInOutBack;
        }
    }
}
