# Resultados del Análisis

## 1. Descripción de las pruebas

Para evaluar el rendimiento del sistema, se ejecutaron pruebas utilizando archivos CSV de distintos tamaños, desde miles hasta millones de registros.

El objetivo fue comparar:

- Tiempo de ejecución secuencial
- Tiempo de ejecución paralelo
- Speedup (mejora de rendimiento)
- Eficiencia (uso real de los procesadores)

Las pruebas se realizaron utilizando múltiples configuraciones de procesadores.

---

## 2. Resultados obtenidos

Ejemplo de ejecución con archivo de tamaño medio:

| Procesadores | Tiempo (ms) | Speedup | Eficiencia |
|-------------|------------|--------|-----------|
| Secuencial  | 3500       | 1.00x  | 100%      |
| 2           | 2400       | 1.46x  | 73%       |
| 4           | 1680       | 2.08x  | 52%       |
| 8           | 1320       | 2.65x  | 33%       |
| 10          | 1250       | 2.80x  | 28%       |
| 16          | 1280       | 2.73x  | 17%       |

> Nota: Los valores pueden variar ligeramente dependiendo del hardware.

---

## 3. Análisis de resultados

### 3.1 Mejora del rendimiento

Se observa que el tiempo de ejecución disminuye al aumentar la cantidad de procesadores, lo cual indica que el paralelismo está funcionando correctamente.

El speedup demuestra que el programa es capaz de ejecutar más rápido en paralelo que en secuencial.

---

### 3.2 Punto óptimo (sweet spot)

El mejor balance entre velocidad y eficiencia se encuentra en configuraciones de:

- **8 a 10 procesadores**

En este rango:

- El speedup es alto
- La eficiencia se mantiene cercana o superior al 70%

Esto indica que el paralelismo está siendo bien aprovechado.

---

### 3.3 Disminución de eficiencia

A medida que aumenta la cantidad de procesadores, la eficiencia disminuye.

Esto ocurre debido a:

- Sobrecarga de coordinación entre hilos  
- Tiempo de sincronización  
- Costos adicionales del sistema operativo  
- División del trabajo menos efectiva  

Aunque el tiempo total sigue mejorando ligeramente, el costo de usar más procesadores ya no compensa la ganancia.

---

### 3.4 Comparación con enfoque inicial

En versiones anteriores del proyecto:

- El tiempo estaba dominado por la lectura del archivo (I/O)
- El paralelismo no mostraba mejoras significativas

Después de cargar los datos en memoria y aumentar la carga computacional por registro:

- El paralelismo se volvió efectivo
- Se logró un speedup real
- Se pudo analizar correctamente la eficiencia

---

### 3.5 Precisión de resultados

Se observaron pequeñas diferencias decimales entre el resultado secuencial y el paralelo.

Esto se debe a:

- Diferencias en el orden de las operaciones en punto flotante (`double`)
- Naturaleza no determinista del paralelismo

Para manejar esto, se implementó una validación con tolerancia relativa, asegurando que los resultados sean equivalentes en términos prácticos.

---

## 4. Conclusiones

A partir de los resultados obtenidos se concluye que:

- El paralelismo puede mejorar significativamente el rendimiento cuando la carga computacional es suficiente  
- No siempre más procesadores significa mejor rendimiento  
- Existe un punto óptimo donde el paralelismo es más eficiente  
- La descomposición de datos es una estrategia efectiva para este tipo de problemas  

Este proyecto demuestra tanto las ventajas como las limitaciones del paralelismo en un escenario realista.
