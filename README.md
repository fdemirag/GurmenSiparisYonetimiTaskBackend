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
