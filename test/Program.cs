using Hasher = BCrypt.Net.BCrypt;

using BCrypt.Net;

// Parolayı belirle
string password = "1234";

// Hash oluştur
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

// Ekrana yaz
Console.WriteLine(hashedPassword);
