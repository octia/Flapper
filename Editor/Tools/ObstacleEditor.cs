using UnityEngine;
using UnityEditor;

public class ObstacleEditor : EditorWindow
{
    private int createdAssets = 0;
    private string assetName = "NewObstacle";

    private Obstacle obsTemplate;

    // Asset variables
    private int minScore = 0;

    private Color obstacleColor = Color.white;
    private bool duplicateColor = true;
    private Color obstacleTopColor = Color.white;
    private Color obstacleBotColor = Color.white;

    private Sprite obstacleTopSprite;
    private Sprite obstacleBotSprite;


    [MenuItem("Window/CreatorMenu/Obstacle")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditor>(true, "Obstacle Creator");
    }

    Vector2 scrollPos;


    private void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        if (!obsTemplate)
        {
            obsTemplate = ScriptableObject.CreateInstance<Obstacle>();
        }
        GUILayout.Label("Obstacle template, to import values (optional)");
        Obstacle importedObs = (Obstacle)EditorGUILayout.ObjectField("Obstacle Template", obsTemplate, typeof(Obstacle));
        if (importedObs != obsTemplate)
        {
            obsTemplate = importedObs;
            minScore = obsTemplate.minScore;
            obstacleTopColor = obsTemplate.obstacleColorTop;
            obstacleBotColor = obsTemplate.obstacleColorBot; 
            obstacleColor = obsTemplate.obstacleColorTop;
            if (obstacleTopColor == obstacleBotColor)
            {
                duplicateColor = true;
            }
            else
            {
                duplicateColor = false;
            }
            obstacleTopSprite = obsTemplate.obstacleSpriteTop;
            obstacleBotSprite = obsTemplate.obstacleSpriteBot;
        }

        GUILayout.Label("Obstacle Parameters:");
        string newAssetName = EditorGUILayout.TextField("Obstacle asset name", assetName);
        if (newAssetName != assetName)
        {
            assetName = newAssetName;
            createdAssets = 0;
        }
        if (minScore < 0)
        {
            minScore = 0;
        }
        minScore = EditorGUILayout.IntField("Minimum score", minScore);
        GUILayout.Space(15);

        GUILayout.Label("Obstacle colors:");
        duplicateColor = EditorGUILayout.Toggle("Use one color", duplicateColor);
        if (duplicateColor)
        {
            obstacleColor = EditorGUILayout.ColorField("Color: ",obstacleColor);
            obstacleTopColor = obstacleColor; 
            obstacleBotColor = obstacleColor;
        }
        else
        {
            obstacleTopColor = EditorGUILayout.ColorField("Top color: ", obstacleTopColor);
            obstacleBotColor = EditorGUILayout.ColorField("Bottom color: ", obstacleBotColor);
        }
        GUILayout.Space(15);
        GUILayout.Label("Obstacle sprites:");
        obstacleTopSprite = (Sprite)EditorGUILayout.ObjectField("Top sprite", obstacleTopSprite, typeof(Sprite));
        obstacleBotSprite = (Sprite)EditorGUILayout.ObjectField("Bottom sprite", obstacleBotSprite, typeof(Sprite));

        GUILayout.Space(25);
        if (GUILayout.Button("Create Object!"))
        {
            createdAssets++;
            CreateMyAsset();
        }

        GUILayout.EndScrollView();



    }

    void CreateMyAsset()
    {
        
        Obstacle asset = ScriptableObject.CreateInstance<Obstacle>();

        asset.obstacleColorTop = obstacleTopColor;
        asset.obstacleColorBot = obstacleBotColor;
        asset.obstacleSpriteBot = obstacleBotSprite;
        asset.obstacleSpriteTop = obstacleTopSprite;
        asset.minScore = minScore;
        AssetDatabase.CreateAsset(asset, "Assets/Scriptables/Obstacles/" + assetName + createdAssets.ToString() + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

}
