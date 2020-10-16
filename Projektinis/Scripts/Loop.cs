public class TextureGenerator
{

    private Vector2 m_Dimension = Vector2.one;
    private Vector2 m_Step = Vector2.one;
    private Vector2 m_Offset = Vector3.zero;

    private int m_Width = 0;
    private int m_Heigth = 0;

    private Texture2D GenerateTexture()
    {
        m_Width = (int)m_Dimension.x;
        m_Heigth = (int)m_Dimension.y;

        for (int i = 0; i < m_Width; ++i)
        {
            for (int j = 0; j < m_Heigth; ++j)
            {
                var x = i * m_Step.x + m_Offset.x;
                var y = j * m_Step.y + m_Offset.y;
            }
        }
        return null;
    }
}
