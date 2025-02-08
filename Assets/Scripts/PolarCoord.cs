using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarCoord
{
    public float Radius { get; set; }
    public float Angle { get; set; }
    public float AngleDegree { get { return (this.Angle * 360) / (Mathf.PI * 2); } set { this.Angle = (value * 2f * Mathf.PI) / 360f; } }

    //public CartesianCoord ToCartesianCoors()
    //{
    //    CartesianCoord c = new CartesianCoord();

    //    c.X = this.Radius * Mathf.Cos(Angle);
    //    c.Y = this.Radius * Mathf.Sin(Angle);

    //    return c;
    //}

    public Vector2 ToCartesianCoords()
    {
        Vector2 vec = new Vector2();

        vec.x = this.Radius * Mathf.Cos(Angle);
        vec.y = this.Radius * Mathf.Sin(Angle);

        return vec;
    }

    public static PolarCoord ConvertCartesianToPolar(Vector2 cartesianCoord)
    {
        PolarCoord p = new PolarCoord();
        p.Radius = Mathf.Sqrt(cartesianCoord.x * cartesianCoord.x +
            cartesianCoord.y * cartesianCoord.y);
        p.Angle = Mathf.Atan2(cartesianCoord.y, cartesianCoord.x);
        return p;
    }

}
