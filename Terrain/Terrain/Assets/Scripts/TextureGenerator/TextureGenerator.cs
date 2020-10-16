using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

[RequireComponent(typeof(CanvasRenderer))]
public class TextureGenerator : MonoBehaviour
{
    [SerializeField]
    private Image m_Image = default;
    public Image image
    {
        get
        {
            return m_Image;
        }
        set
        {
            m_Image = value;
            Redraw();
        }
    }


    [SerializeField]
    private Vector2 m_Dimension = Vector2.one;
    public Vector2 dimension
    {
        get
        {
            return m_Dimension;
        }
        set
        {
            m_Dimension = value;
            Redraw();
        }
    }

    [SerializeField]
    private Color m_DefaultColor = Color.black;
    public Color defaultColor
    {
        get
        {
            return m_DefaultColor;
        }
        set
        {
            m_DefaultColor = value;
            Redraw();
        }
    }

    [SerializeField]
    private Vector2 m_Step = Vector2.one;
    public Vector2 step
    {
        get
        {
            return m_Step;
        }
        set
        {
            m_Step = value;
            Redraw();
        }
    }

    [SerializeField]
    private Vector2 m_Speed = Vector2.one;
    public Vector2 speed
    {
        get
        {
            return m_Speed;
        }
        set
        {
            m_Speed = value;
            Redraw();
        }
    }

    [SerializeField]
    private bool m_Alpha = false;
    public bool alpha
    {
        get
        {
            return m_Alpha;
        }
        set
        {
            m_Alpha = value;
            Redraw();
        }
    }

    [SerializeField]
    private bool m_Red = false;
    public bool red
    {
        get
        {
            return m_Red;
        }
        set
        {
            m_Red = value;
            Redraw();
        }
    }

    [SerializeField]
    private bool m_Green = false;
    public bool green
    {
        get
        {
            return m_Green;
        }
        set
        {
            m_Green = value;
            Redraw();
        }
    }

    [SerializeField]
    private bool m_Blue = false;
    public bool blue
    {
        get
        {
            return m_Blue;
        }
        set
        {
            m_Blue = value;
            Redraw();
        }
    }

    [SerializeField]
    private bool m_UsePerlin = false;
    public bool usePerlin
    {
        get
        {
            return m_UsePerlin;
        }
        set
        {
            m_UsePerlin = value;
            Redraw();
        }
    }

    [SerializeField]
    private Vector2 m_Offset = Vector3.zero;
    public Vector2 offset
    {
        get
        {
            return m_Offset;
        }
        set
        {
            m_Offset = value;
            Redraw();
        }
    }

    private int m_Width = 0;
    private int m_Heigth = 0;

    private Sprite m_Sprite = null;




    private Texture2D m_Texture = default;
    public Texture2D texture
    {
        get { return m_Texture;}
    }

    public void Redraw()
    {
        m_Width = (int)m_Dimension.x;
        m_Heigth = (int)m_Dimension.y;

        m_Texture = GenerateTexture();

        m_Sprite = Sprite.Create(texture, new Rect(0, 0, m_Width, m_Heigth), new Vector2(0.5f, 0.5f));
        m_Image.sprite = m_Sprite;
    }

    public void CreateSprite()
    {
        m_Texture = GenerateTexture();

        m_Sprite = Sprite.Create(texture, new Rect(0, 0, m_Width, m_Heigth), new Vector2(0.5f, 0.5f));
        m_Image.sprite = m_Sprite;
    }

    public void Initialise()
    {
        m_Width = (int)m_Dimension.x;
        m_Heigth = (int)m_Dimension.y;
        CreateSprite();
    }

