# 🚗 CarwashSystem

**CarwashSystem** es un sistema web diseñado para la gestión operativa de un car wash, enfocado en el control de servicios, ventas rápidas y administración de caja diaria.
El objetivo principal es registrar de forma sencilla los ingresos por servicios vehiculares y ventas de productos, manteniendo un flujo claro y controlado.

---

## 🎯 Objetivo del sistema

* Registrar servicios realizados a vehículos (carros y motos).
* Manejar precios variables por servicio.
* Controlar ventas de productos adicionales (snacks, gaseosas, etc.).
* Administrar la caja diaria (apertura, movimientos, arqueo y cierre).
* Obtener un resumen claro de ingresos por servicios y ventas.

---

## 🧩 Alcance del MVP

Este proyecto **NO** incluye:

* Múltiples usuarios
* Roles complejos
* Facturación electrónica
* Control de inventario avanzado
* Clientes registrados

El enfoque es **operativo y simple**.

---

## 🔐 Autenticación

* El sistema cuenta con **un solo usuario**.
* Es obligatorio iniciar sesión para acceder a cualquier funcionalidad.
* No existe registro público de usuarios.

---

## 💰 Flujo de Caja

### Apertura de Caja

* Al iniciar el día se debe abrir la caja.
* Mientras la caja esté cerrada:

  * No se pueden registrar servicios.
  * No se pueden realizar ventas.

### Estados de la Caja

* **Abierta**
* **Cerrada**

---

## 🚘 Gestión de Servicios

### Registro de Vehículo

Al llegar un vehículo se registra:

* Tipo (Carro / Moto)
* Marca
* Modelo
* Placa

### Servicios

* Cada servicio contiene:

  * Id
  * Nombre
  * Descripción
* **El precio no es fijo** y se asigna manualmente al momento de realizar el servicio.

Ejemplos:

* Cambio de aceite (precio depende del tipo de vehículo y aceite)
* Lavado
* Encerado

---

## ⏳ Estados del Servicio

Un servicio puede estar en:

* **Pendiente**

  * Servicios agregados sin precio completo
  * Permite edición

* **Completado**

  * Todos los servicios tienen precio
  * Se calcula el total
  * Se registra el ingreso en caja

📌 No se puede completar un servicio si algún precio está vacío.

---

## 💳 Pago de Servicios

Al completar un servicio:

* Se muestra el total a pagar
* Se registra:

  * Monto recibido
  * Vuelto
* Se genera un movimiento de caja tipo **Ingreso por Servicio**

---

## 🥤 Ventas de Productos

Las ventas de productos funcionan de manera independiente a los servicios.

Flujo:

1. Buscar producto
2. Seleccionar cantidad
3. Calcular total
4. Registrar pago
5. Registrar ingreso en caja

---

## 📊 Historial y Reportes

La caja permite visualizar:

* Historial de movimientos
* Separación de ingresos por:

  * Servicios
  * Ventas
* Total general del día

---

## 🧮 Arqueo y Cierre de Caja

Al finalizar la jornada:

* Se muestra el total registrado por el sistema
* Se ingresa el monto real contado
* Se calcula la diferencia (si existe)
* Se cierra la caja

📌 Una caja cerrada no permite nuevos movimientos.

---

## 🛠️ Tecnologías

* ASP.NET Core MVC / Razor Pages
* ASP.NET Core Identity (autenticación)
* Entity Framework Core
* SQL Server
* jQuery
* AJAX

---

## 📌 Estado del Proyecto

🚧 **En desarrollo (MVP)**
El sistema se construye de forma incremental, priorizando:

* Flujo funcional
* Simplicidad
* Rapidez de implementación

---

## 📄 Licencia

Proyecto de uso privado / comercial.
