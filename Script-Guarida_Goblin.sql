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

-- LIMPIEZA DE TABLAS

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
GO

-- 1. TABLAS MAESTRAS (CATÁLOGOS)

-- CATEGORÍAS DE PRODUCTOS
CREATE TABLE Categoria (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50) NOT NULL,
    descripcion VARCHAR(250) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1  -- 1:Activo, 0:Inactivo, -1:Eliminado
);

-- CLIENTES (Compradores locales)
CREATE TABLE Cliente (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    cedulaIdentidad VARCHAR(15) NULL,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NULL,
    celular VARCHAR(15) NULL,
    direccion VARCHAR(200) NULL,
    email VARCHAR(100) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

-- EMPLEADOS
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

-- USUARIOS DEL SISTEMA
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

-- PROVEEDORES (Nacionales e Internacionales)
CREATE TABLE Proveedor (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    tipo VARCHAR(20) NOT NULL CHECK (tipo IN ('Nacional', 'Internacional')),
    razonSocial VARCHAR(150) NOT NULL,
    nit VARCHAR(20) NULL,
    contacto VARCHAR(100) NULL,
    email VARCHAR(100) NULL,
    telefono VARCHAR(20) NULL,
    pais VARCHAR(50) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

-- 2. TABLA DE PRODUCTOS

CREATE TABLE Producto (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idCategoria INT NOT NULL,
    codigo VARCHAR(30) NOT NULL,
    nombre VARCHAR(150) NOT NULL,
    descripcion VARCHAR(500) NULL,
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

-- 3. TABLAS DE TRANSACCIONES (COMPRAS Y VENTAS)

-- COMPRAS (Pedidos a proveedores)
CREATE TABLE Compra (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idProveedor INT NOT NULL,
    fecha DATE NOT NULL DEFAULT GETDATE(),
    subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    total DECIMAL(10,2) NOT NULL,
    -- Campos de seguimiento (solo se llenan si el proveedor es Internacional)
    trackingNumber VARCHAR(100) NULL,
    estadoEnvio VARCHAR(50) NULL,  -- 'Pendiente', 'En camino', 'En aduana', 'Recibido'
    fechaEstimada DATE NULL,
    observaciones VARCHAR(500) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_Compra_Proveedor FOREIGN KEY (idProveedor) REFERENCES Proveedor(id)
);

-- DETALLE DE COMPRAS
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

-- VENTAS (Atención en tienda)
CREATE TABLE Venta (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idCliente INT NOT NULL,
    idUsuario INT NOT NULL,
    transaccion INT NULL,  -- Número de factura (opcional al registrar)
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    total DECIMAL(10,2) NOT NULL,
    observaciones VARCHAR(500) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_Venta_Cliente FOREIGN KEY (idCliente) REFERENCES Cliente(id),
    CONSTRAINT fk_Venta_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(id)
);

-- DETALLE DE VENTAS
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

-- 4. ÍNDICES DE RENDIMIENTO

CREATE INDEX idx_Producto_nombre ON Producto(nombre);
CREATE INDEX idx_Producto_codigo ON Producto(codigo);
CREATE INDEX idx_Cliente_nombres ON Cliente(nombres);
CREATE INDEX idx_Proveedor_razonSocial ON Proveedor(razonSocial);
CREATE INDEX idx_Venta_fecha ON Venta(fecha);
CREATE INDEX idx_Compra_fecha ON Compra(fecha);
GO

-- 5. PROCEDIMIENTOS ALMACENADOS

-- Listar productos con búsqueda
DROP PROC IF EXISTS paProductoListar;
GO
CREATE PROC paProductoListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT p.id, p.idCategoria, c.nombre AS categoria,
           p.codigo, p.nombre, p.descripcion, p.pesoKg,
           p.precioCostoUsd, p.precioVentaBob,
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
    SELECT id, cedulaIdentidad, nombres, apellidos, celular, direccion, email,
           usuarioRegistro, fechaRegistro, estado
    FROM Cliente
    WHERE estado = 1
      AND (nombres + ' ' + ISNULL(apellidos,'') + ' ' + ISNULL(cedulaIdentidad,'') + ' ' + ISNULL(celular,'')) LIKE '%' + @parametro + '%'
    ORDER BY nombres;
END
GO

-- Listar proveedores
DROP PROC IF EXISTS paProveedorListar;
GO
CREATE PROC paProveedorListar @tipo VARCHAR(20), @parametro VARCHAR(100)
AS
BEGIN
    SELECT id, tipo, razonSocial, nit, contacto, email, telefono, pais,
           usuarioRegistro, fechaRegistro, estado
    FROM Proveedor
    WHERE estado = 1
      AND (@tipo = 'AMBOS' OR tipo = @tipo)
      AND (razonSocial + ' ' + ISNULL(nit,'') + ' ' + ISNULL(contacto,'')) LIKE '%' + @parametro + '%'
    ORDER BY razonSocial;
END
GO

-- Listar empleados
DROP PROC IF EXISTS paEmpleadoListar;
GO
CREATE PROC paEmpleadoListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT id, cedulaIdentidad, nombres, primerApellido, segundoApellido,
           fechaNacimiento, direccion, celular, cargo,
           usuarioRegistro, fechaRegistro, estado
    FROM Empleado
    WHERE estado = 1
      AND (cedulaIdentidad + ' ' + nombres + ' ' + ISNULL(primerApellido,'') + ' ' + ISNULL(segundoApellido,'')) LIKE '%' + @parametro + '%'
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

-- Registrar Venta 
DROP PROC IF EXISTS paVentaRegistrar;
GO
CREATE PROC paVentaRegistrar @idCliente INT, @idUsuario INT, @detalles NVARCHAR(MAX), @total DECIMAL(10,2)
AS
BEGIN
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @idVenta INT;
        
        INSERT INTO Venta (idCliente, idUsuario, total, usuarioRegistro, fechaRegistro, estado)
        VALUES (@idCliente, @idUsuario, @total, SUSER_NAME(), GETDATE(), 1);
        SET @idVenta = SCOPE_IDENTITY();
        
        INSERT INTO VentaDetalle (idVenta, idProducto, cantidad, precioUnitario, subtotal, usuarioRegistro, fechaRegistro, estado)
        SELECT @idVenta, j.idProducto, j.cantidad, j.precioUnitario, j.subtotal, SUSER_NAME(), GETDATE(), 1
        FROM OPENJSON(@detalles) WITH (idProducto INT '$.idProducto', cantidad INT '$.cantidad', precioUnitario DECIMAL(10,2) '$.precioUnitario', subtotal DECIMAL(10,2) '$.subtotal') j;
        
        UPDATE p SET p.stockActual = p.stockActual - j.cantidad
        FROM Producto p INNER JOIN OPENJSON(@detalles) WITH (idProducto INT '$.idProducto', cantidad INT '$.cantidad') j ON p.id = j.idProducto;
        
        COMMIT TRAN;
        SELECT @idVenta AS idVenta;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN; THROW;
    END CATCH
END
GO

-- Registrar Compra
DROP PROC IF EXISTS paCompraRegistrar;
GO
CREATE PROC paCompraRegistrar @idProveedor INT, @trackingNumber VARCHAR(100), @estadoEnvio VARCHAR(50), @fechaEstimada DATE, @detalles NVARCHAR(MAX), @total DECIMAL(10,2)
AS
BEGIN
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @idCompra INT;
        
        INSERT INTO Compra (idProveedor, total, trackingNumber, estadoEnvio, fechaEstimada, usuarioRegistro, fechaRegistro, estado)
        VALUES (@idProveedor, @total, @trackingNumber, @estadoEnvio, @fechaEstimada, SUSER_NAME(), GETDATE(), 1);
        SET @idCompra = SCOPE_IDENTITY();
        
        INSERT INTO CompraDetalle (idCompra, idProducto, cantidad, precioUnitario, subtotal, usuarioRegistro, fechaRegistro, estado)
        SELECT @idCompra, j.idProducto, j.cantidad, j.precioUnitario, j.subtotal, SUSER_NAME(), GETDATE(), 1
        FROM OPENJSON(@detalles) WITH (idProducto INT '$.idProducto', cantidad INT '$.cantidad', precioUnitario DECIMAL(10,2) '$.precioUnitario', subtotal DECIMAL(10,2) '$.subtotal') j;
        
        UPDATE p SET p.stockActual = p.stockActual + j.cantidad
        FROM Producto p INNER JOIN OPENJSON(@detalles) WITH (idProducto INT '$.idProducto', cantidad INT '$.cantidad') j ON p.id = j.idProducto;
        
        COMMIT TRAN;
        SELECT @idCompra AS idCompra;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN; THROW;
    END CATCH
END
GO
--Para listar Venta--
DROP PROC IF EXISTS paVentaListar;
GO
CREATE PROC paVentaListar
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL
AS
BEGIN
    SELECT 
        v.id,c.nombres + ' ' + c.apellidos AS Cliente,u.usuario,v.transaccion,v.fecha,v.subtotal,
        v.total,v.observaciones,v.usuarioRegistro,v.fechaRegistro,v.estado
    FROM Venta v
    INNER JOIN Cliente c ON v.idCliente = c.id
    INNER JOIN Usuario u ON v.idUsuario = u.id
    WHERE v.estado IN (1, -1)
      AND (@fechaInicio IS NULL OR v.fecha >= @fechaInicio)
      AND (@fechaFin IS NULL OR v.fecha <= @fechaFin)
    ORDER BY v.id;
END
GO
EXEC paVentaListar ;
EXEC paVentaListarr '2026-01-01', '2026-10-31';
-- Listar venta


-- 6. DML

-- Categorías
INSERT INTO Categoria (nombre, descripcion) VALUES
('Juegos de Mesa', 'Juegos de mesa y cartas'),
('Manuales de Rol', 'Manuales de D&D y libros de rol'),
('Figuras y Miniaturas', 'Figuras coleccionables y miniaturas'),
('Juegos de Cartas', 'Juegos de cartas coleccionables'),
('Accesorios Geek', 'Dados, tapetes y otros accesorios');

-- Proveedores
INSERT INTO Proveedor (tipo, razonSocial, pais, contacto) VALUES
('Internacional', 'Amazon.com', 'USA', 'Soporte Amazon'),
('Internacional', 'Wizards of the Coast', 'USA', 'Ventas WotC'),
('Internacional', 'Devir Argentina', 'Argentina', 'Arkana Comics'),
('Nacional', 'Distribuidora de Juegos La Paz', 'Bolivia', 'Juan Perez'),
('Nacional', 'El mercader errante Santa Cruz', 'Bolivia', 'Judith Villagomez');

-- Clientes
INSERT INTO Cliente (nombres, apellidos, celular) VALUES
('Cliente', 'General', NULL),
('Carlos', 'Mamani', '77712345');

-- Empleados
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, fechaNacimiento, direccion, celular, cargo) VALUES
('1234567', 'Juan', 'Fernández', '2006-01-31', 'Calle Siempre viva #123', 72856559, 'Gerente');

-- Usuarios
INSERT INTO Usuario (idEmpleado, usuario, clave) VALUES
(1, 'admin', 's1udM8c0+VgaifwGKuOcBUMvh0U8nyJQt6XKeBTNb2s='); --"juego123"

-- Productos de ejemplo
INSERT INTO Producto (idCategoria, codigo, nombre, descripcion, pesoKg, precioCostoUsd, precioVentaBob, stockActual, stockMinimo) VALUES
(1, 'JDM-CATAN', 'Juego de Mesa Catan', 'Juego de estrategia y comercio', 1.50, 40.00, 400.00, 3, 1),
(2, 'DND-PH-5E', 'Player''s Handbook D&D 5e', 'Manual básico de D&D 5ta edición', 1.20, 35.00, 350.00, 5, 2),
(3, 'FIG-TYRWHR', 'Figura Tiránido Warhammer', 'Miniatura de Warhammer 40k', 0.15, 20.00, 200.00, 2, 1),
(4, 'JDC-MAGIC', 'Magic The Gathering Booster', 'Sobre de cartas Magic', 0.05, 4.00, 50.00, 20, 5),
(5, 'ACC-DADOS', 'Set de Dados D&D (7 piezas)', 'Dados poliédricos para rol', 0.10, 8.00, 80.00, 10, 3),
(1, 'JDM-DTL01', 'Juego de Mesa Distilled', 'Juego de estrategia', 1.80, 130.00, 1500.00, 4, 1),
(1, 'JDM-AGRC4', 'Juego de Mesa Agricola', 'Juego de comercio', 2.30, 240.00, 2800.00, 2, 1),
(1, 'JDM-PAMP3', 'Juego de Mesa Pampero', 'Juego de rol y avance', 1.80, 215.00, 2500.00, 3, 1);

GO

SELECT * FROM Venta;

