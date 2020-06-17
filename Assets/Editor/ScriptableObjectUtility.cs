
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    // 스태틱으로 되어 다른 클래스에서 쉽게 접근할 수 있습니다.
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        // ScriptableObject 인스턴트를 만듭니다.
        T asset = ScriptableObject.CreateInstance<T>();

        // 저장할 파일의 경로를 말합니다.
        // Selection.activeObject 는 현재 선택된 경로를 말합니다.
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }

        // 지정된 경로 문자열에서 확장명을 반환합니다. (.exe, .asset 등 )
        else if (Path.GetExtension(path) != "")
        {
            // 확장명이 있으면 잘못된 경로입니다. (폴더가 아닌 파일)
            // 해당 파일이 있는 폴더 경로를 불러옵니다.
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        // 해당 경로가 중복되었으면 파일 뒤에 숫자를 붙여 
        // 유이크한 경로를 만들어냅니다.
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        // 애셋을 생성하고
        AssetDatabase.CreateAsset(asset, assetPathAndName);

        // 애셋을 저장하고
        AssetDatabase.SaveAssets();

        // 변경사항을 임포트합니다.
        AssetDatabase.Refresh();

        // 저장된 경로에 포커스를 맞춥니다.
        // Project 뷰가 해당 파일이 생긴 위치로 이동.
        EditorUtility.FocusProjectWindow();

        // 해당 폴더를 선택 상태로 만듭니다. (클릭한거처럼)
        Selection.activeObject = asset;
    }
}
