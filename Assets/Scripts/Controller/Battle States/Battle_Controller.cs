
using UnityEngine;
using System.Collections;

public class Battle_Controller : StateMachine
     // 임시 코드

{
    // 메인카메라의 컴포넌트
    // 카메라가 tileSelectionIndicator를 따라다니게 한다.
    public CameraRig cameraRig;

    // LevelData의 정보를 토대로 타일맵을 불러오는 클래스
    public Board board;

    // 타일맵의 타일들의 좌표 정보가 저장된 LeveData
    public LevelData levelData;

    // 선택된 타일 인디게이터
    public Transform tileSelectionIndicator;

    // tileSelectionIndicator의 좌표를 표시한다.
    public Point pos;

    public GameObject heroPrefab;
    public Unit currentUnit;
    public Tile currentTile { get { return board.GetTile(pos); } }

    void Start()
    {
        // StateMachine.cs의 ChangeState를 호출해서 InitBattleState로 상태를 변경시킨다.
        ChangeState<InitBattleState>();
    }
}
