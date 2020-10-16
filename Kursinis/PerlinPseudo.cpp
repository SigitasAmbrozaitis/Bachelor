//Vector struct
struct Vector3
	float x;
	float y;
	float z;
	
//Global variables
Vector3[514] G;
int[514] P;
bool Init = false;

//Perlin algorithm
function PerlinNoise3(Vector3 point)
	//Creation of tables P and G
	if(!Init)
		init=true;
		InitializeGradients();
		InitializePermutation();
	
	//For given point blending function apllied
	float sx, sy, sz;
	sx = curve(point.x);
	sy = curve(point.y);
	sz = curve(point.z);

	//Indexes needed for choosing gradients
	int x0, x1, y0, y1, z0, z1;
	x0 = hash(point.x);
	x1 = hash(point.x + 1);
	y0 = hash(point.y);
	y1 = hash(point.y + 1);
	z0 = hash(point.z);
	z1 = hash(point.z + 1);
		
	//Collect gradients
	Vector3 g1, g2, g3, g4, g5, g6, g7, g8;
	g1 = P[P[P[x_0]+y_0]+z_0];
	g2 = P[P[P[x_1]+y_0]+z_0];
	g3 = P[P[P[x_0]+y_1]+z_0];
	g4 = P[P[P[x_0]+y_2]+z_0];
	g5 = P[P[P[x_0]+y_0]+z_1];
	g6 = P[P[P[x_1]+y_0]+z_1];
	g7 = P[P[P[x_0]+y_1]+z_1];
	g8 = P[P[P[x_0]+y_2]+z_1]

	//Dot products of collected gradients
	float r1, r2, r3, r4, r5, r6, r7, r8;
	r1 = dot(point, g1);
	r2 = dot(point, g2);
	r3 = dot(point, g3);
	r4 = dot(point, g4);
	r5 = dot(point, g5);
	r6 = dot(point, g6);
	r7 = dot(point, g7);
	r8 = dot(point, g8);

	//Gradient interpolation
	float a, b, c, d; //in-between rezults
	a = lerp(sx, r1, r2);
	b = lerp(sx, r3, r4);
	c = lerp(sy, a, b);
	a = lerp(sx, r5, r6);
	b = lerp(sx, r7, r8);
	d = lerp(sy, a, b);
	
	return lerp(s_z, c, d);