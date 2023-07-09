using System;

[Serializable]
public class PointProperties
{
    public int PointClass;
    public float X;
    public float Y;

    public string TextFirst;
    public string TextSecond;
    public string TextThird;

    public PointProperties(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public PointProperties(){ }
}