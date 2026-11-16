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
--agregamos una columna a la tabla venta para el tipo de pago
ALTER TABLE Venta ADD metodoPago VARCHAR(50) NOT NULL DEFAULT 'Efectivo';
ALTER TABLE Venta ADD descuento DECIMAL(10,2) NOT NULL DEFAULT 0;
ALTER TABLE Venta
DROP COLUMN iva;
ALTER TABLE Venta DROP CONSTRAINT DF__Venta__iva__625A9A57;
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


--Para listar Venta--
DROP PROC IF EXISTS paVentaListar;
GO
ALTER PROC paVentaListar
    @fechaInicio DATETIME = NULL,
    @fechaFin DATETIME = NULL
AS
BEGIN
    SELECT 
        v.id,
        v.fecha,
        v.idCliente,
        v.transaccion,
        v.metodoPago,
        v.total,
        v.estado,
        u.usuario,
        v.subtotal,
        v.descuento,
        c.nombres,
        v.observaciones,
        v.usuarioRegistro,
        v.fechaRegistro
    FROM Venta v
    INNER JOIN Cliente c ON v.idCliente = c.id
    INNER JOIN Usuario u ON v.idUsuario = u.id
    WHERE v.estado IN (1, 0)   -- 1 = Activo, 0 = Anulado
      AND (@fechaInicio IS NULL OR v.fecha >= CAST(@fechaInicio AS DATE))
      AND (@fechaFin IS NULL OR v.fecha < DATEADD(DAY, 1, CAST(@fechaFin AS DATE)))
    ORDER BY v.fecha DESC;
END
GO
EXEC paVentaListar '' ;
EXEC paVentaListar '2026-01-01', '2026-10-31';
-- Listar venta


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
INSERT INTO Cliente (nombres) VALUES
('elmer', 'champi', 67534493);

-- Empleados
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, fechaNacimiento, direccion, celular, cargo) VALUES
('1234567', 'Juan', 'Fernández', '2006-01-31', 'Calle Siempre viva #123', 72856559, 'Gerente');
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, fechaNacimiento, direccion, celular, cargo) VALUES
('10346363', 'Iver', 'Sanchez Siagua', '2003-08-24', 'Calle Siempre viva #123', 63759369, 'Cajero');
-- Usuarios
INSERT INTO Usuario (idEmpleado, usuario, clave) VALUES
(1, 'admin', 's1udM8c0+VgaifwGKuOcBUMvh0U8nyJQt6XKeBTNb2s='); --"juego123"
INSERT INTO Usuario (idEmpleado, usuario, clave) VALUES
(2, 'Iver', 'jgtvVo0MEvhvh1e+ah7FzmNEpkN2FbcKsdWw725Hxx8='); --"iver2003"
UPDATE Usuario 
SET usuario = 'Iver', 
    clave = 'jgtvVo0MEvhvh1e+ah7FzmNEpkN2FbcKsdWw725Hxx8='
WHERE idEmpleado = 2;

-- PRODUCTOS DE EJEMPLO (100 productos)

