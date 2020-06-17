
using UnityEngine;
using System.Collections;
public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        Movement m = owner.currentUnit.GetComponent<Movement>();

        // 해당 유닛의 Move 방식에 따라 이동시킵니다.
        yield return StartCoroutine(m.Traverse(owner.currentTile));

        // 이동이 완료되면 SelectUnitState 상태로 변경합니다.
        owner.ChangeState<SelectUnitState>();
    }
}
