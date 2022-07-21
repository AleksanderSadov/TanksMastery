using UnityEditor;

[CustomEditor(typeof(LoadSceneButton), true)]
public class ScenePickerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var picker = target as LoadSceneButton;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.scenePath);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Load Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var newName = newScene.name;
            var scenePathProperty = serializedObject.FindProperty("scenePath");
            var sceneNameProperty = serializedObject.FindProperty("sceneName");
            scenePathProperty.stringValue = newPath;
            sceneNameProperty.stringValue = newName;
        }
        serializedObject.ApplyModifiedProperties();
    }
}