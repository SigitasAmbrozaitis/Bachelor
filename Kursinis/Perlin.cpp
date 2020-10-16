#include <iostream>
#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <random>

#define B 0x100	//256
#define BM 0xff	//255

#define N 0x1000	
#define NP 12		
#define NM 0xfff	

static int p[B + B + 2];		
static float g3[B + B + 2][3];	
static int start = 1;

double noise3(double x, double y, double z);

static void normalize3(float v[3]);

static void init(void);

#define s_curve(t) ( t * t * (3. - 2. * t) )
#define lerp(t, a, b) ( a + t * (b - a) )
#define setup(i,b0,b1,r0,r1)\
	t = vec[i] + N;\
	b0 = ((int)t) & BM;\
	b1 = (b0+1) & BM;\
	r0 = t - (int)t;\
	r1 = r0 - 1.;

double noise3(double x, double y, double z)
{
	float vec[] = { x, y, z };
	int i, j;
	int bx0, bx1;
	int by0, by1;
	int bz0, bz1;
	int b00, b10, b01, b11;

	float rx0, rx1;
	float ry0, ry1;
	float rz0, rz1;
	float *q, sy, sz, a, b, c, d, t, u, v;

	if (start) {
		start = 0;
		init();
	}

	setup(0, bx0, bx1, rx0, rx1);
	setup(1, by0, by1, ry0, ry1);
	setup(2, bz0, bz1, rz0, rz1);

	i = p[bx0];
	j = p[bx1];

	b00 = p[i + by0];
	b10 = p[j + by0];
	b01 = p[i + by1];
	b11 = p[j + by1];

	t = s_curve(rx0);
	sy = s_curve(ry0);
	sz = s_curve(rz0);

#define at3(rx,ry,rz) ( rx * q[0] + ry * q[1] + rz * q[2] )

	q = g3[b00 + bz0];
	u = at3(rx0, ry0, rz0);
	q = g3[b10 + bz0];
	v = at3(rx1, ry0, rz0);
	a = lerp(t, u, v);

	q = g3[b01 + bz0];
	u = at3(rx0, ry1, rz0);
	q = g3[b11 + bz0];
	v = at3(rx1, ry1, rz0);
	b = lerp(t, u, v);

	c = lerp(sy, a, b);

	q = g3[b00 + bz1];
	u = at3(rx0, ry0, rz1);
	q = g3[b10 + bz1];
	v = at3(rx1, ry0, rz1);
	a = lerp(t, u, v);

	q = g3[b01 + bz1];
	u = at3(rx0, ry1, rz1);
	q = g3[b11 + bz1]; 
	v = at3(rx1, ry1, rz1);
	b = lerp(t, u, v);

	d = lerp(sy, a, b);

	return lerp(sz, c, d);
}

static void normalize3(float v[3])
{
	float s;

	s = sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
	v[0] = v[0] / s;
	v[1] = v[1] / s;
	v[2] = v[2] / s;
}

static void init(void)
{
	int i, j, k;
	for (i = 0; i < B; i++) {
		p[i] = i;
		for (j = 0; j < 3; j++)
			g3[i][j] = (float)((std::rand() % (B + B)) - B) / B;
		normalize3(g3[i]);
	}

	while (--i) {
		k = p[i];
		p[i] = p[j = std::rand() % B];
		p[j] = k;
	}

	for (i = 0; i < B + 2; i++) {
		p[B + i] = p[i];
		g1[B + i] = g1[i];
		for (j = 0; j < 2; j++)
			g2[B + i][j] = g2[i][j];
		for (j = 0; j < 3; j++)
			g3[B + i][j] = g3[i][j];
	}
}