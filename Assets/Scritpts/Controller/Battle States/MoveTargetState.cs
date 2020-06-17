
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTargetState : BattleState
{
    List<Tile> tiles;

    public override void Enter()
    {
        base.Enter();

        // 사용자가 Unit을 선택하면 MoveTargetState 상태가 되어
        // 이동 가능한 타일들의 색상을 변경한다.
        Movement mover = owner.currentUnit.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(board);
        board.SelectTiles(tiles);
    }

    public override void Exit()
    {
        base.Exit();

        // MoveTargetState 상태가 종료되면
        // 변경된 타일들의 색상을 원래대로 변경한다.
        board.DeSelectTiles(tiles);
        tiles = null;
    }


    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        // 클릭한 타일로 이동시킵니다.
        if (tiles.Contains(owner.currentTile))
            owner.ChangeState<MoveSequenceState>();
    }
}
