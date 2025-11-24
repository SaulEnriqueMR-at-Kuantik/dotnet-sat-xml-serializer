## 📄 CFDI Serializer SDK for .NET
XML & JSON Models · Format · Utilities

#### 🧩 Overview

CFDI Serializer SDK is a strongly-typed C# library that provides:

- 📦 XML serialization & deserialization for CFDI 4.0 models

- 🎯 JSON serialization for REST APIs or document persistence

- 🧱 Common models for reuse across multiple projects

- 🔧 Extensible utilities for stamping, timbrado, and formatting

This package is designed to be shared across multiple microservices, ensuring consistent data structures and serialization logic.

## 🚀 Installation

Install from NuGet:

dotnet add package Kuantik.XmlModels


Or via Package Manager:

Install-Package Kuantik.XmlModels


### 📦 Quick Start
🔧 XML → Object

var xml = File.ReadAllText("cfdi.xml");

var comprobante = CfdiSerializer.DeserializeXml<Comprobante40>(xml);

🔧 Object → XML

var xml = CfdiSerializer.CreateXml(comprobante);
Console.WriteLine(xml);


### Work to future
✔ XML Validation
✔ XML Formatter