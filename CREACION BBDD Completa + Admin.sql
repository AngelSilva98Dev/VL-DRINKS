/*
 * ============================================
 * SCRIPT DE CREACIÓN TOTAL - VLDRINKS
 * ============================================
 * Lógica:
 * 1. Se conecta a la base de datos 'master' para poder eliminar 'VLDRINKS'.
 * 2. Elimina la base de datos 'VLDRINKS' si ya existe.
 * 3. Crea la nueva base de datos 'VLDRINKS'.
 * 4. Cambia a la base de datos 'VLDRINKS'.
 * 5. Crea todas las tablas (USUARIO, CLIENTE, CATEGORIA, MARCA, PRODUCTO, etc.).
 * 6. Inserta el SuperUsuario (pass: '123456').
 * 7. Inserta datos de prueba para la tienda (Categorías, Marcas, 13 Productos).
 */

-- 1. CONECTAR A 'MASTER' Y ELIMINAR LA BBDD ANTIGUA
USE master;
GO

IF DB_ID('VLDRINKS') IS NOT NULL
BEGIN
    ALTER DATABASE VLDRINKS SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VLDRINKS;
    PRINT 'Base de datos VLDRINKS anterior eliminada.';
END
GO

-- 2. CREAR LA NUEVA BASE DE DATOS
CREATE DATABASE VLDRINKS;
GO
PRINT 'Base de datos VLDRINKS creada.';
GO

-- 3. CAMBIAR A LA NUEVA BASE DE DATOS
USE VLDRINKS;
GO

/*
 * ============================================
 * 4. CREAR TABLAS (Esquema final)
 * ============================================
 */

-- Tabla de Administradores
CREATE TABLE dbo.USUARIO (
    IdUsuario     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombres       VARCHAR(100) NULL,
    Apellidos     VARCHAR(100) NULL,
    Correo        VARCHAR(100) NOT NULL,
    Reestablecer  BIT NOT NULL DEFAULT 0,
    Activo        BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE() NULL,
    esAdmin       BIT NOT NULL DEFAULT 0, 
    PasswordHash  VARBINARY(20) NOT NULL, -- SHA-1 (20 bytes)
    PasswordSalt  VARBINARY(32) NOT NULL,
    FechaUltimoReinicio DATETIME NULL, -- Para el límite de 2 min
    CONSTRAINT UQ_Usuario_Correo UNIQUE(Correo)
);
GO
PRINT 'Tabla USUARIO creada.'
GO

-- Tabla de Clientes (para la Tienda)
CREATE TABLE dbo.CLIENTE (
    IdCliente     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombres       VARCHAR(100) NULL,
    Apellidos     VARCHAR(100) NULL,
    Correo        VARCHAR(100) NOT NULL,
    Reestablecer  BIT NOT NULL DEFAULT 0,
    FechaRegistro DATETIME DEFAULT GETDATE() NULL,
    PasswordHash  VARBINARY(20) NOT NULL, -- SHA-1 (20 bytes)
    PasswordSalt  VARBINARY(32) NOT NULL,
    EsMayorDeEdad BIT NOT NULL DEFAULT 0, -- Para el registro
    Telefono      VARCHAR(50) NULL,      -- Para el checkout
    Direccion     VARCHAR(200) NULL,     -- Para el checkout
    FechaUltimoReinicio DATETIME NULL, -- Para el límite de 2 min
    CONSTRAINT UQ_Cliente_Correo UNIQUE(Correo)
);
GO
PRINT 'Tabla CLIENTE creada.'
GO

CREATE TABLE dbo.CATEGORIA (
    IdCategoria INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descripcion VARCHAR(100) NULL,
    Activo      BIT NOT NULL DEFAULT 1
);
GO
PRINT 'Tabla CATEGORIA creada.'
GO

CREATE TABLE dbo.MARCA (
    IdMarca     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descripcion VARCHAR(100) NULL,
    Activo      BIT NOT NULL DEFAULT 1
);
GO
PRINT 'Tabla MARCA creada.'
GO

