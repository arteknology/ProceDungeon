using UnityEngine;
using Random = System.Random;

namespace Utils {
	public static class NoiseUtils {
	
		public static float[,] PerlinCave(float[,] map, float zoom, int xOffset, int yOffset) {
			for (int x = 0; x < map.GetUpperBound(0); x++) {
				for (int y = 0; y < map.GetUpperBound(1); y++) {
					if (x == 0 || y == 0 || x == map.GetLength(0) - 1 || y == map.GetLength(1) - 1)
					{
						map[x, y] = 1;
					}
					else
					{
						map[x, y] = Mathf.PerlinNoise(x * zoom + xOffset, y * zoom + yOffset);
					}
				}
			}
			return map;
		}
	}
}