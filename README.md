# Product Catalog & Dynamic QR Code System

A robust, enterprise-ready backend API built with .NET 10, PostgreSQL, and JWT Authentication. This system enables users to perform full CRUD operations on product inventory and dynamically generate QR codes. Instead of embedding static product information directly into the QR code, this system embeds a secure tracking URL containing the unique Product Identifier. This allows inventory data (such as price, name, and dimensions) to be updated in real-time without re-printing physical materials.

---

## 1. System Architecture & Use Case Flowg

The system architecture decouples the administration panel, public client interface, database layer, and generation engine.