# Dataset utilizado

## Descripción general

El proyecto utiliza un dataset de tipo transaccional simulado que representa ventas realizadas en una plataforma de comercio electrónico. Este dataset contiene información detallada sobre cada operación, incluyendo precios, cantidades, categorías de productos y ubicación del cliente.

El objetivo de utilizar este tipo de dataset es simular un escenario real donde se manejan grandes volúmenes de datos financieros, permitiendo evaluar el impacto del procesamiento paralelo en el rendimiento del sistema.

---

## Estructura del dataset

Cada registro del archivo CSV contiene las siguientes columnas:

- order_id → Identificador de la orden
- order_item_id → Identificador del ítem dentro de la orden
- product_id → Identificador del producto
- seller_id → Identificador del vendedor
- shipping_limit_date → Fecha límite de envío
- price → Precio del producto
- freight_value → Costo de envío
- quantity → Cantidad vendida
- product_category_name → Categoría del producto
- payment_value → Valor total pagado
- customer_state → Estado del cliente
- order_purchase_timestamp → Fecha de compra

Este conjunto de datos permite realizar distintos tipos de análisis, tanto simples (sumas, promedios) como más complejos (simulación de escenarios financieros, cálculo de márgenes y clasificación de ventas).

---

## Tamaños de los datasets utilizados

Para evaluar el comportamiento del sistema en diferentes condiciones, se utilizaron múltiples archivos con distintos tamaños:

- 15 MB (~100,000 registros)
- 76 MB (~500,000 registros)
- 153 MB (~1,000,000 registros)
- 700 MB (~5,000,000 registros)
- 3 GB (~20,000,000+ registros)

Esto permite observar cómo escala el rendimiento tanto en ejecución secuencial como paralela.

---

## Generación de datasets grandes

Los archivos de mayor tamaño no fueron obtenidos directamente de una fuente externa, sino generados a partir de datasets base mediante un proceso de expansión controlada.

### Estrategia utilizada

1. Se partió de un dataset base en formato CSV.
2. Se replicaron los registros múltiples veces.
3. Durante la replicación, se aplicaron ligeras variaciones en los valores numéricos para evitar datos completamente idénticos.
4. Se guardaron los resultados en nuevos archivos con tamaños crecientes.

### Objetivo de este enfoque

Este proceso permitió:

- Simular cargas de trabajo realistas
- Evitar sesgos por datasets pequeños
- Generar volúmenes suficientemente grandes para justificar el uso de paralelismo
- Evaluar el impacto del tamaño del dataset en el rendimiento

---

## Uso del dataset en el sistema

El dataset es cargado completamente en memoria antes del análisis. Esto permite:

- Eliminar el cuello de botella de entrada/salida (I/O)
- Enfocar la medición en el tiempo de procesamiento
- Obtener resultados más precisos al comparar ejecución secuencial vs paralela

---

## Justificación del dataset en el contexto del proyecto

El uso de datos de ventas permite simular un problema del mundo real donde:

- Se procesan grandes volúmenes de información
- Se requieren cálculos financieros complejos
- El tiempo de respuesta es importante

Esto cumple con uno de los requisitos principales del proyecto: aplicar paralelismo a un problema realista y medible :contentReference[oaicite:1]{index=1}.

---

## Conclusión

El dataset utilizado permite evaluar correctamente el impacto del paralelismo, ya que combina:

- Volumen de datos significativo
- Operaciones computacionalmente intensivas
- Escenarios realistas de análisis financiero

Gracias a esto, es posible demostrar de manera clara cuándo el paralelismo resulta beneficioso y cuándo no.
