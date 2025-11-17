# VL-DRINKS - E-Commerce (Proyecto Universitario)

`VL-DRINKS` es un proyecto universitario de e-commerce desarrollado en ASP.NET (.NET Framework). La plataforma está diseñada para la venta y distribución de bebidas alcohólicas, implementando un flujo de compra completo,la **verificación de mayoría de edad**, entre otras funcionalidades.

---

## 🎓 Contexto Académico

Este proyecto es un trabajo práctico integrador para la institución **Quality ISAD**, correspondiente al 2do año de la **Tecnicatura Superior en Desarrollo de Software**.

* **Materias Involucradas:** Programación II, Modelado y arquitectura de software.
* **Profesores:** Karina Salto, Fermin Stura.

---

## 🚀 Funcionalidades Principales (Épicas)

El proyecto está estructurado en cuatro épicas principales gestionadas en Jira, que definen el alcance funcional del sistema.

### 1. EP-001: Gestión de Usuarios y Autenticación
Sistema completo de registro, autenticación y gestión de usuarios.
* Registro de usuarios con **verificación de mayoría de edad** (requisito legal).
* Inicio de sesión seguro con credenciales encriptadas (`HU-002`).
* Cierre de sesión del usuario autenticado (`HU-003`).
* Recuperación de contraseña mediante correo electrónico (`HU-004`).

### 2. EP-002: Catálogo de Productos
Visualización, organización y gestión del catálogo de productos.
* Visualización del catálogo de productos (`HU-005`).
* Sistema de búsqueda y filtrado por categorías (`HU-006`).
* Visualización detallada de cada producto (`HU-007`).

### 3. EP-003: Carrito de Compras y Gestión de Pedidos
Núcleo transaccional de la plataforma, cubriendo el flujo completo de compra.
* Agregar productos al carrito de compras (`HU-009`).
* Modificar y eliminar productos del carrito (`HU-010`).
* Confirmación de pedido y **actualización de stock** (`HU-011`).
* Integración con la **API de MercadoPago** para procesar pagos (`HU-012`).

### 4. EP-004: Panel de Administración
Centro de control interno para la gestión de la tienda.
* Acceso restringido al panel basado en roles (`HU-013`).
* **Gestión de Productos (CRUD):** Alta, baja y modificación de productos desde el panel (`HU-014`).
* **Gestión de Pedidos:** Visualización y seguimiento de los pedidos de los clientes (`HU-015`).

---

## 🛠️ Arquitectura y Stack Tecnológico

El proyecto está construido en **ASP.NET (.NET Framework)** utilizando el patrón **MVC (Modelo-Vista-Controlador)** y una **arquitectura de N-Capas** para una clara separación de responsabilidades.

### Stack de Tecnologías
* **Lenguaje:** C#
* **Framework:** ASP.NET (.NET Framework)
* **Base de Datos:** SQL Server
* **Pasarela de Pagos:** API de MercadoPago
* **Gestión de Proyecto:** Jira

### Capas del Proyecto
* `CAPAPRESENTACION`: (Proyecto MVC) Contiene los controladores, vistas (HTML/CSS/JS) y la lógica de interacción con el usuario.
* `CapaNegocio`: Contiene las reglas de negocio, validaciones y lógica central.
* `CapaEntidad`: Define los modelos y objetos que se usan en la aplicación.
* `(CapaDatos)`: (Integrada) Maneja la conexión y las consultas a la base de datos SQL Server.

---

## 👥 Equipo de Desarrollo

* **Aguilera, Nahuel** ([@Nahuel Aguilera](https://github.com/Nahuel105))
* **Lelli, Fabrizio** ([@Fabrizio Lelli](https://github.com/FabrizioIvanLelli))
* **Monelli, Alexander** ([@Alexander Monelli](https://github.com/Atzur1))
* **Silva, Angel** ([@Angel Silva](https://github.com/AngelSilva98Dev))
