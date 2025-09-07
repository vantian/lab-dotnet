# PermissionHandler

# Prerequisites

Sebelum menjalankan project, pastikan Anda sudah menambahkan package berikut:

```pwsh
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Scalar.AspNetCore
```

**Keterangan fungsi package:**

- `Microsoft.AspNetCore.Authentication.JwtBearer`: Middleware untuk autentikasi JWT di ASP.NET Core, menangani validasi token dan integrasi dengan pipeline authentication/authorization.
- `System.IdentityModel.Tokens.Jwt`: Library untuk membuat, membaca, dan memproses token JWT di .NET, digunakan saat generate token di endpoint `/Login`.
- `Microsoft.EntityFrameworkCore.InMemory`: Provider database in-memory untuk Entity Framework Core, digunakan agar data tersimpan hanya selama aplikasi berjalan (cocok untuk demo dan testing).
- `Scalar.AspNetCore`: Library untuk menambahkan fitur API Explorer/OpenAPI/Swagger pada aplikasi, sehingga endpoint dapat didokumentasikan dan diuji langsung melalui browser.

## Deskripsi

PermissionHandler adalah contoh aplikasi ASP.NET Core minimal API yang mengimplementasikan autentikasi dan otorisasi berbasis JWT serta policy berbasis permission. Aplikasi ini menggunakan database in-memory untuk menyimpan data user, role, dan permission.

## Fitur

- Autentikasi JWT (JSON Web Token)
- Otorisasi berbasis policy dan claim permission
- Endpoint CRUD untuk data Users
- Seeder data otomatis saat development
- Scalar untuk dokumentasi API

## Keterangan data seeder

- User => hanya read user endpoint
- Admin => bisa semua

## Konfigurasi JWT

- Secret key minimal 32 karakter, disimpan di variabel `SSKey` pada `Program.cs`.
- Issuer dan Audience: `vantian.net`

## Endpoint Utama

- `POST /Login` : Mendapatkan JWT token dengan mengirimkan username.
- `GET /Users` : Mendapatkan daftar user (perlu permission `UserRead`).
- `GET /Users/{id}` : Mendapatkan detail user (perlu permission `UserRead`).
- `POST /Users` : Menambah user baru (perlu permission `UserWrite`).
- `PUT /Users/{id}` : Update data user (perlu permission `UserWrite`).
- `DELETE /Users/{id}` : Hapus user (perlu permission `UserDelete`).

## Cara Autentikasi

1. Login dengan endpoint `/Login` untuk mendapatkan token JWT.
2. Sertakan token pada header setiap request:
   ```
   Authorization: Bearer <token>
   ```

## Proses Validasi Authorization

Authorization divalidasi oleh middleware ASP.NET Core secara otomatis setelah Anda menambahkan baris berikut di `Program.cs`:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

Ketika request masuk ke endpoint yang diberi atribut `RequireAuthorization` atau policy tertentu, middleware akan:

- Mengecek header Authorization dan memvalidasi token JWT menggunakan konfigurasi di `AddJwtBearer` pada `Program.cs`.
  `builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>.....`
- Mengecek claim permission pada token sesuai policy yang ditentukan di `AddAuthorization` pada `Program.cs`.
  `builder.Services.AddAuthorization(options =>`
- Jika token valid dan claim sesuai, request diteruskan ke endpoint. Jika tidak, akan dikembalikan error 401/403.

## Penjelasan SymmetricSecurityKey dan SigningCredentials

- **SymmetricSecurityKey** digunakan untuk membuat dan memvalidasi signature pada token JWT. Key ini harus sama antara pembuatan token dan validasi di middleware agar token bisa diverifikasi.
- **SigningCredentials** digunakan saat membuat token JWT untuk menentukan algoritma dan key yang dipakai untuk menandatangani token. Tanpa signature, token tidak bisa diverifikasi keasliannya dan tidak aman.

Jadi, kedua komponen ini penting agar token JWT yang dihasilkan benar-benar aman dan hanya bisa divalidasi oleh server yang memiliki secret key yang sama.

## Konfigurasi Authorization Policy

Authorization policy dikonfigurasi di bagian berikut pada `Program.cs`:

```csharp
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy(PermissionsConst.UserRead, policy => policy.RequireClaim("permission", PermissionsConst.UserRead));
   options.AddPolicy(PermissionsConst.UserWrite, policy => policy.RequireClaim("permission", PermissionsConst.UserWrite));
   options.AddPolicy(PermissionsConst.UserDelete, policy => policy.RequireClaim("permission", PermissionsConst.UserDelete));
   options.AddPolicy(PermissionsConst.RoleRead, policy => policy.RequireClaim("permission", PermissionsConst.RoleRead));
   options.AddPolicy(PermissionsConst.RoleWrite, policy => policy.RequireClaim("permission", PermissionsConst.RoleWrite));
   options.AddPolicy(PermissionsConst.RoleDelete, policy => policy.RequireClaim("permission", PermissionsConst.RoleDelete));
});
```

Setiap policy mengharuskan token JWT memiliki claim `permission` sesuai dengan permission yang dibutuhkan. Policy ini digunakan pada endpoint dengan method `RequireAuthorization(PermissionsConst.PermissionName)`, sehingga hanya user dengan permission yang sesuai yang bisa mengakses endpoint tersebut.

## Struktur Project

- `Program.cs` : Entry point dan konfigurasi utama aplikasi
- `Entities/` : Model data (Users, Roles, RolePermissionMapping)
- `Persistent/` : DbContext dan Seeder
- `Constant/PermissionsConst.cs` : Daftar permission

## Catatan

- Semua endpoint (kecuali `/Login`) membutuhkan JWT dan permission yang sesuai.
- Data akan di-reset setiap aplikasi dijalankan ulang (karena menggunakan InMemoryDb).

## Lisensi

Proyek ini hanya untuk pembelajaran dan demonstrasi.
