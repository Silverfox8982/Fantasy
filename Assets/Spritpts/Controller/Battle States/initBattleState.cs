
using UnityEngine;
using System.Collections;
public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        // 타일맵을 로드한다.
        board.Load(levelData);

        // 현재 선택된 타일인디게이터(게임오브젝트) 의 좌표를 설정한다.
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        yield return null;

        // 현재 상태를 MoveTargetState로 변경한다.
        owner.ChangeState<MoveTargetState>();
    }
}

