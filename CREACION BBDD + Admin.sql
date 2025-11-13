/*
 * ============================================
 * SCRIPT DE CREACIÓN TOTAL - VLDRINKS
 * ============================================
 * Lógica:
 * 1. Crea la base de datos 'VLDRINKS'.
 * 2. Cambia a esa base de datos.
 * 3. Crea todas las tablas (USUARIO, CLIENTE, CATEGORIA, MARCA, PRODUCTO, etc.).
 * 4. Inserta el SuperUsuario (pass: '123456').
 */

-- 1. CREAR LA NUEVA BASE DE DATOS
CREATE DATABASE VLDRINKS;
GO

PRINT 'Base de datos VLDRINKS creada.';
GO

-- 2. CAMBIAR A LA NUEVA BASE DE DATOS
USE VLDRINKS;
GO

/*
 * ============================================
 * 3. CREAR TABLAS (Esquema final)
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
    MontoTotal    DECIMAL(10, 2) NOT NULL,
    Contacto      VARCHAR(100) NULL,
    Telefono      VARCHAR(50) NULL,
    Direccion     VARCHAR(500) NULL,
    MetodoPago    VARCHAR(50) NOT NULL,
    Estado        VARCHAR(50) NOT NULL DEFAULT 'Esperando Comprobante',
    FechaPedido   DATETIME DEFAULT GETDATE() NULL,
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
 * 4. INSERCIÓN DEL SUPERUSUARIO (pass: 123456)
 * ============================================
 */
INSERT INTO USUARIO (
    Nombres, Apellidos, Correo, esAdmin, 
    PasswordHash, PasswordSalt
)
VALUES
(
    'Super', 'Admin', 'admin@vldrinks.com', 1, -- esAdmin = TRUE
    
    -- Hash (SHA-1, 20 bytes) de "123456"
    0xF713CAFF814CBFB19760832EB23A5DBF52A25EB9, 
    
    -- Salt (32 bytes)
    0xFAE50C4337DF4A82AF8CA6CD0DCF1FAFAD27FE0BE42A40A8AED64949F396BFF6
);
GO

PRINT '¡Script finalizado! Base de datos VLDRINKS limpia y SuperUsuario creado.'
GO

USE VLDRINKS

ALTER TABLE USUARIO
ADD FechaUltimoReinicio DATETIME NULL;
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'FechaUltimoReinicio'
          AND Object_ID = Object_ID(N'dbo.USUARIO'))
BEGIN
    ALTER TABLE USUARIO
    ADD FechaUltimoReinicio DATETIME NULL;
    
    PRINT 'Columna "FechaUltimoReinicio" creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La columna "FechaUltimoReinicio" ya existía.';
END
GO