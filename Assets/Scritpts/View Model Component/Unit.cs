
using UnityEngine;
using System.Collections;
public class Unit : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Directions dir;
    public void Place(Tile target)
    {
        // 이전에 선택한 tile이 null로 초기화 안되었다면
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    // 해당 게임 오브젝트의 Position 과 EulerAngles 값을 변경합니다.
    public void Match()
    {
        transform.localPosition = tile.center;

        // Vector3(x, y, z)로 Rotation을 구하는 겁니다.
        transform.localEulerAngles = dir.ToEuler();
    }
}
