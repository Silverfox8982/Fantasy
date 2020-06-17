
using UnityEngine;
using System.Collections;
public class TeleportMovement : Movement
{
    public override IEnumerator Traverse(Tile tile)
    {
        // 목적지를 해당 Unit 머물고 있는 타일로 갱신
        unit.Place(tile);

        // 순간이동할 때 회전과 스케일 값 변화로
        // 연출을 하네요.
        Tweener spin = jumper.RotateToLocal(new Vector3(0, 360, 0), 0.5f, EasingEquations.EaseInOutQuad);
        spin.easingControl.loopCount = 1;
        spin.easingControl.loopType = EasingControl.LoopType.PingPong;
        Tweener shrink = transform.ScaleTo(Vector3.zero, 0.5f, EasingEquations.EaseInBack);

        while (shrink != null) yield return null;

        // 목적지로 이동 완료.
        transform.position = tile.center;
        Tweener grow = transform.ScaleTo(Vector3.one, 0.5f, EasingEquations.EaseOutBack);
        while (grow != null) yield return null;
    }
}
