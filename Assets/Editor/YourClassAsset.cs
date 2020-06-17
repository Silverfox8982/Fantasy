
using UnityEngine;
using UnityEditor;
public class YourClassAsset
{
    // 유니티 상단 메뉴의 아래 경로에 
    // 아이템(버튼)을 추가시킨다.
    [MenuItem("Assets/Create/Conversation Data")]
    public static void CreateConversationData()
    {
        // 버튼이 눌리면 ScriptableObjectUtility.CreateAsset<T>를
        // 호출한다.
        ScriptableObjectUtility.CreateAsset<ConversationData>();
    }
}
