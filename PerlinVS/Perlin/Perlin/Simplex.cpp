#include "pch.h"
#include "Simplex.h"
#include <cmath>

namespace 
{
	const static int T[] = { 0x15,0x38,0x32,0x2c,0x0d,0x13,0x07,0x2a };
	static int i, j, k, A[3];
	static double u, v, w;
};



Simplex::Simplex()
{
	i = j = k = 0;
	u = v = w = 0;
	A[0] = 0;
	A[1] = 0;
	A[2] = 0;
}

Simplex::~Simplex()
{
}

double Simplex::noise(double x, double y, double z) //73+ 6/ 29- 28* 100>> 104& 52<< 52|| 54?
{
	//midle point between points
	//double s = (x + y + z) / 3; //2+ 1/
	double s = (x + y + z) * 0.33333f;

	
	//new coordinate middle point + current point
	i = (int)std::floor(x + s); //1+
	j = (int)std::floor(y + s); //1+
	k = (int)std::floor(z + s); //1+

	//new middle point between new coodrinates
	//s = (i + j + k) / 6.; //2+ 1/
	s = (i + j + k) * 0.16666f; //2+ 1/

	//again new coodinates
	u = x - i + s; //1+ 1-
	v = y - j + s; //1+ 1-
	w = z - k + s; //1+ 1-


	A[0] = A[1] = A[2] = 0;

	int hi = u >= w ? u >= v ? 0 : 1 : v >= w ? 1 : 2; //3?
	int lo = u < w ? u < v ? 0 : 1 : v < w ? 1 : 2;    //3?
	return K(hi) + K(3 - hi - lo) + K(lo) + K(0);	   //63+ 4/ 26- 28* 100>> 104& 52<< 52| 48? 
}

double Simplex::K(int a)//15+ 1/ 6- 7* 25>> 26& 14<< 14| 12?
{
	double s;
	//s = (A[0] + A[1] + A[2]) / 6.;//2+ 1/
	s = (A[0] + A[1] + A[2]) * 0.16666f;//2+ 1/

	double x = u - A[0] + s;//1+ 1-
	double y = v - A[1] + s;//1+ 1-
	double z = w - A[2] + s;//1+ 1-
	double t = .6 - x * x - y * y - z * z;//3* 3-

	int h = shuffle(i + A[0], j + A[1], k + A[2]);//21>> 21& 14<< 14| 7+
	A[a]++;//1+
	if (t < 0)//1?
		return 0;

	int b5 = h >> 5 & 1;  //1>> 1&
	int b4 = h >> 4 & 1;  //1>> 1&
	int b3 = h >> 3 & 1;  //1>> 1&
	int b2 = h >> 2 & 1;   //1>> 1&
	int b = h & 3;      //1&

	double p = b == 1 ? x : b == 2 ? y : z; //2?
	double q = b == 1 ? y : b == 2 ? z : x; //2?
	double r = b == 1 ? z : b == 2 ? x : y; //2?

	p = (b5 == b3 ? -p : p); //1?
	q = (b5 == b4 ? -q : q); //1?
	r = (b5 != (b4^b3) ? -r : r);//1? 1^
	t *= t;//1*

	return 8 * t * t * (p + (b == 0 ? q + r : b2 == 0 ? q : r));//3* 2+ 2?
}

int Simplex::shuffle(int i, int j, int k)//21>> 21& 14<< 14| 7+
{
	return b(i, j, k, 0) + b(j, k, i, 1) + b(k, i, j, 2) + b(i, j, k, 3) +
		b(j, k, i, 4) + b(k, i, j, 5) + b(i, j, k, 6) + b(j, k, i, 7);
}

int Simplex::b(int i, int j, int k, int B)//3>> 3& 2<< 2|
{ 
	return T[b(i, B) << 2 | b(j, B) << 1 | b(k, B)]; 
}
int Simplex::b(int N, int B)//1>> 1&
{
	return N >> B & 1;
}