-- CATEGORÍA 1: JUEGOS DE MESA (50 productos)
INSERT INTO Producto (idCategoria, codigo, nombre, descripcion, pesoKg, precioCostoUsd, precioVentaBob, stockActual, stockMinimo) VALUES
(1, 'JDM-CATAN', 'Juego de Mesa Catan', 'Juego de estrategia y comercio', 1.50, 40.00, 400.00, 3, 1),
(1, 'JDM-DTL01', 'Juego de Mesa Distilled', 'Juego de estrategia', 1.80, 130.00, 1500.00, 4, 1),
(1, 'JDM-AGRC4', 'Juego de Mesa Agricola', 'Juego de comercio', 2.30, 240.00, 2800.00, 2, 1),
(1, 'JDM-PAMP3', 'Juego de Mesa Pampero', 'Juego de rol y avance', 1.80, 215.00, 2500.00, 3, 1),
(1, 'JDM-TWIL', 'Twilight Imperium 4ta Ed', 'Estrategia espacial épica', 3.50, 150.00, 1750.00, 2, 1),
(1, 'JDM-GLOOM', 'Gloomhaven', 'RPG táctico cooperativo', 4.20, 140.00, 1650.00, 1, 1),
(1, 'JDM-PANDEM', 'Pandemic', 'Cooperativo salvar el mundo', 1.20, 40.00, 480.00, 5, 2),
(1, 'JDM-RISK', 'Risk Classic', 'Conquista mundial', 1.80, 45.00, 520.00, 4, 2),
(1, 'JDM-SCRAB', 'Scrabble', 'Juego de palabras', 1.00, 25.00, 300.00, 8, 3),
(1, 'JDM-MONOP', 'Monopoly Classic', 'Juego inmobiliario', 1.10, 30.00, 350.00, 6, 2),
(1, 'JDM-CARCAS', 'Carcassonne', 'Construcción de ciudades', 1.00, 35.00, 400.00, 7, 2),
(1, 'JDM-7WONDR', '7 Wonders', 'Construcción de civilizaciones', 1.20, 50.00, 580.00, 4, 2),
(1, 'JDM-TICKET', 'Ticket to Ride', 'Rutas ferroviarias', 1.30, 55.00, 640.00, 5, 2),
(1, 'JDM-AZUL', 'Azul', 'Estrategia de azulejos', 1.10, 40.00, 470.00, 6, 2),
(1, 'JDM-SPLNDR', 'Splendor', 'Comercio de gemas', 1.00, 40.00, 470.00, 5, 2),
(1, 'JDM-EVOLVE', 'Evolution', 'Evolución de especies', 0.90, 35.00, 410.00, 4, 2),
(1, 'JDM-DIXIT', 'Dixit', 'Juego narrativo ilustrado', 0.80, 35.00, 410.00, 6, 2),
(1, 'JDM-CODENM', 'Codenames', 'Juego de palabras en equipo', 0.60, 20.00, 240.00, 10, 3),
(1, 'JDM-EXPLOD', 'Exploding Kittens', 'Juego de cartas explosivo', 0.30, 20.00, 240.00, 12, 4),
(1, 'JDM-UNO', 'UNO Classic', 'Juego de cartas familiar', 0.20, 10.00, 120.00, 15, 5),
(1, 'JDM-JENGA', 'Jenga Classic', 'Torre de bloques', 1.00, 15.00, 180.00, 10, 3),
(1, 'JDM-CLUE', 'Clue', 'Misterio y deducción', 0.90, 25.00, 300.00, 7, 2),
(1, 'JDM-BATTLE', 'Battleship', 'Hundir la flota', 0.80, 20.00, 240.00, 8, 3),
(1, 'JDM-OPERAT', 'Operation', 'Juego de destreza', 0.70, 20.00, 240.00, 6, 2),
(1, 'JDM-TRIVIA', 'Trivial Pursuit', 'Juego de preguntas', 1.20, 35.00, 410.00, 5, 2),
(1, 'JDM-PUZZLE', 'Puzzle 1000 piezas', 'Rompecabezas paisaje', 0.80, 15.00, 180.00, 12, 4),
(1, 'JDM-CHESS', 'Ajedrez de madera', 'Juego clásico de estrategia', 1.50, 30.00, 350.00, 6, 2),
(1, 'JDM-BACKGM', 'Backgammon', 'Juego de tablero clásico', 1.20, 25.00, 300.00, 5, 2),
(1, 'JDM-GO', 'Juego de Go', 'Estrategia milenaria', 0.80, 40.00, 470.00, 4, 2),
(1, 'JDM-MAHJNG', 'Mahjong', 'Juego chino tradicional', 1.50, 35.00, 410.00, 3, 1),
(1, 'JDM-DOMINO', 'Dominó clásico', 'Juego de fichas', 0.80, 12.00, 140.00, 10, 3),
(1, 'JDM-YAHTZE', 'Yahtzee', 'Juego de dados', 0.50, 15.00, 180.00, 8, 3),
(1, 'JDM-CRANIUM', 'Cranium', 'Juego de habilidades', 1.00, 30.00, 350.00, 5, 2),
(1, 'JDM-BOGGLE', 'Boggle', 'Juego de palabras rápido', 0.40, 15.00, 180.00, 7, 2),
(1, 'JDM-PARCH', 'Parchís', 'Juego de parcours', 0.60, 12.00, 140.00, 9, 3),
(1, 'JDM-OTELLO', 'Othello', 'Juego de discos', 0.50, 18.00, 210.00, 6, 2),
(1, 'JDM-ABALON', 'Abalone', 'Juego de canicas estratégicas', 1.20, 35.00, 410.00, 4, 2),
(1, 'JDM-HIVE', 'Hive', 'Estrategia sin tablero', 0.40, 25.00, 300.00, 5, 2),
(1, 'JDM-BLOKUS', 'Blokus', 'Estrategia geométrica', 0.70, 25.00, 300.00, 6, 2),
(1, 'JDM-QWIXX', 'Qwixx', 'Juego de dados rápido', 0.20, 15.00, 180.00, 8, 3),
(1, 'JDM-SUSHGO', 'Sushi Go', 'Draft de cartas', 0.20, 12.00, 140.00, 10, 3),
(1, 'JDM-LOVELET', 'Love Letter', 'Juego de cartas minimalista', 0.15, 10.00, 120.00, 12, 4),
(1, 'JDM-HANABI', 'Hanabi', 'Cooperativo de fuegos artificiales', 0.30, 15.00, 180.00, 7, 2),
(1, 'JDM-SPLENDOR', 'Splendor Duel', 'Versión para 2 jugadores', 0.60, 30.00, 350.00, 5, 2),
(1, 'JDM-WINGSPAN', 'Wingspan', 'Estrategia de aves', 1.50, 60.00, 700.00, 3, 1),
(1, 'JDM-ROOT', 'Root', 'Guerra asimétrica', 1.80, 60.00, 700.00, 2, 1),
(1, 'JDM-SPIRIT', 'Spirit Island', 'Cooperativo defender la isla', 2.00, 70.00, 820.00, 2, 1),
(1, 'JDM-SCYT', 'Scythe', 'Estrategia en mundo alternativo', 2.50, 80.00, 940.00, 2, 1),
(1, 'JDM-BLOOD', 'Blood Rage', 'Estrategia vikinga', 2.00, 65.00, 760.00, 2, 1),
(1, 'JDM-NEURO', 'Neuroshima Hex', 'Estrategia táctica', 0.80, 35.00, 410.00, 3, 1);

