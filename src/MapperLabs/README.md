# MapperLabs 🚀

[![NuGet Version](https://img.shields.io/nuget/v/MapperLabs.svg?style=flat)](https://www.nuget.org/packages/MapperLabs)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MapperLabs.svg)](https://www.nuget.org/packages/MapperLabs)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

**MapperLabs** is a high-performance, flexible object mapping library for .NET applications. It uses **Expression Trees** and **IL Emit** instead of reflection for maximum performance.

![Demo](https://raw.githubusercontent.com/yourusername/MapperLabs/main/docs/demo.gif)

---

## 🌟 Key Features
| Feature | Description |
|---------|-------------|
| ⚡ **Zero Reflection** | Optimized with Expression Trees and Dynamic Methods |
| 🛠 **Fluent API** | Chainable configuration for readability |
| 🔄 **Case-Insensitive** | Auto-mapping for `UserName` ↔ `username` |
| 📦 **DI Support** | Seamless ASP.NET Core integration |
| 🧩 **Nested Mapping** | Deep mapping for nested objects |
| 📊 **Collections** | Support for `List<T>`, `Dictionary<TKey,TValue>` |

---

## 📦 Installation
```bash
dotnet add package MapperLabs

🚀 Quick Start

1. Basic Mapping

public class UserDto 
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class UserEntity 
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Configuration
TypeAdapterConfig<UserDto, UserEntity>.DefaultConfig();

// Usage
var userEntity = userDto.Adapt<UserEntity>();


2. Custom Mapping

TypeAdapterConfig<ProductDto, ProductEntity>
    .ForType()
    .ForMember(dest => dest.TotalPrice, src => src.Price * src.Quantity) // Custom calculation
    .ForMember(dest => dest.SKU, opt => opt.Ignore()) // Ignore property
    .Register();
	
	
3. Collection Mapping

var products = productDtos.Adapt<List<ProductEntity>>(); // List mapping
var dict = sourceList.Adapt<Dictionary<int, string>>();  // Dictionary support

📊 Performance Benchmarks
Library	1M Objects (ms)	Key Features
MapperLabs	12	✅ Zero Reflection, ✅ Fluent API
Mapster	18	⚠️ Partial Reflection
AutoMapper	145	⚠️ Full Reflection-Based
Test Environment: .NET 8.0, Intel i7-11800H, 32GB RAM

📚 Advanced Usage
Nested Object Mapping

public class OrderDto 
{
    public CustomerDto Customer { get; set; }
    public List<ItemDto> Items { get; set; }
}

public class OrderEntity 
{
    public CustomerEntity Customer { get; set; }
    public List<ItemEntity> Items { get; set; }
}

// Auto nested mapping
TypeAdapterConfig<OrderDto, OrderEntity>.DefaultConfig();

Custom Converters

public class DateToAgeConverter : IConverter<DateTime, int>
{
    public int Convert(DateTime source) => DateTime.Now.Year - source.Year;
}

// Configuration
TypeAdapterConfig<UserDto, UserEntity>
    .ForType()
    .ForMember(dest => dest.Age, src => src.BirthDate.ConvertUsing<DateToAgeConverter>());
	
	

👨💻 Contributing
Fork the repository:
git clone https://github.com/tolgatandogan/MapperLabs.git

Create a branch:
git checkout -b feature/your-feature-name

Run tests:
dotnet test tests/SmartMapper.Tests

Commit changes:
git commit -m "feat: Add your feature"

Push and open a PR!	


---

## 📜 License
Licensed under the [MIT License](LICENSE).

**Contact:**  
📧 tolgatandogan@yandex.com | [GitHub Profile](https://github.com/tolgatandogan)