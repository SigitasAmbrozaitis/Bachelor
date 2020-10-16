using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace CPU
{
	public delegate NoiseSample NoiseMethod(Vector3 point, float frequency);



	public struct NoiseSample
	{

		public float m_Value;
		public float m_ColorValue;
		public Vector3 m_Derivative;

		public static NoiseSample operator +(NoiseSample a, NoiseSample b)
		{
			a.m_Value += b.m_Value;
			a.m_ColorValue += b.m_ColorValue;
			a.m_Derivative += b.m_Derivative;
			return a;
		}

		public static NoiseSample operator *(NoiseSample a, float b)
		{
			a.m_Value *= b;
			a.m_ColorValue *= b;
			a.m_Derivative *= b;
			return a;
		}
	}

	public static class Noise
    {
		public static long m_NoiseCalculationDuration = 0;
		public static long m_DerivativeCalculationDuration = 0;
		public static long m_GeometricalDerivativeDuration = 0;

		private static Vector2[] GRADIENT_2D = 
		{
			new Vector2( 1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2( 0f, 1f),
			new Vector2( 0f,-1f),
			new Vector2( 1f, 1f).normalized,
			new Vector2(-1f, 1f).normalized,
			new Vector2( 1f,-1f).normalized,
			new Vector2(-1f,-1f).normalized
		};

		private static readonly int[] PERMUTATION = 
		{
			151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
			140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
			247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
			 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
			 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
			 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
			 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
			200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
			 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
			207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
			119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
			129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
			218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
			 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
			184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
			222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,
			151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
			140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
			247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
			 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
			 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
			 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
			 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
			200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
			 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
			207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
			119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
			129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
			218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
			 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
			184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
			222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
		};

		private static int MASK = 255;
		private const int MASK2D = 7;
		private static float DIV_MASK = 1f / MASK;
		private static float SQR2 = math.sqrt(2f);

		public static float m_Min = float.MaxValue;
		public static float m_Max = float.MinValue;

		public static NoiseSample Sample(Vector2 pos, float frequency, int octaves, float strength, float lacunarity, float persistance, float amplitude, float damping, float strColoring, ENormal normalType)
		{
			bool bDamping = math.abs(damping - 1f) < 0.0001f;
			bool bColoring = math.abs(strColoring - 1f) < 0.0001f;



			float amp = bDamping ? strength / frequency : strength;
			var noiseSample = SumNoise(pos, frequency, octaves, lacunarity, persistance, amplitude, normalType);

			noiseSample.m_ColorValue = math.saturate(bColoring ? noiseSample.m_Value * strength * amplitude : noiseSample.m_Value);
			noiseSample.m_Value *= strength * amp;
			noiseSample.m_Derivative = Vector3.Normalize(noiseSample.m_Derivative * strength * amp);

			return noiseSample;
		}

		public static NoiseSample SumNoise(Vector2 pos, float frequency, int octaves, float lacunarity, float persistance, float amplitude, ENormal normalType)
		{
			var noiseSample = Noise2D(pos, frequency, normalType);
			float range = amplitude;
			for(int i = 0; i < octaves; ++i)
			{
				frequency *= lacunarity;
				amplitude *= persistance;
				range += amplitude;
				noiseSample += Noise2D(pos, frequency, normalType) * amplitude;
			}

			return noiseSample * (1f / range);
		}
		private static float MapTo1(float x)
		{
			return x * 0.5f + 0.5f;
		}
		private static float Smooth(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		private static float SmoothDerivative(float t)
		{
			return 30f * t * t * (t * (t - 2f) + 1f);
		}

		private static float Dot(Vector3 g, float x, float y)
		{
			return g.x * x + g.y * y;
		}

		public static NoiseSample Noise2D(Vector2 point, float frequency, ENormal normalType)
		{
			var noiseStart = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			point *= frequency;
			int ix0 = (int)math.floor(point.x);
			int iy0 = (int)math.floor(point.y);

			
			float tx0 = point.x - ix0;
			float ty0 = point.y - iy0;

			float tx1 = tx0 - 1f;
			float ty1 = ty0 - 1f;


			ix0 &= MASK;
			iy0 &= MASK;

			int ix1 = ix0 + 1;
			int iy1 = iy0 + 1;

			int h0 = PERMUTATION[ix0];
			int h1 = PERMUTATION[ix1];

			Vector3 g00 = GRADIENT_2D[PERMUTATION[h0 + iy0] & MASK2D];
			Vector3 g10 = GRADIENT_2D[PERMUTATION[h1 + iy0] & MASK2D];
			Vector3 g01 = GRADIENT_2D[PERMUTATION[h0 + iy1] & MASK2D];
			Vector3 g11 = GRADIENT_2D[PERMUTATION[h1 + iy1] & MASK2D];


			float v00 = Dot(g00, tx0, ty0);
			float v10 = Dot(g10, tx1, ty0);
			float v01 = Dot(g01, tx0, ty1);
			float v11 = Dot(g11, tx1, ty1);

			float tx = Smooth(tx0);
			float ty = Smooth(ty0);

			float a = v00;
			float b = v10 - v00;
			float c = v01 - v00;
			float d = v11 - v01 - v10 + v00;

			NoiseSample sample;

			sample.m_Value = MapTo1(a + b * tx + (c + d * tx) * ty);
			sample.m_ColorValue = sample.m_Value;
			sample.m_Derivative = Vector3.zero;
			

			if (normalType == ENormal.Analytical)
			{
				
				var startDerivative = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				float dtx = SmoothDerivative(tx0);
				float dty = SmoothDerivative(ty0);

				Vector2 da = g00;
				Vector2 db = g10 - g00;
				Vector2 dc = g01 - g00;
				Vector2 dd = g11 - g01 - g10 + g00;

				sample.m_Derivative = da + db * tx + (dc + dd * tx) * ty;
				sample.m_Derivative.x += (b + d * ty) * dtx;
				sample.m_Derivative.y += (c + d * tx) * dty;
				sample.m_Derivative.z = 0f;
				m_DerivativeCalculationDuration += System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startDerivative;
			}
			else if(normalType == ENormal.GeometricDinamic)
			{
				var startGeometrical = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

				var x1 = GetValue(new Vector2(point.x + 0.1f, point.y));
				var x2 = GetValue(new Vector2(point.x - 0.1f, point.y));
				var y1 = GetValue(new Vector2(point.x, point.y + 0.1f));
				var y2 = GetValue(new Vector2(point.x, point.y - 0.1f));

				sample.m_Derivative.x = (x1 - x2) * 0.5f;
				sample.m_Derivative.y = (y1 - x2) * 0.5f;

				m_GeometricalDerivativeDuration += System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startGeometrical;
			}
			sample.m_Derivative *= frequency;
			m_NoiseCalculationDuration += System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - noiseStart;
			return sample;
		}

		public static float GetValue(Vector2 point)
		{
			int ix0 = (int)math.floor(point.x);
			int iy0 = (int)math.floor(point.y);


			float tx0 = point.x - ix0;
			float ty0 = point.y - iy0;

			float tx1 = tx0 - 1f;
			float ty1 = ty0 - 1f;


			ix0 &= MASK;
			iy0 &= MASK;

			int ix1 = ix0 + 1;
			int iy1 = iy0 + 1;

			int h0 = PERMUTATION[ix0];
			int h1 = PERMUTATION[ix1];

			Vector3 g00 = GRADIENT_2D[PERMUTATION[h0 + iy0] & MASK2D];
			Vector3 g10 = GRADIENT_2D[PERMUTATION[h1 + iy0] & MASK2D];
			Vector3 g01 = GRADIENT_2D[PERMUTATION[h0 + iy1] & MASK2D];
			Vector3 g11 = GRADIENT_2D[PERMUTATION[h1 + iy1] & MASK2D];


			float v00 = Dot(g00, tx0, ty0);
			float v10 = Dot(g10, tx1, ty0);
			float v01 = Dot(g01, tx0, ty1);
			float v11 = Dot(g11, tx1, ty1);

			float tx = Smooth(tx0);
			float ty = Smooth(ty0);

			float a = v00;
			float b = v10 - v00;
			float c = v01 - v00;
			float d = v11 - v01 - v10 + v00;

			return (a + b * tx + (c + d * tx) * ty);
		}

	}



}
