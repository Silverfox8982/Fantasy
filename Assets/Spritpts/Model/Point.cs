[System.Serializable]
// 포인트 구조체를 만들었습니다.
//타일 좌표는 월드 좌표와 다르게 타일의 배치된 순서에 따라 좌표가 생성됩니다.
//하지만, 유니티에서는 타일 좌표 기능이 없으므로 이것을 Point 구조체를 만들어서 표현할 것입니다.
public struct Point
{
    public int x;
    public int y;
    // Point의 생성자 입니다. 해당 구조체가 생성될 때 Point(int x, int y) 생성자가 호출됩니다.
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }


    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }
    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.x - p2.x, p1.y - p2.y);
    }
    public static bool operator ==(Point a, Point b)
    {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }


    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }
    public bool Equals(Point p)
    {
        return x == p.x && y == p.y;
    }
    public override int GetHashCode()
    {
        return x ^ y;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }
}

