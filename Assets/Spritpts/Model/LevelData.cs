using System.Collections.Generic;
using UnityEngine;


//스크립팅이 가능한 오브젝트라는 의미입니다.ScriptableObject는 게임오브젝트의 컴포넌트로 추가되지 않아도 독자적으로 데이터를 저장할 수 있습니다.

public class LevelData : ScriptableObject
{
    public List<Vector3> tiles;
}

