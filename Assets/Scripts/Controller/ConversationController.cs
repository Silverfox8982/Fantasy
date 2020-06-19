
using UnityEngine;
using System;
using System.Collections;

public class ConversationController : MonoBehaviour
{
    // 이벤트 핸들러
    public static event EventHandler completeEvent;

    // Panel.cs의 SetPosition 으로 보낼 때 사용.
    // SetPositon은 키값으로 UI를 이동시킬 쌔 사용.
    const string ShowTop = "Show Top";
    const string ShowBottom = "Show Bottom";
    const string HideTop = "Hide Top";
    const string HideBottom = "Hide Bottom";


    // 왼쪽 대화상자 (상대방)
    [SerializeField]
    ConversationPanel leftPanel;
    // 오른쪽 대화상자 (본인)
    [SerializeField]
    ConversationPanel rightPanel;

    // 대화상자의 부모오브젝트.
    // 대화가 끝나면 Canvas를 Off 시켜서
    // 랜더링 최적화에 도움이 되도록 한다.
    Canvas canvas;

    // 대화 상자 코루틴
    IEnumerator conversation;

    // 애니메이션에 사용되는 것.
    // 다른 강좌에서 만든 스크립트를 재활용 중.
    Tweener transition;


    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();

        // 처음에 대화상자를 "Hide Bottom" 로 보낸다.
        if (leftPanel.panel.CurrentPosition == null)
        {
            leftPanel.panel.SetPosition(HideBottom, false);
        }

        if (rightPanel.panel.CurrentPosition == null)
        {
            rightPanel.panel.SetPosition(HideBottom, false);
        }

        // 부모 캔버스를 비활성화 시킨다.
        canvas.gameObject.SetActive(false);
    }

    // 대화상자 연출 시작
    // ConversationData 는 대화 내용들을 저장한다.
    public void Show(ConversationData data)
    {
        canvas.gameObject.SetActive(true);
        conversation = Sequence(data);
        conversation.MoveNext();        // 코루틴 시작.
    }

    public void Next()
    {
        // 대화가 끝났거나, 애니메이션이 진행중인지 확인
        if (conversation == null || transition != null)
            return;

        conversation.MoveNext();        // 코루틴 시작.
    }



    IEnumerator Sequence(ConversationData data)
    {
        // 해당 대화 이벤트에서 서로 주고받는
        // 대화 수 만큼 반복문을 돌림.
        for (int i = 0; i < data.list.Count; ++i)
        {
            // 대사마다 필요한 정보를 차례대로 불러온다.
            SpeakerData sd = data.list[i];

            // 대화상자의 위치를 지정한다. (왼쪽대화상자? 오른쪽 대화상자?)
            // 오른쪽은 Player, 왼쪽은 상대방
            ConversationPanel currentPanel
                = (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.MiddleLeft || sd.anchor == TextAnchor.LowerLeft) ? leftPanel : rightPanel;

            // ConversationPanel.Display 는
            // 대사 내용, 아바타 이미지, 화살표 연출 여부 등을
            // 처리한다.
            IEnumerator presenter = currentPanel.Display(sd);

            // Display 코루틴 재생
            // MoveNext() 는  yield return 을 받기 전까지만 재생시킨다.
            presenter.MoveNext();

            string show, hide;

            // 어떤 키값으로 UI 연출을 할 것인지 체크
            if (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.UpperCenter || sd.anchor == TextAnchor.UpperRight)
            {
                show = ShowTop;
                hide = HideTop;
            }
            else
            {
                show = ShowBottom;
                hide = HideBottom;
            }

            // 처음 대화 상자가 나오면 아래에서 위로 올라오는 연출이
            // 진행된다.
            currentPanel.panel.SetPosition(hide, false);
            MovePanel(currentPanel, show);

            // MoveNext(); 를 다시 전달 받기 전에는
            // 아래 코드를 진행하지 않는다.
            yield return null;

            // currentPanel.Display(sd);
            // 전달받은 SpeakerData의 모든 대화 내용을
            // 하나씩 출력하고 멈추고
            // 다시 MoveNext()가 호출될때까지 기다리는 것을
            // 반복한다.
            while (presenter.MoveNext())
            {
                yield return null;
            }


            // 대사 내용이 종료되면
            // 해당 대화 상자를 아래로 내리는 연출을 진행한다.
            MovePanel(currentPanel, hide);

            // EasingControl의 completedEvent 이벤트에 델리게이트를 등록시킨다.
            transition.completedEvent += delegate (object sender, EventArgs e) {
                conversation.MoveNext();
            };


            yield return null;
        }

        // 모든 대사가 종료되면 canvas를 비활성화
        canvas.gameObject.SetActive(false);

        // completedEvent 에 등록된
        // 델리게이트를 호출한다. (*씬을 바꾸는 함수가 등록될 예정) 
        if (completeEvent != null)
            completeEvent(this, EventArgs.Empty);
    }

    // UI 이동 연출
    void MovePanel(ConversationPanel obj, string pos)
    {
        transition = obj.panel.SetPosition(pos, true);
        transition.duration = 0.5f;
        transition.equation = EasingEquations.EaseOutQuad;
    }

}
