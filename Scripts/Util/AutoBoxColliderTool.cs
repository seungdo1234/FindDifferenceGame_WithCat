using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AutoBoxColliderTool : EditorWindow
{
    private List<SpriteRenderer> cachedSpriteRenderers = new List<SpriteRenderer>();
    private Vector2 scrollPosition;

    [MenuItem("Tools/Auto Box Collider 2D")]
    public static void ShowWindow()
    {
        GetWindow<AutoBoxColliderTool>("Auto Box Collider 2D");
    }

    private void OnGUI()
    {
        GUILayout.Label("Auto Box Collider Tool", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Selected to SpriteRenderers"))
        {
            AddSelectedToCache();
        }
        if (GUILayout.Button("Clear SpriteRenderers"))
        {
            cachedSpriteRenderers.Clear();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < cachedSpriteRenderers.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(cachedSpriteRenderers[i], typeof(SpriteRenderer), true);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                cachedSpriteRenderers.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Box Colliders"))
        {
            GenerateBoxColliders();
        }
    }

    private void AddSelectedToCache()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        foreach (GameObject obj in selectedObjects)
        {
            SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer renderer in renderers)
            {
                if (!cachedSpriteRenderers.Contains(renderer))
                {
                    cachedSpriteRenderers.Add(renderer);
                }
            }
        }
    }

    private void GenerateBoxColliders()
    {
        int processedCount = 0;
        foreach (SpriteRenderer spriteRenderer in cachedSpriteRenderers)
        {
            if (CreateColliderForSprite(spriteRenderer))
            {
                processedCount++;
            }
        }

        EditorUtility.DisplayDialog("Complete", $"Processed {processedCount} out of {cachedSpriteRenderers.Count} SpriteRenderers.", "OK");
    }

    private bool CreateColliderForSprite(SpriteRenderer spriteRenderer)
    {
        if (!spriteRenderer || !spriteRenderer.sprite)
        {
            return false;
        }

        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;

        if (!texture)
        {
            Debug.LogWarning($"Skipping {spriteRenderer.name}: Invalid texture.");
            return false;
        }
        
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter && !textureImporter.isReadable)
        {
            textureImporter.isReadable = true;
            AssetDatabase.ImportAsset(path);
        }

        Rect spriteRect = sprite.rect;
        Vector2 pivot = sprite.pivot;

        int startX = int.MaxValue, startY = int.MaxValue, endX = int.MinValue, endY = int.MinValue;
        
        for (int x = 0; x < (int)spriteRect.width; x++)
        {
            for (int y = 0; y < (int)spriteRect.height; y++)
            {
                Color pixel = sprite.texture.GetPixel((int)spriteRect.x + x, (int)spriteRect.y + y);
                if (pixel.a > 0)
                {
                    startX = Mathf.Min(startX, x);
                    startY = Mathf.Min(startY, y);
                    endX = Mathf.Max(endX, x);
                    endY = Mathf.Max(endY, y);
                }
            }
        }
        
        BoxCollider2D boxCollider = spriteRenderer.GetComponent<BoxCollider2D>();
        if (!boxCollider)
        {
            boxCollider = Undo.AddComponent<BoxCollider2D>(spriteRenderer.gameObject);
        }

        float pixelsPerUnit = sprite.pixelsPerUnit;

        float sizeX = (endX - startX + 1) / pixelsPerUnit;
        float sizeY = (endY - startY + 1) / pixelsPerUnit;
        Undo.RecordObject(boxCollider, "Adjust Box Collider");
        boxCollider.size = new Vector2(sizeX, sizeY);
        
        float offsetX = (startX + (endX - startX) / 2f - pivot.x) / pixelsPerUnit;
        float offsetY = (startY + (endY - startY) / 2f - pivot.y) / pixelsPerUnit;
        boxCollider.offset = new Vector2(offsetX, offsetY);
       
        if (textureImporter && textureImporter.isReadable)
        {
            textureImporter.isReadable = false;
        }

        EditorUtility.SetDirty(spriteRenderer.gameObject);
        
        return true;
    }
}