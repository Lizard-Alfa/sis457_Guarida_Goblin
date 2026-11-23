-- DDL - TIENDA "GUARIDA GOBLIN" (SUCRE, BOLIVIA)

CREATE DATABASE Guarida
GO
USE master
GO
CREATE LOGIN usrgoblin WITH PASSWORD=N'123456',
DEFAULT_DATABASE=Guarida,
CHECK_EXPIRATION=OFF,
CHECK_POLICY=ON
GO
USE Guarida
GO
CREATE USER usrgoblin FOR LOGIN usrgoblin
GO
ALTER ROLE [db_owner] ADD MEMBER usrgoblin
GO

-- 1. LIMPIEZA DE TABLAS
DROP TABLE IF EXISTS VentaDetalle;
DROP TABLE IF EXISTS Venta;
DROP TABLE IF EXISTS CompraDetalle;
DROP TABLE IF EXISTS Compra;
DROP TABLE IF EXISTS Producto;
DROP TABLE IF EXISTS Proveedor;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Empleado;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS Categoria;
DROP TABLE IF EXISTS DescuentoDetalle;
DROP TABLE IF EXISTS Descuento;
GO

-- 2. CREACIÓN DE TABLAS MAESTRAS

CREATE TABLE Categoria (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50) NOT NULL,
    descripcion VARCHAR(250) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