    private Texture2D GenerateSinTexture()
    {
        var texture = new Texture2D(m_Width, m_Heigth, TextureFormat.ARGB32, false);

        for (int i = 0; i < m_Width; ++i)
        {
            for (int j = 0; j < m_Heigth; ++j)
            {


                var x = i * m_Step.x + m_Offset.x;
                var y = j * m_Step.y + m_Offset.y;
                //m_DefaultColor.a = GetNoise(
                //    GetNoise(GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), 0f),
                //    GetNoise(GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), 0f),
                //    0f);

                //Beer2
                /*
                if (m_Alpha)
                    m_DefaultColor.a = GetNoise(math.sin(x) + math.cos(y), math.sin(y) + math.cos(x), math.sin(y) + math.cos(x));

                if (m_Red)
                    m_DefaultColor.r = GetNoise(math.sin(x) + math.cos(y), math.sin(y) + math.cos(x), math.sin(y) + math.cos(x));//GetNoise(x, y, x);

                if (m_Green)
                    m_DefaultColor.g = GetNoise(math.sin(x) + math.cos(y), math.sin(y) + math.cos(x), math.sin(y) + math.cos(x));//GetNoise(x, y, x);

                if (m_Blue)
                    m_DefaultColor.b = GetNoise(math.sin(x) + math.cos(y), math.sin(y) + math.cos(x), math.sin(y) + math.cos(x));//GetNoise(x, y, x);
                */

                //ripple like
                /*
                if (m_Alpha)
                    m_DefaultColor.a = GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y)));

                if (m_Red)
                    m_DefaultColor.r = GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y)));//GetNoise(x, y, x);

                if (m_Green)
                    m_DefaultColor.g = GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y)));//GetNoise(x, y, x);

                if (m_Blue)
                    m_DefaultColor.b = GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y)));//GetNoise(x, y, x);

                */
                //x -= 0.5f * m_Width * m_Step.x + m_Offset.x;
                //y -= 0.5f * m_Heigth * m_Step.y + m_Offset.y;




                //var alpha1 = GetNoise(0f, math.sin(math.sqrt(x * x + y * y)), 0f);
                //var alpha2 = GetNoise(0f, x / (y + 0.01f) * y / (x + 0.01f), 0f);
                //var alpha3 = GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y))); //ripple
                //var alpha4 = GetNoise(math.sin(x / (y + 0.01f)), math.sin(math.sqrt(x * x + y * y)),math.sin(y*x));
                //var alpha5 = math.sin(GetNoise(0f, math.sin(math.sqrt(x * x + y * y)), 0f) * GetNoise(0f, x / (y + 0.01f) * y / (x + 0.01f), 0f)); //blurred circle
                //var alpha6 = GetNoise(0f, math.sin(math.sqrt(x * x + y * y)), 0f); //circle
                //var alpha7 = GetNoise(0f, 0f, x * y) - alpha5;

                //var alpha8 = math.dot(new Vector3(alpha1, alpha2, alpha3), new Vector3(alpha4, alpha5, alpha7));
                //var alpha9 = GetNoise(GetNoise(x * y, 0f, 0f), alpha6, 0f);
                var alpha10 = GetNoise(x, y * 0.1f, 0f); //stretched noise
                var alpha11 = (GetNoise(x * 5f, y, 0f) - GetNoise(y, x * 5f, 0f)); //grains
                var alpha12 = alpha10 * 0.5f + alpha11 * 0.5f; //fused alpha11 & alpha10
                //var alpha13 = GetNoise(math.sin(math.sqrt(alpha6*alpha6 + x*y )), 0f, 0f);
                var alpha14 = 20*GetNoise(x*0.1f, y*0.1f, 0.8f); //wood
                alpha14 = alpha14 - math.floor(alpha14);
                var alpha15 = alpha12 + alpha14;


                var alpha = alpha10;

                if (m_Alpha)
                    m_DefaultColor.a = alpha;

                if (m_Red)
                    m_DefaultColor.r = alpha;// GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y)));//GetNoise(x, y, x);

                if (m_Green)
                    m_DefaultColor.g = alpha;//GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y)));//GetNoise(x, y, x);

                if (m_Blue)
                    m_DefaultColor.b = alpha;//GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y)));//GetNoise(x, y, x);


                texture.SetPixel(i, j, m_DefaultColor);
            }
        }

        m_Offset.x += m_Speed.x * Time.unscaledDeltaTime;
        m_Offset.y += m_Speed.y * Time.unscaledDeltaTime;

        texture.Apply();
        return texture;
    }