-- CATEGORÍA 2: MANUALES DE ROL (20 productos)
INSERT INTO Producto (idCategoria, codigo, nombre, descripcion, pesoKg, precioCostoUsd, precioVentaBob, stockActual, stockMinimo) VALUES
(2, 'DND-PH-5E', 'Player''s Handbook D&D 5e', 'Manual básico de D&D 5ta edición', 1.20, 35.00, 350.00, 5, 2),
(2, 'DND-DMG-5E', 'Dungeon Master Guide 5e', 'Guía del Dungeon Master', 1.30, 40.00, 400.00, 4, 2),
(2, 'DND-MM-5E', 'Monster Manual 5e', 'Manual de monstruos', 1.40, 40.00, 400.00, 4, 2),
(2, 'DND-XGE', 'Xanathar''s Guide to Everything', 'Expansión de reglas', 1.20, 35.00, 350.00, 3, 1),
(2, 'DND-TCE', 'Tasha''s Cauldron of Everything', 'Expansión de clases', 1.20, 35.00, 350.00, 3, 1),
(2, 'DND-SCAG', 'Sword Coast Adventurer''s Guide', 'Guía de la Costa de la Espada', 1.10, 30.00, 300.00, 3, 1),
(2, 'DND-EGW', 'Explorer''s Guide to Wildemount', 'Guía de Wildemount', 1.30, 40.00, 400.00, 2, 1),
(2, 'DND-MOT', 'Mythic Odysseys of Theros', 'Aventura en Theros', 1.20, 35.00, 350.00, 2, 1),
(2, 'DND-VRGR', 'Van Richten''s Guide to Ravenloft', 'Guía de Ravenloft', 1.30, 40.00, 400.00, 2, 1),
(2, 'DND-FTD', 'Fizban''s Treasury of Dragons', 'Tesoro de dragones', 1.20, 35.00, 350.00, 3, 1),
(2, 'PATH-CR', 'Pathfinder Core Rulebook', 'Manual básico de Pathfinder', 1.50, 50.00, 500.00, 2, 1),
(2, 'PATH-BST', 'Pathfinder Bestiary', 'Bestiario de Pathfinder', 1.60, 45.00, 450.00, 2, 1),
(2, 'COF-CORE', 'Call of Cthulhu 7th Ed', 'Manual básico de La Llamada', 1.20, 40.00, 400.00, 2, 1),
(2, 'VAMP-V5', 'Vampire The Masquerade 5th', 'Manual básico de Vampiro', 1.30, 45.00, 450.00, 2, 1),
(2, 'WOD-WR', 'Werewolf The Apocalypse 5th', 'Manual de Hombre Lobo', 1.20, 40.00, 400.00, 2, 1),
(2, 'DND-BGG', 'Baldur''s Gate: Descent into Avernus', 'Aventura de D&D', 1.40, 40.00, 400.00, 2, 1),
(2, 'DND-CURSE', 'Curse of Strahd', 'Aventura gótica', 1.30, 35.00, 350.00, 3, 1),
(2, 'DND-TOMB', 'Tomb of Annihilation', 'Aventura en Chult', 1.30, 35.00, 350.00, 2, 1),
(2, 'DND-WATER', 'Waterdeep: Dragon Heist', 'Aventura urbana', 1.20, 35.00, 350.00, 3, 1),
(2, 'DND-ICE', 'Icewind Dale: Rime of the Frostmaiden', 'Aventura en el norte', 1.40, 40.00, 400.00, 2, 1);