CREATE TABLE Cliente (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    cedulaIdentidad VARCHAR(15) NULL,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NULL,
    ciudad VARCHAR(50) NULL, -- AGREGADO para distribución geográfica
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

CREATE TABLE Empleado (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    cedulaIdentidad VARCHAR(15) NOT NULL,
    nombres VARCHAR(50) NOT NULL,
    primerApellido VARCHAR(30) NULL,
    segundoApellido VARCHAR(30) NULL,
    fechaNacimiento DATE NOT NULL,
    direccion VARCHAR(150) NOT NULL,
    celular VARCHAR(15) NOT NULL,
    cargo VARCHAR(50) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT uk_Empleado_CI UNIQUE (cedulaIdentidad)
);

CREATE TABLE Usuario (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idEmpleado INT NOT NULL,
    usuario VARCHAR(20) NOT NULL,
    clave VARCHAR(255) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_Usuario_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(id),
    CONSTRAINT uk_Usuario_nombre UNIQUE (usuario)
);

CREATE TABLE Proveedor (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    tipo VARCHAR(20) NOT NULL CHECK (tipo IN ('Nacional', 'Internacional')),
    razonSocial VARCHAR(150) NOT NULL,
    nit VARCHAR(20) NULL,
    contacto VARCHAR(100) NULL,
    email VARCHAR(100) NULL,
    telefono VARCHAR(20) NULL,
    pais VARCHAR(50) NULL,
    ciudad VARCHAR(50) NULL,      -- AGREGADO
    direccion VARCHAR(150) NULL,   -- AGREGADO
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

-- 3. TABLA DE DESCUENTOS (NUEVA)
CREATE TABLE Descuento (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    porcentaje DECIMAL(5,2) NOT NULL,
    fechaInicio DATE NOT NULL,
    fechaFin DATE NOT NULL,
    descripcion VARCHAR(250) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

-- 4. TABLA DE PRODUCTOS
CREATE TABLE Producto (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idCategoria INT NOT NULL,
    codigo VARCHAR(30) NOT NULL,
    imagenUrl VARCHAR(500) NULL,
    nombre VARCHAR(150) NOT NULL,
    descripcion VARCHAR(1000) NULL,
    pesoKg DECIMAL(5,2) NULL,
    precioCostoUsd DECIMAL(10,2) NULL,
    precioVentaBob DECIMAL(10,2) NOT NULL,
    stockActual INT NOT NULL DEFAULT 0,
    stockMinimo INT NOT NULL DEFAULT 1,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_Producto_Categoria FOREIGN KEY (idCategoria) REFERENCES Categoria(id),
    CONSTRAINT uk_Producto_codigo UNIQUE (codigo)
);

-- 5. TABLAS DE TRANSACCIONES
CREATE TABLE Compra (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idProveedor INT NOT NULL,
    fecha DATE NOT NULL DEFAULT GETDATE(),
    subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    total DECIMAL(10,2) NOT NULL,
    trackingNumber VARCHAR(100) NULL,
    estadoEnvio VARCHAR(50) NULL,
    fechaEstimada DATE NULL,
    observaciones VARCHAR(500) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_Compra_Proveedor FOREIGN KEY (idProveedor) REFERENCES Proveedor(id)
);

CREATE TABLE CompraDetalle (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idCompra INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_CD_Compra FOREIGN KEY (idCompra) REFERENCES Compra(id),
    CONSTRAINT fk_CD_Producto FOREIGN KEY (idProducto) REFERENCES Producto(id)
);

CREATE TABLE Venta (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idCliente INT NOT NULL,
    idUsuario INT NOT NULL,
    idDescuento INT NULL, -- AGREGADO: Relación con Descuento
    transaccion INT NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    montoDescuentoAplicado DECIMAL(10,2) NOT NULL DEFAULT 0, -- AGREGADO
    total DECIMAL(10,2) NOT NULL,
    metodoPago VARCHAR(50) NOT NULL DEFAULT 'Efectivo',
    observaciones VARCHAR(500) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- 1: Activo, 0: Anulado
    CONSTRAINT fk_Venta_Cliente FOREIGN KEY (idCliente) REFERENCES Cliente(id),
    CONSTRAINT fk_Venta_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(id),
    CONSTRAINT fk_Venta_Descuento FOREIGN KEY (idDescuento) REFERENCES Descuento(id)
);

CREATE TABLE VentaDetalle (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idVenta INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_VD_Venta FOREIGN KEY (idVenta) REFERENCES Venta(id),
    CONSTRAINT fk_VD_Producto FOREIGN KEY (idProducto) REFERENCES Producto(id)
);
GO

CREATE TABLE DescuentoDetalle (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idDescuento INT NOT NULL,
    idProducto INT NOT NULL,

    -- Esta tabla solo sirve para decir: "Este descuento aplica a ESTE producto".
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    
    CONSTRAINT fk_DD_Descuento FOREIGN KEY (idDescuento) REFERENCES Descuento(id),
    CONSTRAINT fk_DD_Producto FOREIGN KEY (idProducto) REFERENCES Producto(id)
);
GO

-- 6. ÍNDICES DE RENDIMIENTO
CREATE INDEX idx_Producto_nombre ON Producto(nombre);
CREATE INDEX idx_Producto_codigo ON Producto(codigo);
CREATE INDEX idx_Cliente_nombres ON Cliente(nombres);
CREATE INDEX idx_Cliente_ciudad ON Cliente(ciudad);
CREATE INDEX idx_Proveedor_razonSocial ON Proveedor(razonSocial);
CREATE INDEX idx_Venta_fecha ON Venta(fecha);
CREATE INDEX idx_Compra_fecha ON Compra(fecha);
CREATE INDEX idx_Descuento_fechas ON Descuento(fechaInicio, fechaFin);
CREATE INDEX idx_DescuentoDetalle_Producto ON DescuentoDetalle(idProducto);
GO

-- 7. PROCEDIMIENTOS ALMACENADOS
-- Listar productos
DROP PROC IF EXISTS paProductoListar;
GO
CREATE PROC paProductoListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT p.id, p.idCategoria, c.nombre AS categoria, p.codigo, p.nombre, p.descripcion, 
           p.imagenUrl,
           p.pesoKg, p.precioCostoUsd, p.precioVentaBob, 
           p.stockActual, p.stockMinimo, 
           p.usuarioRegistro, p.fechaRegistro, p.estado
    FROM Producto p
    LEFT JOIN Categoria c ON c.id = p.idCategoria
    WHERE p.estado = 1
    AND (p.codigo + ' ' + p.nombre + ' ' + ISNULL(p.descripcion,'') + ' ' + c.nombre) LIKE '%' + @parametro + '%'
    ORDER BY p.nombre;
END
GO

-- Listar clientes
DROP PROC IF EXISTS paClienteListar;
GO
CREATE PROC paClienteListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT id, cedulaIdentidad, nombres, apellidos, ciudad, usuarioRegistro, fechaRegistro, estado
    FROM Cliente
    WHERE estado = 1
    AND (nombres + ' ' + ISNULL(apellidos,'') + ' ' + ISNULL(cedulaIdentidad,'') + ' ' + ISNULL(ciudad,'')) LIKE '%' + @parametro + '%'
    ORDER BY nombres;
END
GO

-- Listar proveedores
DROP PROC IF EXISTS paProveedorListar;
GO
CREATE PROC paProveedorListar @tipo VARCHAR(20), @parametro VARCHAR(100)
AS
BEGIN
    SELECT id, tipo, razonSocial, nit, contacto, email, telefono, pais, ciudad, usuarioRegistro, fechaRegistro, estado
    FROM Proveedor
    WHERE estado = 1
    AND (@tipo = 'AMBOS' OR tipo = @tipo)
    AND (razonSocial + ' ' + ISNULL(nit,'') + ' ' + ISNULL(contacto,'') + ' ' + ISNULL(ciudad,'')) LIKE '%' + @parametro + '%'
    ORDER BY razonSocial;
END
GO

-- Listar empleados
DROP PROC IF EXISTS paEmpleadoListar;
GO
CREATE PROC paEmpleadoListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT id, cedulaIdentidad, nombres, primerApellido, segundoApellido, fechaNacimiento, direccion, celular, cargo, usuarioRegistro, fechaRegistro, estado
    FROM Empleado
    WHERE estado = 1
    AND (cedulaIdentidad + ' ' + nombres + ' ' + ISNULL(primerApellido,'')) LIKE '%' + @parametro + '%'
    ORDER BY nombres;
END
GO

-- Validar usuario
DROP PROC IF EXISTS paUsuarioValidar;
GO
CREATE PROC paUsuarioValidar @usuario VARCHAR(20), @clave VARCHAR(255)
AS
BEGIN
    SELECT u.id, u.idEmpleado, u.usuario, u.clave,
           e.nombres + ' ' + e.primerApellido AS nombreCompleto, e.cargo
    FROM Usuario u
    INNER JOIN Empleado e ON e.id = u.idEmpleado
    WHERE u.usuario = @usuario AND u.clave = @clave AND u.estado = 1 AND e.estado = 1;
END
GO

-- Listar Ventas
DROP PROC IF EXISTS paVentaListar;
GO
CREATE PROC paVentaListar
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL
AS
BEGIN
    SELECT v.id, v.fecha, v.idCliente, v.transaccion, v.metodoPago, v.subtotal, v.montoDescuentoAplicado, v.total, v.estado,
           u.usuario, c.nombres AS cliente, d.nombre AS nombreDescuento, v.observaciones, v.usuarioRegistro, v.fechaRegistro
    FROM Venta v
    INNER JOIN Cliente c ON v.idCliente = c.id
    INNER JOIN Usuario u ON v.idUsuario = u.id
    LEFT JOIN Descuento d ON v.idDescuento = d.id
    WHERE v.estado IN (1, 0)
    AND (@fechaInicio IS NULL OR v.fecha >= CAST(@fechaInicio AS DATE))
    AND (@fechaFin IS NULL OR v.fecha < DATEADD(DAY, 1, CAST(@fechaFin AS DATE)))
    ORDER BY v.fecha DESC;
END
GO

-- Listar Descuentos Activos
DROP PROC IF EXISTS paDescuentoListarActivos;
GO
CREATE PROC paDescuentoListarActivos
AS
BEGIN
    SELECT id, nombre, porcentaje, fechaInicio, fechaFin, descripcion
    FROM Descuento
    WHERE estado = 1 
    AND CAST(GETDATE() AS DATE) BETWEEN fechaInicio AND fechaFin
    ORDER BY fechaInicio DESC;
END
GO

-- Registrar/Actualizar Descuento (Validación de fechas cruzadas)
DROP PROC IF EXISTS paDescuentoRegistrar;
GO

CREATE PROC paDescuentoRegistrar
    @id INT = NULL,
    @nombre VARCHAR(100),
    @porcentaje DECIMAL(5,2),
    @fechaInicio DATE,
    @fechaFin DATE,
    @descripcion VARCHAR(250),
    @usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validar que no se crucen fechas con otro descuento activo
    IF EXISTS (
        SELECT 1 FROM Descuento 
        WHERE estado = 1 
        AND id <> ISNULL(@id, 0) 
        AND (@fechaInicio <= fechaFin AND @fechaFin >= fechaInicio)
    )
    BEGIN
        RAISERROR('El rango de fechas se cruza con otro descuento ya activo.', 16, 1);
        RETURN;
    END

    IF @id IS NULL
    BEGIN
        INSERT INTO Descuento (nombre, porcentaje, fechaInicio, fechaFin, descripcion, usuarioRegistro)
        VALUES (@nombre, @porcentaje, @fechaInicio, @fechaFin, @descripcion, @usuario);
    END
    ELSE
    BEGIN
        UPDATE Descuento 
        SET nombre = @nombre, 
            porcentaje = @porcentaje, 
            fechaInicio = @fechaInicio, 
            fechaFin = @fechaFin, 
            descripcion = @descripcion
        WHERE id = @id;
    END
END
GO

-- 8. DATOS DE PRUEBA - SDD
-- Categorías
INSERT INTO Categoria (nombre, descripcion) VALUES
('Juegos de Mesa', 'Juegos de mesa y cartas'),
('Manuales de Rol', 'Manuales de D&D y libros de rol'),
('Figuras y Miniaturas', 'Figuras coleccionables y miniaturas'),
('Juegos de Cartas', 'Juegos de cartas coleccionables'),
('Accesorios Geek', 'Dados, tapetes y otros accesorios');

-- Empleados (3: Admin, Cajero Mañana, Cajero Tarde)
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, fechaNacimiento, direccion, celular, cargo) VALUES
('10000000', 'Administrador', 'Sistema', '1990-01-01', 'Oficina Central', '70000000', 'Administrador'),
('20000000', 'Cajero', 'Mañana', '1995-05-15', 'Sucre', '71111111', 'Cajero Turno Mañana'),
('30000000', 'Cajero', 'Tarde', '1998-08-20', 'Sucre', '72222222', 'Cajero Turno Tarde');

-- Usuarios (Contraseña por defecto para los 3: "juego123" -> hash: s1udM8c0+VgaifwGKuOcBUMvh0U8nyJQt6XKeBTNb2s=)
INSERT INTO Usuario (idEmpleado, usuario, clave) VALUES
(1, 'admin', 's1udM8c0+VgaifwGKuOcBUMvh0U8nyJQt6XKeBTNb2s='),
(2, 'cajero_m', 's1udM8c0+VgaifwGKuOcBUMvh0U8nyJQt6XKeBTNb2s='),
(3, 'cajero_t', 's1udM8c0+VgaifwGKuOcBUMvh0U8nyJQt6XKeBTNb2s=');

-- Proveedores Nacionales (6: 1 La Paz, 2 Cochabamba, 3 Santa Cruz)
INSERT INTO Proveedor (tipo, razonSocial, nit, contacto, telefono, email, pais, ciudad, direccion) VALUES
('Nacional', 'Distribuidora de Juegos La Paz', '1001001', 'Juan Perez', '2222222', 'ventas@juegoslapaz.bo', 'Bolivia', 'La Paz', 'Av. 16 de Julio #123'),
('Nacional', 'Juegos y Más CBBA', '1002002', 'Maria Lopez', '4444444', 'info@juegosycbba.bo', 'Bolivia', 'Cochabamba', 'Calle España #456'),
('Nacional', 'El Mercader Errante CBBA', '1003003', 'Carlos Ruiz', '4555555', 'sucursalcbb@mercadererrante.bo', 'Bolivia', 'Cochabamba', 'Av. Ayacucho #789'),
('Nacional', 'Geek Store SCZ', '1004004', 'Ana Torres', '3333333', 'ventas@geekstore.bo', 'Bolivia', 'Santa Cruz', 'Av. Banzer #100'),
('Nacional', 'Dragon Dice SCZ', '1005005', 'Luis Gomez', '3444444', 'contacto@dragondice.bo', 'Bolivia', 'Santa Cruz', 'Calle Murillo #200'),
('Nacional', 'Rol y Dados SCZ', '1006006', 'Pedro Sanchez', '3555555', 'info@rolydados.bo', 'Bolivia', 'Santa Cruz', 'Av. Cristóbal de Mendoza #300');

-- Proveedores Internacionales (7: 2 Devir, Amazon, AliExpress, Mercado Libre, Temu, eBay)
INSERT INTO Proveedor (tipo, razonSocial, contacto, telefono, email, pais, ciudad) VALUES
('Internacional', 'Devir Argentina', 'Arkana Comics', '+54 (11) 4784 3284', 'contacto@devir.com.ar', 'Argentina', 'Buenos Aires'),
('Internacional', 'Devir España', 'Soporte Devir', '+34 (93) 238 9870', 'devir.es@devir.com', 'España', 'Barcelona'),
('Internacional', 'Amazon.com', 'Soporte Amazon', '+1 (206) 764-9499', 'sales.usa@amazon.com', 'USA', 'Seattle'),
('Internacional', 'AliExpress Global', 'Atención al Cliente', '+86 571 8502 2088', 'service@aliexpress.com', 'China', 'Hangzhou'),
('Internacional', 'Mercado Libre Internacional', 'Soporte Vendedores', '+54 (11) 5555 1234', 'soporte@mercadolibre.com', 'Argentina', 'Buenos Aires'),
('Internacional', 'Temu Global', 'Atención al Cliente', '+86 21 6056 8888', 'support@temu.com', 'China', 'Shanghai'),
('Internacional', 'eBay Inc.', 'Soporte Global', '+1 (408) 376-7008', 'support@ebay.com', 'USA', 'San Jose');

-- Clientes (45: 20 Sucre, 15 Potosí, 10 Cochabamba, 6 Santa Cruz, 4 Oruro)
DECLARE @i INT = 1;
WHILE @i <= 20 BEGIN
    INSERT INTO Cliente (cedulaIdentidad, nombres, apellidos, ciudad) VALUES (CAST(1000000 + @i AS VARCHAR), 'Cliente', 'Sucre ' + CAST(@i AS VARCHAR), 'Sucre');
    SET @i = @i + 1;
END
SET @i = 1;
WHILE @i <= 15 BEGIN
    INSERT INTO Cliente (cedulaIdentidad, nombres, apellidos, ciudad) VALUES (CAST(2000000 + @i AS VARCHAR), 'Cliente', 'Potosi ' + CAST(@i AS VARCHAR), 'Potosí');
    SET @i = @i + 1;
END
SET @i = 1;
WHILE @i <= 10 BEGIN
    INSERT INTO Cliente (cedulaIdentidad, nombres, apellidos, ciudad) VALUES (CAST(3000000 + @i AS VARCHAR), 'Cliente', 'Cochabamba ' + CAST(@i AS VARCHAR), 'Cochabamba');
    SET @i = @i + 1;
END
SET @i = 1;
WHILE @i <= 6 BEGIN
    INSERT INTO Cliente (cedulaIdentidad, nombres, apellidos, ciudad) VALUES (CAST(4000000 + @i AS VARCHAR), 'Cliente', 'SantaCruz ' + CAST(@i AS VARCHAR), 'Santa Cruz');
    SET @i = @i + 1;
END
SET @i = 1;
WHILE @i <= 4 BEGIN
    INSERT INTO Cliente (cedulaIdentidad, nombres, apellidos, ciudad) VALUES (CAST(5000000 + @i AS VARCHAR), 'Cliente', 'Oruro ' + CAST(@i AS VARCHAR), 'Oruro');
    SET @i = @i + 1;
END

-- Descuento de Ejemplo (Navidad)
INSERT INTO Descuento (nombre, porcentaje, fechaInicio, fechaFin, descripcion) VALUES
('Promoción Navidad', 5.00, '2026-12-20', '2026-12-26', 'Descuento en la venta de cualquier producto por las fiestas navideñas');

-- 100 Productos (Mismos del script anterior, optimizados)
INSERT INTO Producto (idCategoria, codigo, imagenUrl, nombre, descripcion, pesoKg, precioCostoUsd, precioVentaBob, stockActual, stockMinimo) VALUES
(1, 'JDM-CATAN', NULL, 'Juego de Mesa Catan', 'Juego de estrategia y comercio', 1.50, 40.00, 400.00, 3, 1),
(1, 'JDM-DTL01', NULL, 'Juego de Mesa Distilled', 'Juego de estrategia', 1.80, 130.00, 1500.00, 4, 1),
(1, 'JDM-AGRC4', NULL, 'Juego de Mesa Agricola', 'Juego de comercio', 2.30, 240.00, 2800.00, 2, 1),
(1, 'JDM-PAMP3', NULL, 'Juego de Mesa Pampero', 'Juego de rol y avance', 1.80, 215.00, 2500.00, 3, 1),
(1, 'JDM-TWIL', NULL, 'Twilight Imperium 4ta Ed', 'Estrategia espacial épica', 3.50, 150.00, 1750.00, 2, 1),
(1, 'JDM-GLOOM', NULL, 'Gloomhaven', 'RPG táctico cooperativo', 4.20, 140.00, 1650.00, 1, 1),
(1, 'JDM-PANDEM', NULL, 'Pandemic', 'Cooperativo salvar el mundo', 1.20, 40.00, 480.00, 5, 2),
(1, 'JDM-RISK', NULL, 'Risk Classic', 'Conquista mundial', 1.80, 45.00, 520.00, 4, 2),
(1, 'JDM-SCRAB', NULL, 'Scrabble', 'Juego de palabras', 1.00, 25.00, 300.00, 8, 3),
(1, 'JDM-MONOP', NULL, 'Monopoly Classic', 'Juego inmobiliario', 1.10, 30.00, 350.00, 6, 2),
(1, 'JDM-CARCAS', NULL, 'Carcassonne', 'Construcción de ciudades', 1.00, 35.00, 400.00, 7, 2),
(1, 'JDM-7WONDR', NULL, '7 Wonders', 'Construcción de civilizaciones', 1.20, 50.00, 580.00, 4, 2),
(1, 'JDM-TICKET', NULL, 'Ticket to Ride', 'Rutas ferroviarias', 1.30, 55.00, 640.00, 5, 2),
(1, 'JDM-AZUL', NULL, 'Azul', 'Estrategia de azulejos', 1.10, 40.00, 470.00, 6, 2),
(1, 'JDM-SPLNDR', NULL, 'Splendor', 'Comercio de gemas', 1.00, 40.00, 470.00, 5, 2),
(1, 'JDM-EVOLVE', NULL, 'Evolution', 'Evolución de especies', 0.90, 35.00, 410.00, 4, 2),
(1, 'JDM-DIXIT', NULL, 'Dixit', 'Juego narrativo ilustrado', 0.80, 35.00, 410.00, 6, 2),
(1, 'JDM-CODENM', NULL, 'Codenames', 'Juego de palabras en equipo', 0.60, 20.00, 240.00, 10, 3),
(1, 'JDM-EXPLOD', NULL, 'Exploding Kittens', 'Juego de cartas explosivo', 0.30, 20.00, 240.00, 12, 4),
(1, 'JDM-UNO', NULL, 'UNO Classic', 'Juego de cartas familiar', 0.20, 10.00, 120.00, 15, 5),
(1, 'JDM-JENGA', NULL, 'Jenga Classic', 'Torre de bloques', 1.00, 15.00, 180.00, 10, 3),
(1, 'JDM-CLUE', NULL, 'Clue', 'Misterio y deducción', 0.90, 25.00, 300.00, 7, 2),
(1, 'JDM-BATTLE', NULL, 'Battleship', 'Hundir la flota', 0.80, 20.00, 240.00, 8, 3),
(1, 'JDM-OPERAT', NULL, 'Operation', 'Juego de destreza', 0.70, 20.00, 240.00, 6, 2),
(1, 'JDM-TRIVIA', NULL, 'Trivial Pursuit', 'Juego de preguntas', 1.20, 35.00, 410.00, 5, 2),
(1, 'JDM-PUZZLE', NULL, 'Puzzle 1000 piezas', 'Rompecabezas paisaje', 0.80, 15.00, 180.00, 12, 4),
(1, 'JDM-CHESS', NULL, 'Ajedrez de madera', 'Juego clásico de estrategia', 1.50, 30.00, 350.00, 6, 2),
(1, 'JDM-BACKGM', NULL, 'Backgammon', 'Juego de tablero clásico', 1.20, 25.00, 300.00, 5, 2),
(1, 'JDM-GO', NULL, 'Juego de Go', 'Estrategia milenaria', 0.80, 40.00, 470.00, 4, 2),
(1, 'JDM-MAHJNG', NULL, 'Mahjong', 'Juego chino tradicional', 1.50, 35.00, 410.00, 3, 1),
(1, 'JDM-DOMINO', NULL, 'Dominó clásico', 'Juego de fichas', 0.80, 12.00, 140.00, 10, 3),
(1, 'JDM-YAHTZE', NULL, 'Yahtzee', 'Juego de dados', 0.50, 15.00, 180.00, 8, 3),
(1, 'JDM-CRANIUM', NULL, 'Cranium', 'Juego de habilidades', 1.00, 30.00, 350.00, 5, 2),
(1, 'JDM-BOGGLE', NULL, 'Boggle', 'Juego de palabras rápido', 0.40, 15.00, 180.00, 7, 2),
(1, 'JDM-PARCH', NULL, 'Parchís', 'Juego de parcours', 0.60, 12.00, 140.00, 9, 3),
(1, 'JDM-OTELLO', NULL, 'Othello', 'Juego de discos', 0.50, 18.00, 210.00, 6, 2),
(1, 'JDM-ABALON', NULL, 'Abalone', 'Juego de canicas estratégicas', 1.20, 35.00, 410.00, 4, 2),
(1, 'JDM-HIVE', NULL, 'Hive', 'Estrategia sin tablero', 0.40, 25.00, 300.00, 5, 2),
(1, 'JDM-BLOKUS', NULL, 'Blokus', 'Estrategia geométrica', 0.70, 25.00, 300.00, 6, 2),
(1, 'JDM-QWIXX', NULL, 'Qwixx', 'Juego de dados rápido', 0.20, 15.00, 180.00, 8, 3),
(1, 'JDM-SUSHGO', NULL, 'Sushi Go', 'Draft de cartas', 0.20, 12.00, 140.00, 10, 3),
(1, 'JDM-LOVELET', NULL, 'Love Letter', 'Juego de cartas minimalista', 0.15, 10.00, 120.00, 12, 4),
(1, 'JDM-HANABI', NULL, 'Hanabi', 'Cooperativo de fuegos artificiales', 0.30, 15.00, 180.00, 7, 2),
(1, 'JDM-SPLENDOR', NULL, 'Splendor Duel', 'Versión para 2 jugadores', 0.60, 30.00, 350.00, 5, 2),
(1, 'JDM-WINGSPAN', NULL, 'Wingspan', 'Estrategia de aves', 1.50, 60.00, 700.00, 3, 1),
(1, 'JDM-ROOT', NULL, 'Root', 'Guerra asimétrica', 1.80, 60.00, 700.00, 2, 1),
(1, 'JDM-SPIRIT', NULL, 'Spirit Island', 'Cooperativo defender la isla', 2.00, 70.00, 820.00, 2, 1),
(1, 'JDM-SCYT', NULL, 'Scythe', 'Estrategia en mundo alternativo', 2.50, 80.00, 940.00, 2, 1),
(1, 'JDM-BLOOD', NULL, 'Blood Rage', 'Estrategia vikinga', 2.00, 65.00, 760.00, 2, 1),
(1, 'JDM-NEURO', NULL, 'Neuroshima Hex', 'Estrategia táctica', 0.80, 35.00, 410.00, 3, 1),
(2, 'DND-PH-5E', NULL, 'Player''s Handbook D&D 5e', 'Manual básico de D&D 5ta edición', 1.20, 35.00, 350.00, 5, 2),
(2, 'DND-DMG-5E', NULL, 'Dungeon Master Guide 5e', 'Guía del Dungeon Master', 1.30, 40.00, 400.00, 4, 2),
(2, 'DND-MM-5E', NULL, 'Monster Manual 5e', 'Manual de monstruos', 1.40, 40.00, 400.00, 4, 2),
(2, 'DND-XGE', NULL, 'Xanathar''s Guide to Everything', 'Expansión de reglas', 1.20, 35.00, 350.00, 3, 1),
(2, 'DND-TCE', NULL, 'Tasha''s Cauldron of Everything', 'Expansión de clases', 1.20, 35.00, 350.00, 3, 1),
(2, 'DND-SCAG', NULL, 'Sword Coast Adventurer''s Guide', 'Guía de la Costa de la Espada', 1.10, 30.00, 300.00, 3, 1),
(2, 'DND-EGW', NULL, 'Explorer''s Guide to Wildemount', 'Guía de Wildemount', 1.30, 40.00, 400.00, 2, 1),
(2, 'DND-MOT', NULL, 'Mythic Odysseys of Theros', 'Aventura en Theros', 1.20, 35.00, 350.00, 2, 1),
(2, 'DND-VRGR', NULL, 'Van Richten''s Guide to Ravenloft', 'Guía de Ravenloft', 1.30, 40.00, 400.00, 2, 1),
(2, 'DND-FTD', NULL, 'Fizban''s Treasury of Dragons', 'Tesoro de dragones', 1.20, 35.00, 350.00, 3, 1),
(2, 'PATH-CR', NULL, 'Pathfinder Core Rulebook', 'Manual básico de Pathfinder', 1.50, 50.00, 500.00, 2, 1),
(2, 'PATH-BST', NULL, 'Pathfinder Bestiary', 'Bestiario de Pathfinder', 1.60, 45.00, 450.00, 2, 1),
(2, 'COF-CORE', NULL, 'Call of Cthulhu 7th Ed', 'Manual básico de La Llamada', 1.20, 40.00, 400.00, 2, 1),
(2, 'VAMP-V5', NULL, 'Vampire The Masquerade 5th', 'Manual básico de Vampiro', 1.30, 45.00, 450.00, 2, 1),
(2, 'WOD-WR', NULL, 'Werewolf The Apocalypse 5th', 'Manual de Hombre Lobo', 1.20, 40.00, 400.00, 2, 1),
(2, 'DND-BGG', NULL, 'Baldur''s Gate: Descent into Avernus', 'Aventura de D&D', 1.40, 40.00, 400.00, 2, 1),
(2, 'DND-CURSE', NULL, 'Curse of Strahd', 'Aventura gótica', 1.30, 35.00, 350.00, 3, 1),
(2, 'DND-TOMB', NULL, 'Tomb of Annihilation', 'Aventura en Chult', 1.30, 35.00, 350.00, 2, 1),
(2, 'DND-WATER', NULL, 'Waterdeep: Dragon Heist', 'Aventura urbana', 1.20, 35.00, 350.00, 3, 1),
(2, 'DND-ICE', NULL, 'Icewind Dale: Rime of the Frostmaiden', 'Aventura en el norte', 1.40, 40.00, 400.00, 2, 1),
(3, 'FIG-TYRWHR', NULL, 'Figura Tiránido Warhammer', 'Miniatura de Warhammer 40k', 0.15, 20.00, 200.00, 2, 1),
(3, 'FIG-SPMAR', NULL, 'Space Marine Primaris', 'Miniatura Warhammer 40k', 0.12, 25.00, 250.00, 3, 1),
(3, 'FIG-ORKBOY', NULL, 'Ork Boyz Warhammer', 'Escuadrón de Orkos', 0.20, 30.00, 300.00, 2, 1),
(3, 'FIG-ELDAR', NULL, 'Eldar Guardian', 'Miniatura Eldar', 0.10, 18.00, 180.00, 4, 2),
(3, 'FIG-NECRON', NULL, 'Necron Warrior', 'Miniatura Necron', 0.12, 22.00, 220.00, 3, 1),
(3, 'FIG-CHAOS', NULL, 'Chaos Space Marine', 'Marine del Caos', 0.13, 24.00, 240.00, 2, 1),
(3, 'FIG-DRAGON', NULL, 'Dragón Rojo D&D', 'Miniatura de dragón', 0.25, 35.00, 350.00, 2, 1),
(3, 'FIG-BEHOLD', NULL, 'Beholder D&D', 'Miniatura de beholder', 0.18, 30.00, 300.00, 2, 1),
(3, 'FIG-LICH', NULL, 'Lich D&D', 'Miniatura de lich', 0.15, 28.00, 280.00, 2, 1),
(3, 'FIG-TREANT', NULL, 'Treant D&D', 'Miniatura de treant', 0.22, 32.00, 320.00, 2, 1),
(3, 'FIG-GIANT', NULL, 'Hill Giant D&D', 'Miniatura de gigante', 0.30, 40.00, 400.00, 1, 1),
(3, 'FIG-DEMON', NULL, 'Demon Lord D&D', 'Miniatura de demonio', 0.20, 35.00, 350.00, 2, 1),
(3, 'FIG-PALADN', NULL, 'Paladin D&D', 'Miniatura de paladín', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-WIZARD', NULL, 'Wizard D&D', 'Miniatura de mago', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-ROGUE', NULL, 'Rogue D&D', 'Miniatura de pícaro', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-CLERIC', NULL, 'Cleric D&D', 'Miniatura de clérigo', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-RANGER', NULL, 'Ranger D&D', 'Miniatura de explorador', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-BARBAR', NULL, 'Barbarian D&D', 'Miniatura de bárbaro', 0.12, 16.00, 160.00, 4, 2),
(3, 'FIG-SORCER', NULL, 'Sorcerer D&D', 'Miniatura de hechicero', 0.10, 15.00, 150.00, 4, 2),
(3, 'FIG-WARLCK', NULL, 'Warlock D&D', 'Miniatura de brujo', 0.10, 15.00, 150.00, 4, 2),
(4, 'JDC-MAGIC', NULL, 'Magic The Gathering Booster', 'Sobre de cartas Magic', 0.05, 4.00, 50.00, 20, 5),
(4, 'JDC-MAGBOX', NULL, 'Magic Commander Deck', 'Mazo Commander pre-construido', 0.40, 40.00, 400.00, 5, 2),
(4, 'JDC-MAGBND', NULL, 'Magic Bundle', 'Bundle de Magic', 0.60, 50.00, 500.00, 4, 2),
(4, 'JDC-YUOHGX', NULL, 'Yu-Gi-Oh! Booster', 'Sobre de cartas Yu-Gi-Oh!', 0.03, 3.50, 45.00, 25, 8),
(4, 'JDC-YUOSTR', NULL, 'Yu-Gi-Oh! Structure Deck', 'Mazo estructural Yu-Gi-Oh!', 0.25, 12.00, 140.00, 8, 3),
(4, 'JDC-POKBOOST', NULL, 'Pokemon Booster Pack', 'Sobre de cartas Pokemon', 0.03, 4.50, 55.00, 30, 10),
(4, 'JDC-POKELITE', NULL, 'Pokemon Elite Trainer Box', 'Caja de entrenador Pokemon', 0.80, 50.00, 500.00, 6, 2),
(4, 'JDC-POKCOLL', NULL, 'Pokemon Collection Box', 'Caja colección Pokemon', 1.00, 60.00, 600.00, 4, 2),
(4, 'JDC-KEYFORGE', NULL, 'KeyForge Unique Deck', 'Mazo único KeyForge', 0.20, 15.00, 180.00, 10, 3),
(4, 'JDC-ARKHAM', NULL, 'Arkham Horror LCG Expansion', 'Expansión de Arkham Horror', 0.30, 30.00, 300.00, 4, 2),
(4, 'JDC-LOTRLCG', NULL, 'Lord of the Rings LCG', 'Expansión de LOTR LCG', 0.25, 25.00, 250.00, 5, 2),
(4, 'JDC-MARVEL', NULL, 'Marvel Champions LCG', 'Expansión Marvel Champions', 0.30, 30.00, 300.00, 4, 2),
(4, 'JDC-FABRIC', NULL, 'Flesh and Blood Booster', 'Sobre de cartas FAB', 0.04, 5.00, 60.00, 15, 5),
(4, 'JDC-ONEPIECE', NULL, 'One Piece Card Game Booster', 'Sobre de cartas One Piece', 0.03, 4.00, 50.00, 20, 6),
(4, 'JDC-DRAGONB', NULL, 'Dragon Ball Super Card Game', 'Sobre de cartas Dragon Ball', 0.03, 4.00, 50.00, 18, 6),
(5, 'ACC-DADOS', NULL, 'Set de Dados D&D (7 piezas)', 'Dados poliédricos para rol', 0.10, 8.00, 80.00, 10, 3),
(5, 'ACC-TAPETE', NULL, 'Tapete de Juego D&D', 'Tapete de batalla con cuadrícula', 0.50, 20.00, 200.00, 8, 3),
(5, 'ACC-FIGURI', NULL, 'Figuras de Cartón D&D', 'Tokens de cartón para D&D', 0.15, 12.00, 120.00, 12, 4),
(5, 'ACC-PORTAD', NULL, 'Porta Dados de Cuero', 'Bolsa para dados de cuero', 0.08, 10.00, 100.00, 15, 5),
(5, 'ACC-SCREEN', NULL, 'Pantalla de Dungeon Master', 'Pantalla del DM con ilustraciones', 0.40, 25.00, 250.00, 6, 2);
GO

-- 9. VERIFICACIÓN FINAL
PRINT 'Base de datos Guarida Goblin creada y poblada exitosamente.';
SELECT 'Categorias' AS Tabla, COUNT(*) AS Registros FROM Categoria UNION ALL
SELECT 'Clientes', COUNT(*) FROM Cliente UNION ALL
SELECT 'Empleados', COUNT(*) FROM Empleado UNION ALL
SELECT 'Usuarios', COUNT(*) FROM Usuario UNION ALL
SELECT 'Proveedores', COUNT(*) FROM Proveedor UNION ALL
SELECT 'Descuentos', COUNT(*) FROM Descuento UNION ALL
SELECT 'Productos', COUNT(*) FROM Producto;
GO
