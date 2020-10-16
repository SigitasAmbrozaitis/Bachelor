#pragma once

class Simplex{


	
public:
	Simplex();
	~Simplex();
	static double noise(double x, double y, double z);
private:

	static double K(int a);
	static int shuffle(int i, int j, int k);
	static int b(int i, int j, int k, int B);
	static int b(int N, int B);
};