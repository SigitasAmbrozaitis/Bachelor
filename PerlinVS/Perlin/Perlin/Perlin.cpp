// Perlin.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <random>
#include <functional>
#include <time.h>  
#include <chrono>
#include "Simplex.h"
#include <ctime>


#define B 0x100	//256
#define BM 0xff	//255

#define N 0x1000	//4096
#define NP 12		//N = 2^NP
#define NM 0xfff	//4095

static int p[B + B + 2];		//goes to 255
static double g3[B + B + 2][3];	//gradients for 3d noise
static double g2[B + B + 2][2];	//gradients for 2d noise
static double g1[B + B + 2];		//gradients for 1d noise
static int start = 1;



double noise1(double arg);
double noise2(double vec[2]);
double noise3(double x, double y, double z);

static void normalize2(float v[2]);
static void normalize3(float v[3]);

static void init(void);

template <class T > void PrintContainer(T*container, int size);
double benchmark(double count, double increment, std::function<double(double, double, double)>func);
void test(int count, bool debug);
void test2();

#define s_curve(t) ( t * t * (3. - 2. * t) ) //3* 1-
#define lerp(t, a, b) ( a + t * (b - a) ) // 1* 1+ 1-
//2+ 2& 2-
#define setup(i,b0,b1,r0,r1)\
	t = vec[i] + N;\
	b0 = ((int)t) & BM;\
	b1 = (b0+1) & BM;\
	r0 = t - (int)t;\
	r1 = r0 - 1.;

int main()
{
	//noise3(0, 0, 0);
	//double time1 = 0.f; // benchmark(250, 1, noise3);
	//auto time2 = benchmark(250, 1, Simplex::noise);
	//printf("Perlin time:%f\nSimplex time:%f\n", time1, time2);
	//test(10000, false);
	test2();
}

void test2()
{
	double iterations = 1000000000;
	double total = 0.f;
	auto start = std::chrono::high_resolution_clock::now();
	for (double i = 0; i < iterations; ++i)
	{
		auto value = i * 0.16666f;
		total += value;
	}

	auto end = std::chrono::high_resolution_clock::now();
	std::chrono::duration<double, std::milli> elapsed = end - start;
	auto time1 = elapsed.count();
	
	total = 0.f;
	start = std::chrono::high_resolution_clock::now();
	for (double i = 0; i < iterations; ++i)
	{
		auto value = i / 6.f;
		total += value;
	}

	end = std::chrono::high_resolution_clock::now();
	elapsed = end - start;
	auto time2 = elapsed.count();

	printf("Mul time:%f\nDiv time:%f\n", time1, time2);
}

void test(int count, bool debug)
{
	double min = std::numeric_limits<double>::max();
	double max = std::numeric_limits<double>::lowest();
	
	for (int i = 0; i < count; ++i)
	{
		auto val = noise3(i * 0.01f, i * 0.02f, i * 0.03f);

		if (debug)
			std::cout << val << std::endl;

		if (val > max)
			max = val;
		if (val < min)
			min = val;
	}

	printf("Min:%lf Max:%lf", min, max);
}

double benchmark(double count, double increment, std::function<double(double, double, double)>func)
{
	auto start = std::chrono::high_resolution_clock::now();

	double min = 1;
	double max = -1;

	for (double i = 0; i < count; i += increment)
		for (double j = 0; j < count; j += increment)
			for (double k = 0; k < count; k += increment)
			{
				auto value = func(i, j, k);
				//if (value < min)
				//{
				//	min = value;
				//	//printf("Min:%lf\n", min);
				//}
				//if (value > max)
				//{
				//	max = value;
				//	//printf("Max:%lf\n", max);
				//}
			}
		
	//printf("Min:%lf Max:%lf\n", min, max);

	auto end = std::chrono::high_resolution_clock::now();
	std::chrono::duration<double, std::milli> elapsed = end - start;
	return elapsed.count()/(std::pow(count, 3)/increment);
}

double noise1(double arg)
{
	int bx0, bx1;
	float rx0, rx1, sx, t, u, v, vec[1];

	vec[0] = arg;
	if (start) {
		start = 0;
		init();
	}

	setup(0, bx0, bx1, rx0, rx1);

	sx = s_curve(rx0);

	u = rx0 * g1[p[bx0]];
	v = rx1 * g1[p[bx1]];

	return lerp(sx, u, v);
}

double noise2(double vec[2])
{
	int bx0, bx1, by0, by1, b00, b10, b01, b11;
	double rx0, rx1, ry0, ry1, *q, sx, sy, a, b, t, u, v;
	register int  i, j;

	if (start) {
		start = 0;
		init();
	}

	setup(0, bx0, bx1, rx0, rx1);
	setup(1, by0, by1, ry0, ry1);

	i = p[bx0];
	j = p[bx1];

	b00 = p[i + by0];
	b10 = p[j + by0];
	b01 = p[i + by1];
	b11 = p[j + by1];

	sx = s_curve(rx0);
	sy = s_curve(ry0);

#define at2(rx,ry) ( rx * q[0] + ry * q[1] )

	q = g2[b00]; u = at2(rx0, ry0);
	q = g2[b10]; v = at2(rx1, ry0);
	a = lerp(sx, u, v);

	q = g2[b01]; u = at2(rx0, ry1);
	q = g2[b11]; v = at2(rx1, ry1);
	b = lerp(sx, u, v);

	return lerp(sy, a, b);
}

