using System;
using Microsoft.Xna.Framework;

/// <summary>
/// Represents a vector whose magnitude and direction are both expressed as System.Double. 
/// Vector addition and scalar multiplication are supported.
/// </summary>
public class Vector
{
	/// <summary>
	/// Gets or sets the magnitude of the vector.
	/// </summary>
    public double Magnitude
    {
        get;
        set;
    }
    
	/// <summary>
	/// Gets or sets the direction of the vector.
	/// </summary>
    public double Direction
    {
        get;
        set;
    }

    public Vector()
    {

    }

	/// <summary>
	/// Initialises a new instance of the Vector class using the specified magnitude and direction. 
	/// Automatically simplifies the representation to ensure a positive magnitude and a sub-circular angle.
	/// </summary>
	/// <param name="magnitude">The magnitude of the vector.</param>
	/// <param name="direction">The direction of the vector, in degrees.</param>
	public Vector(double magnitude, double direction)
    {
		Magnitude = magnitude;
		Direction = direction;

		if (Magnitude < 0)
        {
			// resolve negative magnitude by reversing direction
			Magnitude = -Magnitude;
			Direction = (180.0 + Direction) % 360;
		}

		// resolve negative direction
		if (Direction < 0)
            Direction = (360.0 + Direction);
	}

	/// <summary>
	/// Calculates the resultant sum of two vectors.
	/// </summary>
	/// <param name="a">The first operand.</param>
	/// <param name="b">The second operand.</param>
	/// <returns>The result of vector addition.</returns>
	public static Vector operator +(Vector a, Vector b)
    {
		// break into x-y components
		double aX = a.Magnitude * Math.Cos((Math.PI / 180.0) * a.Direction);
		double aY = a.Magnitude * Math.Sin((Math.PI / 180.0) * a.Direction);

		double bX = b.Magnitude * Math.Cos((Math.PI / 180.0) * b.Direction);
		double bY = b.Magnitude * Math.Sin((Math.PI / 180.0) * b.Direction);

		// add x-y components
		aX += bX;
		aY += bY;

		// pythagorus' theorem to get resultant magnitude
		double magnitude = Math.Sqrt(Math.Pow(aX, 2) + Math.Pow(aY, 2));

		// calculate direction using inverse tangent
		double direction;
        if (magnitude == 0)
            direction = 0;
        else
            direction = (180.0 / Math.PI) * Math.Atan2(aY, aX);

		return new Vector(magnitude, direction);
	}

	/// <summary>
	/// Calculates the result of multiplication by a scalar value.
	/// </summary>
	/// <param name="vector">The Vector that forms the first operand.</param>
	/// <param name="multiplier">The System.Double that forms the second operand.</param>
	/// <returns>A Vector whose magnitude has been multiplied by the scalar value.</returns>
	public static Vector operator *(Vector vector, double multiplier)
    {
		// only magnitude is affected by scalar multiplication
		return new Vector(vector.Magnitude * multiplier, vector.Direction);
	}

	/// <summary>
	/// Converts the vector into an X-Y coordinate representation.
	/// </summary>
	/// <returns>An X-Y coordinate representation of the Vector.</returns>
	public Point ToPoint()
    {
		// break into x-y components
		double aX = Magnitude * Math.Cos((Math.PI / 180.0) * Direction);
		double aY = Magnitude * Math.Sin((Math.PI / 180.0) * Direction);

		return new Point((int)aX, (int)aY);
	}

	/// <summary>
	/// Returns a string representation of the vector.
	/// </summary>
	/// <returns>A System.String representing the vector.</returns>
	public override string ToString()
    {
		return Magnitude.ToString("N5") + " " + Direction.ToString("N2") + "°";
	}
}