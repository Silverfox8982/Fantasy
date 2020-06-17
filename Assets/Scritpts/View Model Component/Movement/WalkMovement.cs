
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement
{
    protected override bool ExpandSearch(Tile from, Tile to)
    {
        // 점프 높이보다 두 타일 사이의 높이가 높으면 건너띕니다.
        if ((Mathf.Abs(from.height - to.height) > jumpHeight))
            return false;

        // 타일에 다른 대상이 있으면 건너 뛴다.
        if (to.content != null)
            return false;

        // base.ExpandSearch에서는 이동거리를 체크한다.
        return base.ExpandSearch(from, to);
    }

    public override IEnumerator Traverse(Tile tile)
    {
        // 유닛이 머물고 있는 타일 정보를 이동하려는 위치로 갱신한다.
        unit.Place(tile);


        List<Tile> targets = new List<Tile>();
        while (tile != null)
        {
            // targets의 0번에 tile을 추가한다.
            // targets에 있던 데이터들은 한칸씩 뒤로 밀린다.
            targets.Insert(0, tile);
            tile = tile.prev;
        }


        // 연속해서 각 지점으로 이동.
        for (int i = 1; i < targets.Count; ++i)
        {
            // targets[target.count-1] 이 최종 목적지이다.
            Tile from = targets[i - 1];
            Tile to = targets[i];

            // from 이 to를 바라보는 방향을 enum 값으로 반환한다.
            Directions dir = from.GetDirection(to);

            if (unit.dir != dir)
            {
                // unit가 바라보는 방향을 dir 회전시킨다.
                yield return StartCoroutine(Turn(dir));
            }

            if (from.height == to.height)
            {
                // 높이가 같으면 걷고
                yield return StartCoroutine(Walk(to));
            }

            else
            {
                // 높이가 다르면 뛴다.
                yield return StartCoroutine(Jump(to));
            }
        }
        yield return null;
    }

    IEnumerator Walk(Tile target)
    {
        Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }
    IEnumerator Jump(Tile to)
    {
        Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);

        Vector3 stepHeightVec = new Vector3(0, Tile.stepHeight * 2f, 0);
        float destinationTweener = tweener.easingControl.duration / 2f;

        Tweener t2
        = jumper.MoveToLocal(stepHeightVec, destinationTweener, EasingEquations.EaseOutQuad);
        t2.easingControl.loopCount = 1;
        t2.easingControl.loopType = EasingControl.LoopType.PingPong;

        while (tweener != null)
            yield return null;
    }
}
