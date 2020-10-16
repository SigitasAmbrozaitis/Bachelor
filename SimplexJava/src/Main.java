public class Main {

    public static void main(String[] args) {
        System.out.println("Hello World!");
        SimplexNoise noise = new SimplexNoise();
        for(float i = 0.0000000f; i<10; ++i)
            for(float j = 0.0000000f; j<10; ++j)
                for(float k = 0.0000000f; k<10; ++k)
                {
                    double n = noise.noise(i,j,k);

                    String n1 = Double.toString(n);
                    if(n1.charAt(0) != '0' && n1.charAt(0)!='-'){
                        int br = 0;}
                    double a = 1;
                    boolean c = n > a;
                    if(n > 1 || n < -1){
                        int br = 0;}

                    System.out.println("i:" + i + " j:" + j + " k:" + k  + " noise:" + n);
                }

    }
}
