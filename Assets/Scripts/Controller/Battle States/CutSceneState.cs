
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CutSceneState : BattleState
{
    ConversationController conversationController;
    ConversationData data;
    protected override void Awake()
    {
        base.Awake();
        conversationController = owner.GetComponentInChildren<ConversationController>();

        // Resources 폴더에 있는 파일을 로드합니다.
        // 이렇게 런타임 중에 파일을 로드하려면 Resources 폴더에 파일이 있어야합니다.
        data = Resources.Load<ConversationData>("Conversations/IntroScene");
    }
    protected override void OnDestroy()
    {
        // data 가 차지한 메모리를
        // 없앤다.
        base.OnDestroy();
        if (data)
            Resources.UnloadAsset(data);
    }

    public override void Enter()
    {
        // CutSceneState 상태가 되면
        // 대화 상자 연출이 진행된다.
        base.Enter();
        conversationController.Show(data);
    }
    protected override void AddListeners()
    {
        base.AddListeners();
        ConversationController.completeEvent += OnCompleteConversation;
    }
    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        ConversationController.completeEvent -= OnCompleteConversation;
    }
    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        // 클릭할 때마다 코루틴을 재생시킨다. 
        // yield return 에서 멈춘 코루틴이 그 뒤
        // 내용을 실행하게 만든다.
        base.OnFire(sender, e);
        conversationController.Next();
    }

    // 대화 이벤트가 종료되면 씬을 바꿀 함수.
    void OnCompleteConversation(object sender, System.EventArgs e)
    {
        owner.ChangeState<SelectUnitState>();
    }
}
