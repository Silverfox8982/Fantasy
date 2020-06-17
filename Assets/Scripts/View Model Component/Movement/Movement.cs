
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Movement : MonoBehaviour
{
    public int range;           //이동 범위
    public int jumpHeight;      //점프 높이
    protected Unit unit;        //이동하는 개체(Monster or Hero)
    protected Transform jumper;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        jumper = transform.Find("Jumper");
    }

    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);

        // 이동 범위 내 이동할 수 있는 타일들을 반환
        return retValue;
    }

    // Movement를 상속받는 클래스에서는
    // 해당 개체의 이동에 대해 강제적으로 정의하도록
    // 추상화 함수를 만들었습니다.
    public abstract IEnumerator Traverse(Tile tile);

    // 회전을 담당하는 함수
    protected virtual IEnumerator Turn(Directions dir)
    {
        // 다른 강좌에서 했던 내용입니다.
        // 차후 해당 강좌도 블로그에서 설명하겠습니다.
        // 각도 회전 등을 반환합니다.
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal
            (
                dir.ToEuler(),
                0.25f,
                EasingEquations.EaseInOutQuad
            );

        // 북쪽과 서쪽 사이를 회전할 때는 장치가 가장 효율적인 방법으로 회전하는 것처럼 보이도록
        // 예외를 만들어야 한다. (0 과 360도는 동일하게 본다.)
        if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
        {
            t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        }

        else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
        {
            t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        }

        unit.dir = dir;

        while (t != null) yield return null;
    }


    // 이동 범위 안의 타일인지 체크
    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }

    // 몬스터가 있거나 영웅이 있는 타일은 제외시킨다.
    protected virtual void Filter(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

}
