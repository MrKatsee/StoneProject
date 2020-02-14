using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public Sprite cursor;
    public List<Sprite> tiles;
}

[CustomEditor(typeof(MapEditor))]
public class MapEditorCustomEditor : Editor
{
    static float lineSize = 0.25f;

    bool editMode = false;

    Sprite selectedSprite = null;
    Sprite cursor = null;

    GameObject cursorObject;

    private void OnSceneGUI()
    {
        if (!editMode)
        {
            return;
        }

        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        mousePosition = ray.origin;
        mousePosition = RoundWithUnit(mousePosition);

        //Debug.Log(mousePosition);

        //Debug.Log(mousePosition);

        if (cursorObject == null)
        {
            cursorObject = Instantiate(Resources.Load<GameObject>("Prefabs/Cursor"), mousePosition, Quaternion.identity);
        }

        cursorObject.transform.position = mousePosition;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            GameObject tileObject = Instantiate(Resources.Load<GameObject>("Prefabs/Tile"), mousePosition, Quaternion.identity);
            tileObject.transform.parent = ((MapEditor)target).transform;
            tileObject.GetComponent<SpriteRenderer>().sprite = selectedSprite;
        }
    }

    private Vector2 RoundWithUnit(Vector2 vec)
    {
        float x = vec.x;
        float y = vec.y;

        int x_int = (int)(x * 4f);
        int y_int = (int)(y * 4f);

        x = (float)x_int / 4f;
        y = (float)y_int / 4f;

        //Debug.Log(x + " " + y);

        return new Vector2(x, y);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mapEditor = target as MapEditor;

        cursor = mapEditor.cursor;

        Texture button_tex;
        GUIContent button_tex_con;

        if (selectedSprite != null)
        {
            button_tex = ConvertSpriteToTexture(selectedSprite);
            button_tex_con = new GUIContent(button_tex);

            GUILayout.Box(button_tex_con, GUILayout.Width(50), GUILayout.Height(50));
        }

        if (!editMode)
        {
            if (GUILayout.Button("Edit Mode On"))
            {
                ActiveEditorTracker.sharedTracker.isLocked = true;

                editMode = true;

                //Debug.Log(mapEditor.tiles[0].rect);

                //mapEditor.CreateTilesToGUIStyles();
            }
        }
        else if (editMode)
        {
            if (GUILayout.Button("Edit Mode Off"))
            {
                ActiveEditorTracker.sharedTracker.isLocked = false;

                editMode = false;

                DestroyImmediate(cursorObject);
            }

            foreach(var sprite in mapEditor.tiles)
            {
                button_tex = ConvertSpriteToTexture(sprite);
                button_tex_con = new GUIContent(button_tex);

                if (GUILayout.Button(button_tex_con, GUILayout.Width(50), GUILayout.Height(50)))
                {
                    selectedSprite = sprite;
                }
            }
        }
    }

    Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] colors = newText.GetPixels();
        Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                     (int)System.Math.Ceiling(sprite.textureRect.y),
                                                     (int)System.Math.Ceiling(sprite.textureRect.width),
                                                     (int)System.Math.Ceiling(sprite.textureRect.height));
        Debug.Log(colors.Length + "_" + newColors.Length);
        newText.SetPixels(newColors);
        newText.Apply();

        return newText;
    }

}
