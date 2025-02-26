Sipariş Takip Sistemi

Bu proje, Dapper ve .NET Core kullanılarak geliştirilen bir sipariş takip sistemidir. Müşteri yönetimi, ürün yönetimi, sipariş işlemleri ve kampanya uygulamaları gibi temel özellikler içermektedir.
Ayrıca, Vue.js kullanılarak geliştirilen bir frontend ile etkileşimli bir kullanıcı arayüzü sağlamaktadır.

Özellikler

Müşteri Yönetimi: Müşteri ekleme, silme ve listeleme işlemleri.

Ürün Yönetimi: Ürün ekleme, güncelleme, silme ve listeleme işlemleri.

Sipariş Yönetimi: Sipariş oluşturma, güncelleme, silme ve listeleme işlemleri.

Kampanya Sistemi: Belirli bir tutarın üzerindeki siparişlere otomatik indirim uygulanması.

Kupon Kullanımı: Geçerlilik süresi olan indirim kodlarının kullanımı.

Veri Tabanı Yönetimi: PostgreSQL kullanılarak veritabanı işlemleri.

ORM Kullanımı: Dapper ile hızlı ve verimli veri işlemleri.

DTO Kullanımı: AutoMapper ile veri dönüşümleri.

İş Kuralları: İş mantığını düzenleyen ayrı sınıflar ile temiz kod yapısı.

Kullanılan Teknolojiler

Backend: .NET Core 6, Dapper, AutoMapper

Veritabanı: PostgreSQL

Frontend: Vue.js

Diğer: RESTful API, Dependency Injection, Entity-DTO MappingAPI Kullanımı


Aşağıdaki temel API uç noktaları kullanılabilir:

Müşteri İşlemleri: /api/customers

Ürün İşlemleri: /api/products

Sipariş İşlemleri: /api/orders

Kampanya İşlemleri: /api/campaigns

Migration sistemimiz olmadığı için veritabanını el ile oluşturuyoruz. 

PostgreSQL'de bir database oluşturuyoruz ve aşağıda vereceğim kodları query tool bölmesine yapıştırıp çalıştırıyoruz.
WebAPI içerisindeki appsetting.json dosyasının içinde connection stringimiz bulunmaktadır.
Connection stringimizi kendimize göre düzenliyoruz ve artık çalışmaya hazırız.

CREATE TABLE Customers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(50),
    address TEXT
);

CREATE TABLE Campaigns (
    id SERIAL PRIMARY KEY,
    code VARCHAR(50) NOT NULL,
    discountrate DECIMAL(5,2) NOT NULL,
    minimumamount DECIMAL(10,2) NOT NULL,
    expirationdate TIMESTAMP NOT NULL,
    isexpired BOOLEAN DEFAULT FALSE,
    customerid INT,
    CONSTRAINT fk_campaign_customer FOREIGN KEY (customerid) REFERENCES Customers(id) ON DELETE SET NULL
);

CREATE TABLE Products (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL,
    category VARCHAR(100)
);

CREATE TABLE Orders (
    id SERIAL PRIMARY KEY,
    customerid INT NOT NULL,
    orderdate TIMESTAMP NOT NULL DEFAULT NOW(),
    totalamount DECIMAL(10,2) NOT NULL,
    status VARCHAR(50) NOT NULL,
    discountcode VARCHAR(50),
    CONSTRAINT fk_orders_customer FOREIGN KEY (customerid) REFERENCES Customers(id) ON DELETE CASCADE
);

CREATE TABLE OrderDetails (
    id SERIAL PRIMARY KEY,
    orderid INT NOT NULL,
    productid INT NOT NULL,
    quantity INT NOT NULL,
    unitprice DECIMAL(10,2) NOT NULL,
    CONSTRAINT orderdetails_productid_fkey FOREIGN KEY (productid) REFERENCES Products(id) ON DELETE CASCADE,
    CONSTRAINT orderdetails_orderid_fkey FOREIGN KEY (orderid) REFERENCES Orders(id) ON DELETE CASCADE
);

