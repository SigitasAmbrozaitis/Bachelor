Pateikti 2 demo buildai
1. Perlin algoritmas naudojantis darbe aprašytą SurfaceCreator
2. Perlin algoritmas realizuotas shaderyje.

Unity versija 2019.3.8
Norint pasileisti patį Unity projektą reikia įsiinstaliuoti Unity editorių.

Pagrindinė scena test.scene
Pagrindiniai objektai scenoje yra:

TerrainGPU->objektas, kuris veikia naudodamas noise Shaderį 
GameObject->objektas, kuris naudoja SurfaceCreator
biplane->aktorius
Input->įvesties sistema

Runtime programa reaguos į visus pasikeitimus esančiuose faile NoiseSettings.asset
Abu objektai pritaikyti, kad reaguotu į NoiseSettings.asset pakeitimus.

Tikiuosi darbą bus įdomu vertinti.