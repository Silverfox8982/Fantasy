
using UnityEngine;
using System.Collections;
public class AnchorTests : MonoBehaviour
{
    [SerializeField]
    bool animated;
    [SerializeField]
    float delay = 0.5f;

    IEnumerator Start()
    {
        LayoutAnchor anchor = GetComponent<LayoutAnchor>();
        while (true)
        {
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    // 정렬기준(좌, 우, 상, 하 조합)을 
                    // 반복문으로 돌립니다.
                    TextAnchor a1 = (TextAnchor)i;
                    TextAnchor a2 = (TextAnchor)j;
                    Debug.Log(string.Format("A1:{0}   A2:{1}", a1, a2));

                    if (animated)
                    {
                        // animated가 true 이면 UI 가 이동하면서 좌표를 변경합니다.
                        Tweener t = anchor.MoveToAnchorPosition(a1, a2, Vector2.zero);
                        while (t != null)
                            yield return null;
                    }
                    else
                    {
                        // animated가 false 이면 UI 가 순간이동합니다.
                        anchor.SnapToAnchorPosition(a1, a2, Vector2.zero);
                    }
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}