-- CATEGORÍA 3: FIGURAS Y MINIATURAS (20 productos)
INSERT INTO Producto (idCategoria, codigo, nombre, descripcion, pesoKg, precioCostoUsd, precioVentaBob, stockActual, stockMinimo) VALUES
(3, 'FIG-TYRWHR', 'Figura Tiránido Warhammer', 'Miniatura de Warhammer 40k', 0.15, 20.00, 200.00, 2, 1),
(3, 'FIG-SPMAR', 'Space Marine Primaris', 'Miniatura Warhammer 40k', 0.12, 25.00, 250.00, 3, 1),
(3, 'FIG-ORKBOY', 'Ork Boyz Warhammer', 'Escuadrón de Orkos', 0.20, 30.00, 300.00, 2, 1),
(3, 'FIG-ELDAR', 'Eldar Guardian', 'Miniatura Eldar', 0.10, 18.00, 180.00, 4, 2),
(3, 'FIG-NECRON', 'Necron Warrior', 'Miniatura Necron', 0.12, 22.00, 220.00, 3, 1),
(3, 'FIG-CHAOS', 'Chaos Space Marine', 'Marine del Caos', 0.13, 24.00, 240.00, 2, 1),
(3, 'FIG-DRAGON', 'Dragón Rojo D&D', 'Miniatura de dragón', 0.25, 35.00, 350.00, 2, 1),
(3, 'FIG-BEHOLD', 'Beholder D&D', 'Miniatura de beholder', 0.18, 30.00, 300.00, 2, 1),
(3, 'FIG-LICH', 'Lich D&D', 'Miniatura de lich', 0.15, 28.00, 280.00, 2, 1),
(3, 'FIG-TREANT', 'Treant D&D', 'Miniatura de treant', 0.22, 32.00, 320.00, 2, 1),
(3, 'FIG-GIANT', 'Hill Giant D&D', 'Miniatura de gigante', 0.30, 40.00, 400.00, 1, 1),
(3, 'FIG-DEMON', 'Demon Lord D&D', 'Miniatura de demonio', 0.20, 35.00, 350.00, 2, 1),
(3, 'FIG-PALADN', 'Paladin D&D', 'Miniatura de paladín', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-WIZARD', 'Wizard D&D', 'Miniatura de mago', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-ROGUE', 'Rogue D&D', 'Miniatura de pícaro', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-CLERIC', 'Cleric D&D', 'Miniatura de clérigo', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-RANGER', 'Ranger D&D', 'Miniatura de explorador', 0.10, 15.00, 150.00, 5, 2),
(3, 'FIG-BARBAR', 'Barbarian D&D', 'Miniatura de bárbaro', 0.12, 16.00, 160.00, 4, 2),
(3, 'FIG-SORCER', 'Sorcerer D&D', 'Miniatura de hechicero', 0.10, 15.00, 150.00, 4, 2),
(3, 'FIG-WARLCK', 'Warlock D&D', 'Miniatura de brujo', 0.10, 15.00, 150.00, 4, 2);

