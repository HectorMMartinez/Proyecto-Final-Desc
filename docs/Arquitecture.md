# Arquitectura del Sistema

## 1. Descripción general

Este proyecto implementa un sistema de análisis de datos de ventas utilizando descomposición de datos y programación paralela en C#.

El programa permite:

- Cargar archivos CSV de gran tamaño (hasta varios GB)
- Procesar los datos de forma secuencial
- Procesar los mismos datos de forma paralela utilizando múltiples núcleos
- Comparar el rendimiento entre ambos enfoques mediante métricas como speedup y eficiencia

El objetivo principal es demostrar cuándo el paralelismo aporta ventajas reales y cuándo deja de ser eficiente.

---

## 2. Estructura del proyecto

Proyecto-Final-Desc/

- src/
  - Program.cs
  - Models/
    - RegistroVenta.cs
    - ResultadoSimulacion.cs
    - MetadataArchivo.cs
  - Services/
    - ArchivoService.cs
    - SimulacionVentaService.cs
    - CalculadoraVenta.cs
    - MetricaService.cs
  - Utils/
    - FileHelper.cs

- data/
  - input/

- docs/
- metrics/
- test/

---

## 3. Flujo del programa

El flujo general del sistema es el siguiente:

1. El usuario selecciona un archivo CSV desde el menú  
2. Se muestra la metadata del archivo (tamaño, columnas, registros)  
3. El archivo es cargado en memoria  
4. Se ejecuta el análisis secuencial  
5. Se ejecuta el análisis paralelo (automático o personalizado)  
6. Se calculan métricas de rendimiento:
   - Tiempo de ejecución  
   - Speedup  
   - Eficiencia  
7. Se validan los resultados entre ambos enfoques  

---

## 4. Componentes principales

### 4.1 Program.cs

Es el punto de entrada del sistema.

Responsabilidades:

- Controlar el flujo general del programa  
- Solicitar entrada del usuario  
- Ejecutar los análisis secuencial y paralelo  
- Mostrar los resultados en consola  

---

### 4.2 ArchivoService

Encargado de la gestión de archivos.

Funciones principales:

- Mostrar el menú de selección de archivos  
- Leer archivos CSV  
- Obtener metadata (tamaño, columnas, cantidad de registros)  

---

### 4.3 SimulacionVentaService

Controla la ejecución del análisis de datos.

Incluye:

- Ejecución secuencial  
- Ejecución paralela utilizando múltiples hilos  

En el modo paralelo, los datos se dividen entre varios núcleos para ser procesados simultáneamente.

---

### 4.4 CalculadoraVenta

Contiene la lógica de negocio aplicada a cada registro de venta.

Realiza cálculos como:

- Ingreso (revenue)  
- Costo estimado  
- Margen  
- Clasificación de la venta  
- Evaluación de escenarios de descuento  
- Cálculo de un score comercial  

Esta separación permite mantener organizada la lógica del sistema.

---

### 4.5 MetricaService

Encargado de evaluar el rendimiento del sistema.

Calcula:

- Speedup → mejora de tiempo respecto al secuencial  
- Eficiencia → uso real de los procesadores  
- Validación de resultados (con tolerancia para errores de precisión)  

---

## 5. Estrategia de paralelización

El proyecto utiliza **descomposición de datos**.

Esto significa que:

- El dataset completo se divide en partes  
- Cada parte es procesada por un hilo diferente  
- Luego los resultados se combinan  

Este enfoque es adecuado cuando:

- Cada registro puede procesarse de forma independiente  
- El costo computacional por registro es significativo  

---

## 6. Manejo de concurrencia

Para evitar conflictos entre hilos:

- Cada hilo trabaja sobre su propia porción de datos  
- Los resultados parciales se combinan al final  
- Se evita el acceso concurrente a estructuras compartidas  

Esto reduce problemas de sincronización y mejora el rendimiento.

---

## 7. Consideraciones de diseño

Durante el desarrollo se tomaron en cuenta:

- Separación de responsabilidades (servicios independientes)  
- Uso de estructuras simples para facilitar la comprensión  
- Procesamiento en memoria para eliminar el cuello de botella del disco  
- Validación de resultados con tolerancia para evitar errores por precisión decimal  

---

## 8. Limitaciones

- El uso de memoria puede ser alto para archivos muy grandes  
- La eficiencia del paralelismo depende del costo computacional del problema  
- A partir de cierto número de núcleos, la eficiencia disminuye debido a la sobrecarga de coordinación  

---

## 9. Conclusión

La arquitectura del sistema permite evaluar de forma clara el impacto del paralelismo en un problema realista, mostrando tanto sus ventajas como sus limitaciones.