double noise3(double x, double y, double z) //40+ 6& 16- 46*
{
	double vec[] = { x, y, z };
	int i, j;
	int bx0, bx1;
	int by0, by1;
	int bz0, bz1;
	int b00, b10, b01, b11;

	double rx0, rx1;
	double ry0, ry1;
	double rz0, rz1;
	double *q, sy, sz, a, b, c, d, t, u, v;

	if (start) {
		start = 0;
		init();
	}

	setup(0, bx0, bx1, rx0, rx1); //2+ 2& 2-
	setup(1, by0, by1, ry0, ry1); //2+ 2& 2-
	setup(2, bz0, bz1, rz0, rz1); //2+ 2& 2-

	i = p[bx0];
	j = p[bx1];

	b00 = p[i + by0]; //1+
	b10 = p[j + by0]; //1+
	b01 = p[i + by1]; //1+
	b11 = p[j + by1]; //1+

	t = s_curve(rx0);   //3* 1-
	sy = s_curve(ry0);  //3* 1-
	sz = s_curve(rz0);  //3* 1-

#define at3(rx,ry,rz) ( rx * q[0] + ry * q[1] + rz * q[2] ) //3* 2+

	q = g3[b00 + bz0]; //1+
	u = at3(rx0, ry0, rz0); //3* 2+
	q = g3[b10 + bz0];//1+
	v = at3(rx1, ry0, rz0);//3* 2+
	a = lerp(t, u, v); //1 * 1 + 1 -

	q = g3[b01 + bz0];//1+
	u = at3(rx0, ry1, rz0);//3* 2+
	q = g3[b11 + bz0];//1+
	v = at3(rx1, ry1, rz0);//3* 2+
	b = lerp(t, u, v); //1* 1+ 1-

	c = lerp(sy, a, b); //1* 1+ 1-

	q = g3[b00 + bz1];//1+
	u = at3(rx0, ry0, rz1);//3* 2+
	q = g3[b10 + bz1];//1+
	v = at3(rx1, ry0, rz1);//3* 2+
	a = lerp(t, u, v);//1* 1+ 1-

	q = g3[b01 + bz1];//1+
	u = at3(rx0, ry1, rz1);//3* 2+
	q = g3[b11 + bz1]; //1+ 
	v = at3(rx1, ry1, rz1);//3* 2+
	b = lerp(t, u, v);//1* 1+ 1-

	d = lerp(sy, a, b);//1* 1+ 1-

	return lerp(sz, c, d);//1* 1+ 1-
}

static void normalize2(double v[2])
{
	double s;

	s = sqrt(v[0] * v[0] + v[1] * v[1]);
	v[0] = v[0] / s;
	v[1] = v[1] / s;
}

static void normalize3(double v[3])
{
	double s;

	s = sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
	v[0] = v[0] / s;
	v[1] = v[1] / s;
	v[2] = v[2] / s;
}

static void init(void)
{
	int i, j, k;

	std::srand(std::time(nullptr));

	//create 1d/2d/3d gradients
	for (i = 0; i < B; i++) {
		p[i] = i;

		g1[i] = (float)((std::rand() % (B + B)) - B) / B;

		for (j = 0; j < 2; j++)
			g2[i][j] = (double)((std::rand() % (B + B)) - B) / B;
		normalize2(g2[i]);

		for (j = 0; j < 3; j++)
			g3[i][j] = (double)((std::rand() % (B + B)) - B) / B;
		normalize3(g3[i]);
	}

	//shuffle p values 
	while (--i) {
		k = p[i];
		p[i] = p[j = std::rand() % B];
		p[j] = k;
	}

	//PrintContainer<int>(p, B*2+2);

	//extend p value form 255 to 514 indexes, just copy over
	for (i = 0; i < B + 2; i++) {
		p[B + i] = p[i];
		g1[B + i] = g1[i];
		for (j = 0; j < 2; j++)
			g2[B + i][j] = g2[i][j];
		for (j = 0; j < 3; j++)
			g3[B + i][j] = g3[i][j];
	}

	//PrintContainer<int>(p, B * 2 + 2);

	int a = 0;
}

template <class T > void PrintContainer(T*container, int size)
{
	for (int i = 0; i < size; ++i)
	{
		std::cout << container[i] << " ";
		if (i != 0 && i % 16 == 0)
			std::cout << std::endl;
	}
	std::cout << std::endl;
	std::cout << std::endl;
}