CREATE TABLE dbo.PRODUCTO (
    IdProducto    INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre        VARCHAR(100) NULL,
    Descripcion   VARCHAR(500) NULL,
    IdMarca       INT NULL,
    IdCategoria   INT NULL,
    Precio        DECIMAL(10, 2) NOT NULL DEFAULT 0,
    Stock         INT NOT NULL DEFAULT 0,
    NombreImagen  VARCHAR(500) NULL,
    Imagen        VARCHAR(MAX) NULL, -- Para Base64
    Activo        BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE() NULL,
    CONSTRAINT FK_Producto_Marca FOREIGN KEY (IdMarca) REFERENCES dbo.MARCA(IdMarca) ON DELETE SET NULL,
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria) REFERENCES dbo.CATEGORIA(IdCategoria) ON DELETE SET NULL
);
GO
PRINT 'Tabla PRODUCTO creada.'
GO

CREATE TABLE dbo.PEDIDO (
    IdPedido      INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCliente     INT NULL,
    TotalProducto INT NOT NULL,
    MontoTotal    DECIMAL(18, 2) NOT NULL, -- (Cambiado a 18,2)
    Contacto      VARCHAR(100) NULL,
    Telefono      VARCHAR(50) NULL,
    Direccion     VARCHAR(500) NULL,
    MetodoPago    VARCHAR(50) NOT NULL,
    Estado        VARCHAR(50) NOT NULL DEFAULT 'Esperando Comprobante',
    FechaPedido   DATETIME DEFAULT GETDATE() NULL,
    Subtotal      DECIMAL(18, 2) NULL, -- (Columna añadida)
    CostoEnvio    DECIMAL(18, 2) NULL, -- (Columna añadida)
    CONSTRAINT FK_Pedido_Cliente FOREIGN KEY (IdCliente) REFERENCES dbo.CLIENTE(IdCliente) ON DELETE SET NULL
);
GO
PRINT 'Tabla PEDIDO creada.'
GO

CREATE TABLE dbo.DETALLE_PEDIDO (
    IdDetallePedido INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdPedido        INT NOT NULL,
    IdProducto      INT NOT NULL,
    NombreProducto  VARCHAR(100) NOT NULL,
    Cantidad        INT NOT NULL,
    PrecioUnitario  DECIMAL(10, 2) NOT NULL,
    Total           DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_Detalle_Pedido FOREIGN KEY (IdPedido) REFERENCES dbo.PEDIDO(IdPedido) ON DELETE CASCADE,
    CONSTRAINT FK_Detalle_Producto FOREIGN KEY (IdProducto) REFERENCES dbo.PRODUCTO(IdProducto) ON DELETE NO ACTION
);
GO
PRINT 'Tabla DETALLE_PEDIDO creada.'
GO

/*
 * ============================================
 * 5. INSERCIÓN DE DATOS (SuperUsuario y Tienda)
 * ============================================
 */

-- A. SuperUsuario (pass: 123456)
INSERT INTO USUARIO (
    Nombres, Apellidos, Correo, esAdmin, 
    PasswordHash, PasswordSalt
)
VALUES
(
    'Super', 'Admin', 'admin@vldrinks.com', 1, -- esAdmin = TRUE
    
    -- Hash (SHA-1, 20 bytes) de "123456" (el que me pediste)
    0xF713CAFF814CBFB19760832EB23A5DBF52A25EB9, 
    
    -- Salt (32 bytes) (el que me pediste)
    0xFAE50C4337DF4A82AF8CA6CD0DCF1FAFAD27FE0BE42A40A8AED64949F396BFF6
);
GO
PRINT 'SuperUsuario creado.'
GO

-- B. Categorías
INSERT INTO CATEGORIA (Descripcion, Activo) VALUES
('Cervezas', 1),
('Vinos', 1),
('Gaseosas', 1),
('Aperitivos', 1),
('Whisky', 1);
GO
PRINT 'Datos de CATEGORIA cargados.'
GO

-- C. Marcas
INSERT INTO MARCA (Descripcion, Activo) VALUES
('Quilmes', 1),
('Heineken', 1),
('Coca-Cola', 1),
('Fernet Branca', 1),
('Jack Daniels', 1),
('Rutini', 1),
('Gancia', 1); -- (Añadida Gancia)
GO
PRINT 'Datos de MARCA cargados.'
GO

