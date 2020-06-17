
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LayoutAnchor : MonoBehaviour
{
    RectTransform myRT;
    RectTransform parentRT;


    void Awake()
    {
        // 부모와 자식의 RecTransform 좌표계를 가져온다.
        myRT = transform as RectTransform;
        parentRT = transform.parent as RectTransform;

        // 모든 UI는 최소 Canvas를 부모로 둬야한다.
        if (parentRT == null)
            Debug.LogError
("This component requires a RectTransform parent to work.", gameObject);
    }

    Vector2 GetPosition(RectTransform rt, TextAnchor anchor)
    {
        Vector2 retValue = Vector2.zero;

        // anchor 값에 따라 retValue의 x 값 저장
        switch (anchor)
        {
            // anchor의 x가 중앙인 경우
            case TextAnchor.LowerCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.UpperCenter:
                // rt의 중간값을 retValue.x 에 저장
                retValue.x += rt.rect.width * 0.5f;
                break;
            // anchor의 x 가 오른쪽인 경우
            case TextAnchor.LowerRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.UpperRight:
                // rt의 넓이(우측끝값)을 retValue.x 에 저장
                retValue.x += rt.rect.width;
                break;
        }

        // anchor 값에 따라 retValue의 y 값 저장
        switch (anchor)
        {
            // anchor의 y 가 중앙인 경우
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                // rt의 높이의 중앙값을 retValue.y 에 저장
                retValue.y += rt.rect.height * 0.5f;
                break;
            // anchor의 y 가 위쪽인 경우
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight:
                // rt의 높이 끝값을 retValue.y 에 저장
                retValue.y += rt.rect.height;
                break;
        }

        // 좌측끝과 아래쪽의 경우 0,0 이므로,
        // Switch 문을 하지 않음.
        return retValue;
    }


    public Vector2 AnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {

        // GetPosition 에서 Anchor와 이미지크기에 따른 Vector값을받음.
        Vector2 myOffset = GetPosition(myRT, myAnchor);
        Vector2 parentOffset = GetPosition(parentRT, parentAnchor);

        // 해당 UI의 anchor Min/Max 범위 중 Pivot이 위치하는 값.
        // anchor는 0~1의 값을 가진다.
        float anchorCenterX = Mathf.Lerp(myRT.anchorMin.x, myRT.anchorMax.x, myRT.pivot.x);
        float anchorCenterY = Mathf.Lerp(myRT.anchorMin.y, myRT.anchorMax.y, myRT.pivot.y);
        Vector2 anchorCenter = new Vector2(anchorCenterX, anchorCenterY);

        // 해당 UI의 anchor가 
        // 부모오브젝트의 어디쯤에 위치하는가
        float myAnchorOffsetX = parentRT.rect.width * anchorCenter.x;
        float myAnchorOffsetY = parentRT.rect.height * anchorCenter.y;
        Vector2 myAnchorOffset = new Vector2(myAnchorOffsetX, myAnchorOffsetY);

        // UI 의 가로 * 피벗 또는 UI 의 세로 * 피벗
        // 해상도 변경에 따른 UI 위치 변경할 때 사용되는 값
        float myPivotOffsetX = myRT.rect.width * myRT.pivot.x;
        float myPivotOffsetY = myRT.rect.height * myRT.pivot.y;
        Vector2 myPivotOffset = new Vector2(myPivotOffsetX, myPivotOffsetY);

        // 해당 UI가 월드상에 위치할 좌표를 Vector2로 받음
        Vector2 pos = parentOffset - myAnchorOffset - myOffset + myPivotOffset + offset;

        // 반올림
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        return pos;
    }

    // 이미지의 위치를 변경시킴 (순간적으로)
    public void SnapToAnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        myRT.anchoredPosition = AnchorPosition(myAnchor, parentAnchor, offset);
    }


    // 이미지의 위치를 변경시킴 (이동하는 것처럼)
    public Tweener MoveToAnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        return myRT.AnchorTo(AnchorPosition(myAnchor, parentAnchor, offset));
    }
}