    public Texture2D GenerateTexture()
    {
        var texture = new Texture2D(m_Width, m_Heigth, TextureFormat.ARGB32, false);

        for (int i = 0; i < m_Width; ++i)
        {
            for (int j = 0; j < m_Heigth; ++j)
            {
                var x = i * m_Step.x + m_Offset.x;
                var y = j * m_Step.y + m_Offset.y;
                var beer = GetNoise(math.sin(x) + math.cos(y), math.sin(y) + math.cos(x), math.sin(y) + math.cos(x));
                //var ripple = GetNoise(GetNoise(x, y, math.sin(x) + math.sin(y)), y, GetNoise(x, y, math.sin(x) + math.sin(y))); //ripple
                //var circle = GetNoise(0f, math.sin(math.sqrt(x * x + y * y)), 0f);
                //var strange = GetNoise(
                //    GetNoise(GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), 0f),
                //    GetNoise(GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), GetNoise(GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), 0f), 0f), 0f),
                //    0f);
                //var simetric = GetNoise(GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f), GetNoise(math.sin(x), math.sin(y), 0f));

                //var stretched = GetNoise(x, y * 0.1f, 0f); //stretched noise
                //var grain = (GetNoise(x * 5f, y, 0f) - GetNoise(y, x * 5f, 0f)); //grains

                //var groove = 20 * GetNoise(x * 0.1f, y * 0.1f, 0.8f); //groove
                //groove = groove - math.floor(groove);

                //var wood = 0.5f * (stretched + grain) + groove; //wood

                //if (m_Alpha)
                //    m_DefaultColor.a = wood;

                //if(m_Red)
                //    m_DefaultColor.r = wood;

                //if(m_Green)
                //    m_DefaultColor.g = wood;

                //if(m_Blue)
                //    m_DefaultColor.b = wood;

                //var alpha = GetNoise(GetNoise(GetNoise(GetNoise(x, y, 0f), GetNoise(x, y, 0f), 0f), GetNoise(GetNoise(x, y, 0f), GetNoise(x, y, 0f), 0f), 0f), 
                //    GetNoise(GetNoise(GetNoise(x, y, 0f), GetNoise(x, y, 0f), 0f), GetNoise(GetNoise(x, y, 0f), GetNoise(x, y, 0f), 0f), 0f), 0f);

                //var alpha = GetNoise(GetNoise(GetNoise(x*y, x*y, 0f), GetNoise(x * y, x * y, 0f), 0f), 
                //    GetNoise(GetNoise(x * y, x * y, 0f), GetNoise(x * y, x * y, 0f), 0f), 0f);

                var alpha = beer;

                if (m_Alpha)
                    m_DefaultColor.a = alpha;

                if (m_Red)
                    m_DefaultColor.r = alpha;

                if (m_Green)
                    m_DefaultColor.g = alpha;

                if (m_Blue)
                    m_DefaultColor.b = alpha;


                texture.SetPixel(i, j, m_DefaultColor);
            }
        }

        m_Offset.x += m_Speed.x * Time.unscaledDeltaTime;
        m_Offset.y += m_Speed.y * Time.unscaledDeltaTime;

        texture.Apply();
        return texture;
    }

    private void Update()
    {
        Redraw();
    }

    private float GetNoise(float x, float y, float z)
    {
        if(m_UsePerlin)
        {
            return PerlinNoiseByte.GetValue(x, y, z);
        }

        return SimplexNoise.GetValue(x, y, z);
    }





}
