
using UnityEngine;


public abstract class BattleState : State
{
    protected Battle_Controller owner;

    // 아래 변수들은 BattleController가 가지고 있는 변수들입니다.
    public CameraRig cameraRig { get { return owner.cameraRig; } }
    public Board board { get { return owner.board; } }
    public LevelData levelData { get { return owner.levelData; } }
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }


    protected virtual void Awake()
    {
        owner = GetComponent<Battle_Controller>();
    }

    // InputController의 moveEvent와 fireEvent 핸들러에 함수를
    // 등록시킵니다.
    // AddListeners() 는 해당 State 상태로 변경될 때 호출됩니다.
    protected override void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.fireEvent += OnFire;
    }

    protected override void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }


    protected virtual void OnMove(object sender, InfoEventArgs<Point> e)
    {

    }

    protected virtual void OnFire(object sender, InfoEventArgs<int> e)
    {

    }

    // 선택된 타일 인디케이터(게임오브젝트)의 위치를 변경합니다.
    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
            return;
        pos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
    }
}
