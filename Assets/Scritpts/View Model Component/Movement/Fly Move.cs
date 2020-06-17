
using UnityEngine;
using System.Collections;
public class FlyMovement : Movement
{
    public override IEnumerator Traverse(Tile tile)
    {
        // 시작 타일과 대상 타일의 거리를 저장합니다.
        float xMathfPow = Mathf.Pow(tile.pos.x - unit.tile.pos.x, 2);
        float yMathfPow = Mathf.Pow(tile.pos.y - unit.tile.pos.y, 2);

        float dist = Mathf.Sqrt(xMathfPow + yMathfPow);
        unit.Place(tile);

        // 지상 타일과 부딪치지 않을 정도의 높이를 지정
        float y = Tile.stepHeight * 10;
        float duration = (y - jumper.position.y) * 0.5f;
        Vector3 moveToPosition = new Vector3(0, y, 0);

        Tweener tweener
            = jumper.MoveToLocal(moveToPosition, duration, EasingEquations.EaseInOutQuad);

        while (tweener != null) yield return null;

        // 날아가는 방향을 바라보게 만든다.
        Directions dir;
        Vector3 toTile = (tile.center - transform.position);

        // 각도가 많이 꺽인 쪽이 이동하는 방향이다. (생각해보면 그렇다.)
        if (Mathf.Abs(toTile.x) > Mathf.Abs(toTile.z))
            dir = toTile.x > 0 ? Directions.East : Directions.West;
        else
            dir = toTile.z > 0 ? Directions.North : Directions.South;
        yield return StartCoroutine(Turn(dir));


        // 이동시킨다.
        duration = dist * 0.5f;
        tweener = transform.MoveTo(tile.center, duration, EasingEquations.EaseInOutQuad);
        while (tweener != null) yield return null;

        // 착륙
        duration = (y - tile.center.y) * 0.5f;
        tweener = jumper.MoveToLocal(Vector3.zero, 0.5f, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
    }
}

