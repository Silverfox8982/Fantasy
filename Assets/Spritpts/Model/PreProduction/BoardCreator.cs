
using UnityEngine;
using UnityEditor;

// 파일을 저장하거나, 복사, 삭제, 이동 관련 기능을 사용할 때 
// 필요한 네임스페이스 입니다. 
using System.IO;
using System.Collections.Generic;

public class BoardCreator : MonoBehaviour
{
    //Tile 프리팹을 담는 변수
    [SerializeField]
    GameObject tileViewPrefab;

    // TileSelectIndicator 담는 변수(현재 위치)
    [SerializeField]
    GameObject tileSelectionIndicatorPrefab;

    //필드(보드) 의 범위입니다.
    //예) 타일의 층수는 height 보다 클 수 없습니다.
    [SerializeField]
    int width = 10;
    [SerializeField]
    int depth = 10;
    [SerializeField]
    int height = 8;

    //타일을 배치할 위치 정보를 담습니다.
    //유니티 에디터에서 사용자가 정의하는 좌표이다.
    [SerializeField]
    Point pos;


    // 불러오기로 사용할 LevelData를 담는 변수. 
    // LevelData 는 ScriptableObject 상속받았으며, 
    // Vector 타입의 배열 변수가 존재한다.
    [SerializeField]
    LevelData levelData;


    // 타일 배치 정보(좌표, 타일클래스)를 담는 Dictionany 배열
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();


    // 타일을 배치할 위치를 표시하는 tileSelectionIndicatorPrefab 의 현재 좌표를 반환합니다.
    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    Transform _marker;


    //단일로 타일을 추가할 때 처음 호출되는 함수
    public void Grow()
    {
        GrowSingle(pos);
    }

    // 단일로 타일을 제거할 때 처음 호출되는 함수
    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    // 랜덤 범위 내 다수의 타일을 생성할 때 
    // 처음 호출되는 함수
    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }

    // 랜덤 범위 내 다수의 타일을 감소 또는 제거할 때
    // 처음 호출되는 함수
    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    // tileSelectionIndicatorPrefab 의 위치를 변경할 때 호출된다.
    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    // 필드를 무의 상태로 만들어준다.
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }


    // 현재 배치 정보를 저장합니다.
    public void Save()
    {
        // 저장할 경로를 string 타입으로 저장합니다.
        string filePath = Application.dataPath + "/Resources/Levels";

        // 디렉토리의 filePath 경로가 있는지 확인합니다.
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();


        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(tiles.Count);

        // tiles 의 타일의 좌표값을 LevelData board 에 저장합니다.
        foreach (Tile t in tiles.Values)
            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));

        // 파일을 저장합니다.
        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    // LevelData의 좌표로 타일들을 배치합니다.
    public void Load()
    {
        Clear();
        if (levelData == null)
            return;

        foreach (Vector3 v in levelData.tiles)
        {
            Tile t = Create();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }

    // 랜덤 사각형을 반환합니다.
    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, depth);
        int w = UnityEngine.Random.Range(1, width - x + 1);
        int h = UnityEngine.Random.Range(1, depth - y + 1);
        return new Rect(x, y, w, h);
    }

    // 랜덤 사각형 범위 내에 타일을 채워놓습니다.
    void GrowRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    // 랜덤 사각형 범위 내의 타일을 감축시킵니다.
    void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    // 타일을 생성시킵니다.
    Tile Create()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    // tiles 배열에 P 좌표의 타일 있는지 확인하고
    // 있으면 해당 타일을 반환합니다.
    // 없으면 생성한 뒤에 생성한 타일을 반환합니다.
    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();

        // Tile.Load 는 타일의 좌표와 스케일 값을 조정
        Tile.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    // 타일을 높일지 말지 결정합니다.
    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < height)
            t.Grow();
    }

    // 타일을 감축시키거나 삭제
    void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile t = tiles[p];
        t.Shrink();

        if (t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    // 경로(폴더)를 만듭니다.
    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");

        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }
}


