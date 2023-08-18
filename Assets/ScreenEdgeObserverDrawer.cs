using System;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class ScreenEdgeObserverDrawer : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField, Range(0f, 1f)] private float _leftRect = 0.1f;
    [SerializeField, Range(0f, 1f)] private float _rightRect = 0.1f;
    [SerializeField, Range(0f, 1f)] private float _upperRect = 0.1f;
    [SerializeField, Range(0f, 1f)] private float _bottomRect = 0.1f;
    [SerializeField, Range(0f, 1f)] private float _sphereRadius = 0.1f;
    [SerializeField] Vector2 _sphereCenter = Vector2.zero;

    private Texture2D _sphereTexture;

    #region MonoBehaviour

    private void Awake()
    {
        _sphereTexture = GetSphereTexture(Color.blue.WithAlpha(0.2f));
    }

    private void OnValidate()
    {
        _sphereTexture = GetSphereTexture(Color.blue.WithAlpha(0.2f));
    }

    #endregion


    private void OnGUI()
    {
        Texture2D texture = GetRectTexture(Color.red.WithAlpha(0.2f));

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float sphereWidth = screenWidth * _sphereRadius;

        float leftRectSize = screenWidth * _leftRect;
        float rightRectSize = screenWidth * _rightRect;
        float upperRectSize = screenHeight * _upperRect;
        float bottomRectSize = screenHeight * _bottomRect;

        int height = Screen.height;
        int width = Screen.width;
        Rect leftRect = new Rect(0, 0, leftRectSize, height);
        Rect rightRect = new Rect(width - rightRectSize, 0, rightRectSize, height);
        Rect upperRect = new Rect(leftRectSize, 0, width - leftRectSize - rightRectSize, upperRectSize);
        Rect bottomRect = new Rect(leftRectSize, height - bottomRectSize, width - leftRectSize - rightRectSize, bottomRectSize);
        Rect sphereRect = new Rect(_sphereCenter.x - sphereWidth / 2f, _sphereCenter.y - sphereWidth / 2f, sphereWidth, sphereWidth);

        GUI.DrawTexture(leftRect, texture);
        GUI.DrawTexture(rightRect, texture);
        GUI.DrawTexture(upperRect, texture);
        GUI.DrawTexture(bottomRect, texture);

        GUI.DrawTexture(sphereRect, _sphereTexture);
    }

    private Texture2D GetRectTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        return texture;
    }

    private Texture2D GetSphereTexture(Color color)
    {
        const int RESOLUTION = 256;

        Texture2D texture = new Texture2D(RESOLUTION, RESOLUTION);

        Vector2 center = new Vector2(RESOLUTION / 2f, RESOLUTION / 2f);
        float sphereRadius = RESOLUTION / 2f;

        for (int y = 0; y < RESOLUTION; y++)
        {
            for (int x = 0; x < RESOLUTION; x++)
            {
                Vector2 point = new Vector2(x, y);

                float distance = Vector2.Distance(center, point);

                if (distance > sphereRadius)
                {
                    texture.SetPixel(x, y, Color.clear);
                }
                else
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }

        texture.Apply();

        return texture;
    }
}