-- D. Productos (13 productos para paginación, sin imágenes)
DECLARE @IdMarcaHeineken INT = (SELECT IdMarca FROM MARCA WHERE Descripcion = 'Heineken');
DECLARE @IdCatCerveza INT = (SELECT IdCategoria FROM CATEGORIA WHERE Descripcion = 'Cervezas');
DECLARE @IdMarcaCoca INT = (SELECT IdMarca FROM MARCA WHERE Descripcion = 'Coca-Cola');
DECLARE @IdCatGaseosa INT = (SELECT IdCategoria FROM CATEGORIA WHERE Descripcion = 'Gaseosas');
DECLARE @IdMarcaRutini INT = (SELECT IdMarca FROM MARCA WHERE Descripcion = 'Rutini');
DECLARE @IdCatVino INT = (SELECT IdCategoria FROM CATEGORIA WHERE Descripcion = 'Vinos');
DECLARE @IdMarcaQuilmes INT = (SELECT IdMarca FROM MARCA WHERE Descripcion = 'Quilmes');
DECLARE @IdMarcaFernet INT = (SELECT IdMarca FROM MARCA WHERE Descripcion = 'Fernet Branca');
DECLARE @IdCatAperitivo INT = (SELECT IdCategoria FROM CATEGORIA WHERE Descripcion = 'Aperitivos');
DECLARE @IdMarcaJack INT = (SELECT IdMarca FROM MARCA WHERE Descripcion = 'Jack Daniels');
DECLARE @IdCatWhisky INT = (SELECT IdCategoria FROM CATEGORIA WHERE Descripcion = 'Whisky');
DECLARE @IdMarcaGancia INT = (SELECT IdMarca FROM MARCA WHERE Descripcion = 'Gancia');

INSERT INTO PRODUCTO (Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo, NombreImagen, Imagen)
VALUES 
('Heineken 480ml', 'Cerveza rubia premium en lata', @IdMarcaHeineken, @IdCatCerveza, 1250.00, 150, 1, 'heineken.jpg', ''),
('Coca-Cola 2.25L', 'Gaseosa sabor cola descartable', @IdMarcaCoca, @IdCatGaseosa, 1800.00, 200, 1, 'coca.jpg', ''),
('Rutini Cabernet Malbec', 'Vino tinto clásico', @IdMarcaRutini, @IdCatVino, 9500.00, 50, 1, 'rutini.jpg', ''),
('Quilmes Clásica 1L', 'Cerveza rubia retornable', @IdMarcaQuilmes, @IdCatCerveza, 1100.00, 100, 1, 'quilmes1l.jpg', ''),
('Fernet Branca 750ml', 'Aperitivo clásico de hierbas', @IdMarcaFernet, @IdCatAperitivo, 7500.00, 80, 1, 'branca750.jpg', ''),
('Jack Daniels Old No. 7 750ml', 'Tennessee Whiskey Clásico', @IdMarcaJack, @IdCatWhisky, 28000.00, 30, 1, 'jack750.jpg', ''),
('Quilmes Stout 473ml', 'Cerveza negra en lata', @IdMarcaQuilmes, @IdCatCerveza, 1050.00, 70, 1, 'stout473.jpg', ''),
('Fernet Branca Menta 750ml', 'Aperitivo sabor menta', @IdMarcaFernet, @IdCatAperitivo, 7500.00, 40, 1, 'brancamenta.jpg', ''),
('Jack Daniels Honey 750ml', 'Whiskey con un toque de miel', @IdMarcaJack, @IdCatWhisky, 31000.00, 25, 1, 'jackhoney.jpg', ''),
('Quilmes Bock 1L', 'Cerveza roja retornable', @IdMarcaQuilmes, @IdCatCerveza, 1150.00, 50, 1, 'bock1l.jpg', ''),
('Gancia 950ml', 'Aperitivo americano', @IdMarcaGancia, @IdCatAperitivo, 3200.00, 60, 1, 'gancia950.jpg', ''),
('Coca-Cola Zero 2.25L', 'Gaseosa sabor cola sin azúcar', @IdMarcaCoca, @IdCatGaseosa, 1750.00, 120, 1, 'cocazero.jpg', ''),
('Quilmes 1890 1L', 'Cerveza lager estilo pilsener', @IdMarcaQuilmes, @IdCatCerveza, 1200.00, 75, 1, 'quilmes1890.jpg', '');
GO
PRINT 'Datos de PRODUCTO (13) cargados.'
GO

PRINT '¡Script finalizado! Base de datos VLDRINKS lista.'
GO