-- CATEGORÍA 4: JUEGOS DE CARTAS (15 productos)
INSERT INTO Producto (idCategoria, codigo, nombre, descripcion, pesoKg, precioCostoUsd, precioVentaBob, stockActual, stockMinimo) VALUES
(4, 'JDC-MAGIC', 'Magic The Gathering Booster', 'Sobre de cartas Magic', 0.05, 4.00, 50.00, 20, 5),
(4, 'JDC-MAGBOX', 'Magic Commander Deck', 'Mazo Commander pre-construido', 0.40, 40.00, 400.00, 5, 2),
(4, 'JDC-MAGBND', 'Magic Bundle', 'Bundle de Magic', 0.60, 50.00, 500.00, 4, 2),
(4, 'JDC-YUOHGX', 'Yu-Gi-Oh! Booster', 'Sobre de cartas Yu-Gi-Oh!', 0.03, 3.50, 45.00, 25, 8),
(4, 'JDC-YUOSTR', 'Yu-Gi-Oh! Structure Deck', 'Mazo estructural Yu-Gi-Oh!', 0.25, 12.00, 140.00, 8, 3),
(4, 'JDC-POKBOOST', 'Pokemon Booster Pack', 'Sobre de cartas Pokemon', 0.03, 4.50, 55.00, 30, 10),
(4, 'JDC-POKELITE', 'Pokemon Elite Trainer Box', 'Caja de entrenador Pokemon', 0.80, 50.00, 500.00, 6, 2),
(4, 'JDC-POKCOLL', 'Pokemon Collection Box', 'Caja colección Pokemon', 1.00, 60.00, 600.00, 4, 2),
(4, 'JDC-KEYFORGE', 'KeyForge Unique Deck', 'Mazo único KeyForge', 0.20, 15.00, 180.00, 10, 3),
(4, 'JDC-ARKHAM', 'Arkham Horror LCG Expansion', 'Expansión de Arkham Horror', 0.30, 30.00, 300.00, 4, 2),
(4, 'JDC-LOTRLCG', 'Lord of the Rings LCG', 'Expansión de LOTR LCG', 0.25, 25.00, 250.00, 5, 2),
(4, 'JDC-MARVEL', 'Marvel Champions LCG', 'Expansión Marvel Champions', 0.30, 30.00, 300.00, 4, 2),
(4, 'JDC-FABRIC', 'Flesh and Blood Booster', 'Sobre de cartas FAB', 0.04, 5.00, 60.00, 15, 5),
(4, 'JDC-ONEPIECE', 'One Piece Card Game Booster', 'Sobre de cartas One Piece', 0.03, 4.00, 50.00, 20, 6),
(4, 'JDC-DRAGONB', 'Dragon Ball Super Card Game', 'Sobre de cartas Dragon Ball', 0.03, 4.00, 50.00, 18, 6);

-- CATEGORÍA 5: ACCESORIOS GEEK (5 productos)
INSERT INTO Producto (idCategoria, codigo, nombre, descripcion, pesoKg, precioCostoUsd, precioVentaBob, stockActual, stockMinimo) VALUES
(5, 'ACC-DADOS', 'Set de Dados D&D (7 piezas)', 'Dados poliédricos para rol', 0.10, 8.00, 80.00, 10, 3),
(5, 'ACC-TAPETE', 'Tapete de Juego D&D', 'Tapete de batalla con cuadrícula', 0.50, 20.00, 200.00, 8, 3),
(5, 'ACC-FIGURI', 'Figuras de Cartón D&D', 'Tokens de cartón para D&D', 0.15, 12.00, 120.00, 12, 4),
(5, 'ACC-PORTAD', 'Porta Dados de Cuero', 'Bolsa para dados de cuero', 0.08, 10.00, 100.00, 15, 5),
(5, 'ACC-SCREEN', 'Pantalla de Dungeon Master', 'Pantalla del DM con ilustraciones', 0.40, 25.00, 250.00, 6, 2);
GO

GO

SELECT * FROM Producto;
SELECT * FROM Venta;
SELECT * FROM Cliente;
SELECT * FROM Usuario;
SELECT * FROM Empleado;

