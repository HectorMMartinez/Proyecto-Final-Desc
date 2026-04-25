# 📊 Análisis de Datos con Programación Paralela

Este proyecto implementa un sistema de análisis de datos de ventas en C# que compara el rendimiento entre ejecución secuencial y paralela utilizando múltiples procesadores.

El objetivo es demostrar en qué condiciones el paralelismo mejora el tiempo de ejecución y cómo varía la eficiencia al aumentar la cantidad de núcleos.

---

## 🚀 Características

- Procesamiento de archivos CSV de gran tamaño (hasta varios GB)
- Ejecución secuencial y paralela
- Cálculo de métricas financieras por registro:
  - Ingresos
  - Costos estimados
  - Margen
  - Clasificación de ventas
  - Score comercial
- Medición de rendimiento:
  - Tiempo de ejecución
  - Speedup
  - Eficiencia
- Validación automática de resultados entre ambos enfoques

---

## 🧠 Enfoque

El proyecto utiliza **descomposición de datos**, dividiendo el dataset en múltiples segmentos que se procesan de forma simultánea.

Esto permite evaluar el impacto del paralelismo en problemas con alta carga computacional.

---

## 📂 Estructura del proyecto

```
Proyecto-Final-Desc/
│
├── src/
├── data/input/
├── docs/
├── metrics/
└── test/
```

---

## 📥 Datos de entrada

Los datasets se encuentran en:

```
data/input/
```

Debido a su tamaño, los archivos están comprimidos.  
Es necesario descomprimirlos antes de ejecutar el sistema.

---

## ⚙️ Requisitos

- .NET 6 o superior
- Windows
- Visual Studio o VS Code (opcional)

---

## ▶️ Ejecución

Desde consola:

```
dotnet run
```

O desde Visual Studio ejecutando el proyecto.

El sistema mostrará un menú para seleccionar el dataset y el modo de ejecución.

---

## 📊 Resultados

El programa muestra en consola:

- Tiempo de ejecución
- Speedup
- Eficiencia
- Validación de resultados

Los resultados completos se encuentran en:

```
metrics/
```

---

## 🔍 Conclusión

El proyecto demuestra que el paralelismo puede mejorar significativamente el rendimiento cuando la carga computacional es suficiente, pero también evidencia que no escala de forma lineal y que existe un punto óptimo de eficiencia.

---

## 👨‍💻 Autor

Héctor Martínez
