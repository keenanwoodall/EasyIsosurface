using UnityEngine;
using System.Collections;
using Mathx;

namespace Debugx
{
	public static class DebugDraw
	{
		public static Color defaultColor = Color.white;

		public static void Circle(float radius, Vector3 position, Color color)
		{
			float z = position.z;
			Vector3 lastPoint = new Vector3();

			for(float i = 1f; i < 7.5f; i += 0.1f)
			{
				float x = i;
				float y = i;

				x.Remap(0f, 90f, 0f, 1f);
				y.Remap(0f, 90f, 0f, 1f);

				x = (Mathf.Sin(i) * radius) + position.x;
				y = (Mathf.Cos(i) * radius) + position.y;

				if(i != 1f)
					Debug.DrawLine(lastPoint, new Vector3(x, y, z), color);

				lastPoint = new Vector3(x, y, z);
			}
		}
		public static void Circle(float radius, Vector3 position)
		{
			Circle(radius, position, defaultColor);
		}

		public static void Cube(float scale, Vector3 position, Color color)
		{
			Vector3 frontBottomLeft, frontBottomRight, frontTopLeft, frontTopRight;
			Vector3 backBottomLeft, backBottomRight, backTopLeft, backTopRight;

			frontBottomLeft =  new Vector3(position.x - (scale / 2), position.y - (scale / 2), position.z - (scale / 2));
			frontBottomRight = new Vector3(position.x + (scale / 2), position.y - (scale / 2), position.z - (scale / 2));
			frontTopLeft =     new Vector3(position.x - (scale / 2), position.y + (scale / 2), position.z - (scale / 2));
			frontTopRight =    new Vector3(position.x + (scale / 2), position.y + (scale / 2), position.z - (scale / 2));

			backBottomLeft =   new Vector3(position.x - (scale / 2), position.y - (scale / 2), position.z + (scale / 2));
			backBottomRight =  new Vector3(position.x + (scale / 2), position.y - (scale / 2), position.z + (scale / 2));
			backTopLeft =      new Vector3(position.x - (scale / 2), position.y + (scale / 2), position.z + (scale / 2));
			backTopRight =     new Vector3(position.x + (scale / 2), position.y + (scale / 2), position.z + (scale / 2));

			//Front Face
			Debug.DrawLine(frontTopLeft, frontTopRight, color);
			Debug.DrawLine(frontTopRight, frontBottomRight, color);
			Debug.DrawLine(frontBottomRight, frontBottomLeft, color);
			Debug.DrawLine(frontBottomLeft, frontTopLeft, color);
			//Back Face
			Debug.DrawLine(backTopLeft, backTopRight, color);
			Debug.DrawLine(backTopRight, backBottomRight, color);
			Debug.DrawLine(backBottomRight, backBottomLeft, color);
			Debug.DrawLine(backBottomLeft, backTopLeft, color);
			//Left Face
			Debug.DrawLine(frontTopLeft, backTopLeft, color);
			Debug.DrawLine(backTopLeft, backBottomLeft, color);
			Debug.DrawLine(backBottomLeft, frontBottomLeft, color);
			Debug.DrawLine(frontBottomLeft, frontTopLeft, color);

			Debug.DrawLine(frontTopRight, backTopRight, color);
			Debug.DrawLine(backTopRight, backBottomRight, color);
			Debug.DrawLine(backBottomRight, frontBottomRight, color);
			Debug.DrawLine(frontBottomRight, frontTopRight, color);
		}
		public static void Cube(float scale, Vector3 position)
		{
			Cube(scale, position, Color.white);
		}
	}
}