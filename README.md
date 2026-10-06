# Guarida Goblin - Sistema de Venta

Sistema de inventario y ventas para tienda especializada en juegos de mesa, manuales de Dungeons & Dragons (D&D) y figuras coleccionables geek, ubicada en Sucre, Bolivia.

##  Descripción del Negocio

**Guarida Goblin** es un nicho enfocado en la cultura geek y de rol en la ciudad de Sucre. Dado que en Bolivia existen muy pocos distribuidores oficiales de estos productos, el modelo de negocio se basa principalmente en la **importación bajo pedido y stock limitado**.

### Características clave del modelo de negocio:
1. **Abastecimiento por Importación:** La mayoría de los productos se adquieren mediante plataformas internacionales (Amazon, MercadoLibre México/USA/España) o importadores directos.
2. **Gestión de Pedidos Internacionales:** Es crucial rastrear el estado de los pedidos (En camino, En aduana, Bolivia) y los costos de envío internacionales.
3. **Manejo de Moneda:** Los costos se calculan en Dólares (USD) por las compras internacionales, pero las ventas locales se realizan en Bolivianos (Bs).
4. **Venta Local:** Atención al cliente en tienda física en Sucre.

## Objetivos del Sistema
- Controlar el stock real de productos físicos en la tienda.
- Calcular costos reales considerando el tipo de cambio y gastos de envío.
- Registrar ventas locales y mantener un historial de clientes.

---

##  Modelo de Datos (Entidades y Campos Tentativos)

A continuación, se detalla la estructura inicial de la base de datos.

### 1. Producto
Almacena la información de los juegos, libros y figuras.
| Campo	| Tipo de Dato	| Descripción |
|------	|-------------	|-------------		|
| `id` 	| INT (PK) 	| Identificador único. 	|
| `codigo` | VARCHAR 	| Código de barras o SKU interno. |
| `nombre` | VARCHAR 	| Nombre del producto (ej: "Player's Handbook 5e"). |
| `descripcion` | TEXT 	| Descripción detallada, número de jugadores, idioma, etc. |
| `categoria_id` | INT (FK) | Relación con la tabla Categoría. |
| `precio_costo_usd` | DECIMAL | Costo de adquisición en dólares (para importaciones). |
| `precio_venta_bs` | DECIMAL | Precio de venta al público en bolivianos. |
| `stock_actual` | INT | Cantidad disponible en tienda. |
| `stock_minimo` | INT | Punto de reorden para hacer nuevos pedidos. |
| `peso_kg` | DECIMAL | Peso del producto (vital para calcular envíos de Amazon). |
| `origen` | VARCHAR | Plataforma de compra (Amazon, MercadoLibre, Local). |
| `estado` | INT | 1: Activo, 0: Inactivo/Agotado. |

### 2. Categoría
Clasificación de los productos.
| Campo | Tipo de Dato 	| Descripción |
|-------|-------------	|-------------|
| `id`	| INT (PK) 	| Identificador único. |
| `nombre` | VARCHAR | (ej: "Juegos de Mesa", "Libros D&D", "Figuras", "Accesorios"). |

### 3. Proveedor
Entidades de donde se compran los productos.
| Campo | Tipo de Dato | Descripción |
|-------|-------------|-------------|
| `id` | INT (PK) | Identificador único. |
| `nombre` | VARCHAR | Nombre del proveedor o plataforma. |
| `tipo` | VARCHAR | "Internacional" (Amazon/ML) o "Local" (Distribuidor en Bolivia). |
| `plataforma` | VARCHAR | (ej: "Amazon.com", "MercadoLibre.mx", "Distribuidor X"). |
| `contacto` | VARCHAR | Correo o teléfono de contacto. |

### 4. PedidoImportacion (Crítico para este negocio)
Registra las compras hechas a Amazon/MercadoLibre para traer stock.
| Campo | Tipo de Dato | Descripción |
|-------|-------------|-------------|
| `id` | INT (PK) | Identificador único. |
| `proveedor_id` | INT (FK) | Proveedor/Plataforma de la compra. |
| `fecha_pedido` | DATETIME | Fecha en que se hizo la compra. |
| `fecha_estimada` | DATETIME | Fecha estimada de llegada a Sucre. |
| `costo_productos_usd`| DECIMAL | Subtotal de los productos en USD. |
| `costo_envio_usd` | DECIMAL | Costo de envío internacional en USD. |
| `tasa_cambio` | DECIMAL | Tipo de cambio aplicado al momento de la compra. |
| `tracking_number` | VARCHAR | Número de seguimiento del paquete. |
| `estado` | VARCHAR | "Pendiente", "En camino", "En Aduana", "Recibido", "Cancelado". |

### 5. Cliente
Compradores locales de Sucre.
| Campo | Tipo de Dato | Descripción |
|-------|-------------|-------------|
| `id` | INT (PK) | Identificador único. |
| `cedula_identidad` | VARCHAR | CI del cliente (opcional para facturas). |
| `nombres` | VARCHAR | Nombres. |
| `apellidos` | VARCHAR | Apellidos. |
| `telefono` | VARCHAR | Número de celular (para coordinar entregas). |
| `direccion` | VARCHAR | Dirección en Sucre para envíos locales. |
| `estado` | INT | 1: Activo, 0: Inactivo. |

### 6. Venta
Registro de las transacciones en tienda.
| Campo | Tipo de Dato | Descripción |
|-------|-------------|-------------|
| `id` | INT (PK) | Identificador único. |
| `cliente_id` | INT (FK) | Cliente que realiza la compra. |
| `usuario_id` | INT (FK) | Empleado que registró la venta. |
| `fecha` | DATETIME | Fecha y hora de la venta. |
| `total_bob` | DECIMAL | Monto total en bolivianos. |
| `estado` | INT | 1: Completada, 0: Anulada. |

### 7. DetalleVenta
Productos incluidos en cada venta.
| Campo | Tipo de Dato | Descripción |
|-------|-------------|-------------|
| `id` | INT (PK) | Identificador único. |
| `venta_id` | INT (FK) | Relación con la Venta. |
| `producto_id` | INT (FK) | Relación con el Producto. |
| `cantidad` | INT | Cantidad vendida. |
| `precio_unitario` | DECIMAL | Precio en BOB al momento de la venta. |
| `subtotal` | DECIMAL | Cantidad * Precio unitario. |

---
