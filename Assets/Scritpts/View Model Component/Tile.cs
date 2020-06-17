
using UnityEngine;

public class Tile : MonoBehaviour
{
    // 같은 위치에 타일을 두개 이상 배치할 때 Sale y 변화값
    // 이 경우 타일을 같은 위치에 쌓아두는 것이 아니라 
    // 오브젝트의 스케일값을 키워서 층을 만든다.
    public const float stepHeight = 0.25f;
    public GameObject content;
    public Point pos;

    public int height;
    public Tile prev;
    public int distance;

    public Vector3 center
    {
        get
        {
            return new Vector3(pos.x, height * stepHeight, pos.y);
        }
    }

    private void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    // 타일을 생성한다. 
    // 한번에 1층씩 생성한다. 
    public void Grow()
    {
        height++;
        Match();
    }

    // 타일을 제거한다.
    // 한번에 1층씩 제거한다.
    public void Shrink()
    {
        height--;
        Match();
    }


    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }
    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
