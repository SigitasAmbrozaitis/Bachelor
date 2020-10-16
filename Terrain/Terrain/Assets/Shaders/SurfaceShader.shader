Shader "Custom/Surface Shader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ColorTex ("Color Gradeint", 2D) = "white" {}
		
		[Toggle] _Damping("Damping", Float) = 0
		[Toggle] _StrenghtColoring("Strength Coloring", Float) = 0

		_OffsetX("Noise Offset X", Range(-1, 1)) = 0
		_OffsetY("Noise Offset Y", Range(-1, 1)) = 0

		_Frequency("Frequency", Range(1.0, 16.0)) = 1.0
		_Octaves("Ocatves", Range(1, 8)) = 1.0
		_Strength("Strength", Range(0.0, 4.0)) = 1.0
		_Lacunarity("Lacunarity", Range(1.0, 4.0)) = 1.0
		_Persistance("Persistance", Range(0.0, 1.0)) = 1.0
		_Amplitude("Amplitude", Range(0,8)) = 0
		_Contrast("Contrast", Range(0,4)) = 0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert vertex:vert



		static int PERMUTATION[512] = 
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

		static float2 GRADIENT[8] = 
		{
			float2(1,0),
			float2(-1, 0),
			float2(0, 1),
			float2(0, -1),
			normalize(float2(1, 1)),
			normalize(float2(-1, 1)),
			normalize(float2(1, -1)),
			normalize(float2(-1, -1))
		};

		struct NoiseSample
		{
			float value;
			float2 derivative;
			float colorNoise;
		};


		//static declaration
		static int MASK = 255;
		static int MASK3D = 7;
		static half DIV_MASk = 1.0 / 255.0;
		static half SQR2 = sqrt(2.0);

		//function declarations
		float Smooth(float t);
		float SmoothDerivative(float t);
		float MapTo1(float x);
		float Dot(float2 g, float x, float y, float z);

		NoiseSample Add(NoiseSample a, NoiseSample b);
		NoiseSample Mul(NoiseSample a, float b);


		NoiseSample Noise3D(float2 pos, float frequency);
		NoiseSample NoiseSum(float2 pos, float frequency, int octaves, float lacunarity, float persistence);
		NoiseSample SampleNoise(float2 pos);

		sampler2D _MainTex;
		sampler2D _ColorTex;

		bool _Damping;
		bool _StrenghtColoring;

		float _OffsetX;
		float _OffsetY;

		int _Octaves;
		float _Frequency;
		float _Strength;
		float _Lacunarity;
		float _Persistance;
		float _Distance;
		float _Amplitude;
		float _Contrast;

		struct Input {
			float2 uv_MainTex;
			float noiseValue;
			float noiseColor;
			float3 derivative;
		};

		void vert (inout appdata_full input, out Input o)
        {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			float2 offset = float2(_OffsetX, _OffsetY);
			float2 pos = input.vertex;

			pos.x = pos.x  + offset.x;
			pos.y = pos.y  + offset.y;

			NoiseSample noise = SampleNoise(pos);

			//o.derivative = normalize(noise.derivative);
			o.derivative = normalize(float3(-noise.derivative.x, 1, -noise.derivative.y));
			o.noiseColor = noise.colorNoise;
			o.noiseValue = noise.value;

			input.vertex.z = noise.value;

        }

		void surf (Input IN, inout SurfaceOutput o) 
		{
			
			float2 pos = IN.uv_MainTex;
			pos.x = IN.noiseColor;
			half4 ct = tex2D(_MainTex, IN.uv_MainTex);
			half4 c = tex2D (_ColorTex, pos);
		
			o.Normal = IN.derivative;
			o.Albedo = c.rgb * ct.rgb * _Contrast;
			o.Alpha = c.a * ct.rgb * _Contrast;
		}

		//function implementatios
		float Smooth(float t)
		{
			return t * t * t * (t * (t * 6 - 15) + 10);
		}

		float SmoothDerivative(float t)
		{
			return 30 * t * t * (t * (t - 2) + 1);
		}

		float MapTo1(float x)
		{
			return x * 0.8 + 0.5;
		}

		float Dot(float2 g, float x, float y)
		{
			return g.x * x + g.y * y;
		}

		float Saturate(float x)
		{
			return clamp(x, 0.01, 0.99);
		}


		//Noise sample functions
		NoiseSample Add(NoiseSample a, NoiseSample b)
		{
			a.value += b.value;
			a.derivative += b.derivative;
			return a;
		}

		NoiseSample Mul(NoiseSample a, float b)
		{
			a.value *= b;
			a.derivative *= b;
			return a;
		}


		//noise3d
		NoiseSample Noise3D(float2 pos, float frequency)
		{
			pos *= frequency;
			int ix0 = (int)floor(pos.x);
			int iy0 = (int)floor(pos.y);

			
			float tx0 = pos.x - ix0;
			float ty0 = pos.y - iy0;

			float tx1 = tx0 - 1;
			float ty1 = ty0 - 1;


			ix0 &= MASK;
			iy0 &= MASK;

			int ix1 = ix0 + 1;
			int iy1 = iy0 + 1;


			int h0 = PERMUTATION[ix0];
			int h1 = PERMUTATION[ix1];

			float2 g000 = GRADIENT[PERMUTATION[h0 + iy0] & MASK3D];
			float2 g100 = GRADIENT[PERMUTATION[h1 + iy0] & MASK3D];
			float2 g010 = GRADIENT[PERMUTATION[h0 + iy1] & MASK3D];
			float2 g110 = GRADIENT[PERMUTATION[h1 + iy1] & MASK3D];

			float v000 = Dot(g000, tx0, ty0);
			float v100 = Dot(g100, tx1, ty0);
			float v010 = Dot(g010, tx0, ty1);
			float v110 = Dot(g110, tx1, ty1);

			float tx = Smooth(tx0);
			float ty = Smooth(ty0);

			float dtx = SmoothDerivative(tx0);
			float dty = SmoothDerivative(ty0);

			float a = v000;
			float b = v100 - v000;
			float c = v010 - v000;
			float d = v110 - v010 - v100 + v000;

			float2 da = g000;
			float2 db = g100 - g000;
			float2 dc = g010 - g000;
			float2 dd = g110 - g010 - g100 + g000;


			NoiseSample val;

			val.value = MapTo1(a + b * tx + (c + d * tx) * ty);

			val.derivative = da + db * tx + (dc + dd * tx) * ty;
			val.derivative.x += (b + d * ty) * dtx;
			val.derivative.y += (c + d * tx) * dty;
			val.derivative *= frequency;

			val.colorNoise = val.value;


			return val;
		}

		NoiseSample NoiseSum(float2 pos, float frequency, int octaves, float lacunarity, float persistence)
		{
			NoiseSample sum = Noise3D(pos, frequency);
			float range = _Amplitude;
			float amplitude = _Amplitude;
			for (int o = 1; o < octaves; ++o)
			{
				frequency *= lacunarity;
				amplitude *= persistence;
				range += amplitude;
				sum = Add(sum, Mul(Noise3D(pos, frequency), amplitude));
			}
			return Mul(sum, 1 / range);
		}

		NoiseSample SampleNoise(float2 pos)
		{
			
			float amplitude = _Damping ? _Strength / _Frequency : _Strength;
			NoiseSample noiseSample =  NoiseSum(pos, _Frequency, _Octaves, _Lacunarity, _Persistance);

			noiseSample.colorNoise = _StrenghtColoring ? noiseSample.value * _Strength * amplitude : noiseSample.value;
			noiseSample.colorNoise = Saturate(noiseSample.colorNoise);
			noiseSample.value = noiseSample.value * _Strength * amplitude;
			noiseSample.derivative = normalize(noiseSample.derivative * _Strength * amplitude);//(-noiseSample.derivative.x, 1,  -noiseSample.derivative.y);

			return noiseSample;
		}


		ENDCG
	} 
	FallBack "Diffuse"
}