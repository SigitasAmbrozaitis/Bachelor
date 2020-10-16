struct position
	float x;
	float y;

struct pixel
	Vector3 position;
	float value;

int imageSize = 256	
pixel[256] image;

for(i = 0; i<imageSize; ++i)
	for(j = 0; j<imageSize; ++j)
		pixel.position.x = i;
		pixel.position.y = j;
		pixel.value = ApplyNoise(pixel.position)


ApplyNoise(Vector3 point)
	return Noise(position)
	

