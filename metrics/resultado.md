# Análisis de Rendimiento

## Caso 1: Dataset Mediano

**Archivo:** Ventas_153mb.csv  
**Registros:** 1,000,000  

### Ejecución Secuencial

- Tiempo: **2412 ms**

### Ejecución Paralela

| Procesadores | Tiempo (ms) | Speedup | Eficiencia |
|-------------|------------|--------|-----------|
| 2           | 1302       | 1.85x  | 92.63%    |
| 4           | 772        | 3.12x  | 78.11%    |
| 6           | 588        | 4.10x  | 68.37%    |
| 8           | 494        | 4.88x  | 61.03%    |
| 10          | 438        | 5.51x  | 55.07%    |
| 12          | 387        | 6.23x  | 51.94%    |
| 16          | 364        | 6.63x  | 41.41%    |

### Conclusión

El paralelismo mejora significativamente el tiempo de ejecución.

El mejor balance entre rendimiento y eficiencia se encuentra entre **4 y 6 procesadores**, donde la eficiencia se mantiene cercana al 70%.

A partir de ese punto, la eficiencia disminuye debido a la sobrecarga de coordinación entre hilos.

---

## Caso 2: Dataset Grande

**Archivo:** Ventas_700mb.csv  
**Registros:** 5,000,000  

### Ejecución Secuencial

- Tiempo: **12072 ms**

### Ejecución Paralela

| Procesadores | Tiempo (ms) | Speedup | Eficiencia |
|-------------|------------|--------|-----------|
| 2           | 6842       | 1.76x  | 88.22%    |
| 4           | 3953       | 3.05x  | 76.35%    |
| 6           | 2938       | 4.11x  | 68.48%    |
| 8           | 2508       | 4.81x  | 60.17%    |
| 10          | 2292       | 5.27x  | 52.67%    |

### Conclusión

El paralelismo sigue mejorando el tiempo de ejecución con datasets más grandes.

Sin embargo, la eficiencia disminuye progresivamente a medida que aumenta el número de procesadores, debido a la sobrecarga de sincronización.

---

## Conclusión General

- El paralelismo mejora el rendimiento cuando la carga computacional es alta  
- No escala de forma lineal con la cantidad de procesadores  
- Existe un punto óptimo donde la eficiencia es máxima  
- La descomposición de datos es una estrategia efectiva para este tipo de problemas  
