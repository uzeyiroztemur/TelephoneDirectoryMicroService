## Proje Hakk�nda

Birbirleri ile haberle�en minimum iki microservice'in oldu�u bir yap� tasarlayarak, basit
bir telefon rehberi uygulamas� olu�turulmas� sa�lanacakt�r.

Detayl� bilgilere a�a��daki adres yolundan belirtilen pdf dosyas�ndan ula�abilirsiniz.

```
asset/Assessment.pdf
```

<h3><b>Telefon Rehberi Projesinde Kullan�lan Teknolojiler</b></h2>
<ul>
	<li>.NET Core 6</li>
	<li>Entity Framework Core 6</li>
	<li>Postgres</li>
	<li>RabbitMQ</li>
	<li>MassTransit</li>
	<li>HostedService</li>
	<li>Ocelot</li>
	<li>Swagger</li>
	<li>Health Check</li>
	<li>Logging</li>
	<li>xUnit</li>
	<li>Moq</li>
	<li>Coverlet</li>
	<li>Docker</li>
</ul>

<h3><b>Telefon Rehberi Projesi Gereklilikler</b></h2>
<br>

Bilgisayar�n�zda RabbitMQ Postgres kurulu olmal�d�r. E�er de�ilse Docker kurulumu yap�larak a�a��daki yol izlenebilir.

1.) A�a��daki adres �zerinden docker indirilip kurulmal�d�r.

```
https://www.docker.com/products/docker-desktop/
```

<br>

2.) Docker �zerinde Postgres kurulumu i�in a�a��daki komutu kullanabilirsiniz.

```
docker run --name postgresql -e POSTGRES_PASSWORD=rI1l9j!p7gO@ -p 5432:5432 -d postgres
```

<br>

3.) Docker �zerinde RabbitMQ kurulumu i�in a�a��daki komutu kullanabilirsiniz.

```
docker run --name rabbitmq -d -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

<h3><b>Telefon Rehberi Projesinin �al��t�r�lmas�</b></h2>

<br>

1.) �ncelikle a�a��daki dizinlerde bulunan <b>appsettings.json</b> config ve path ayarlamalar�n� kendinize g�re ayarlay�n�z.

```
src/Auth/API/appsettings.json
	-> Logging.Settings
	-> ConnectionStrings.PostgreSql

src/Contact/API/appsettings.json
	-> Logging.Settings
	-> ConnectionStrings.PostgreSql

src/Report/API/appsettings.json
	-> Logging.Settings
	-> ConnectionStrings.PostgreSql
	-> RabbitMQOptions

src/Gateway/API/appsettings.json
	-> Logging.Settings
```

<br>

2.) A�a��da belirtilen pathler i�in migrationlar�n�n �al��t�r�lmas� sa�lanacakt�r. 

```
src/Auth/DataAccess
src/Contact/DataAccess
src/Report/DataAccess
```

```
update-database
```

<br>

3.) Bu i�lemler yap�ld�ktan sonra a�a��daki servisler �al��t�r�larak t�m s�re�ler gateway api servisi �zerinden yap�labilir.

<br><br>

```
src/Auth/API
src/Contact/API
src/Report/API
src/Gateway/API
```

<br>

Token Alma 

```
{
  "userName": "uzeyiroztemur@gmail.com",
  "password": "123456aA!"
}
```


4.) Postman testi i�inde postman collection dosyalar� eklenmi�tir.


```
test/TelephoneDirectory.postman_collection.json
test/TelephoneDirectory.postman_environment.json
```


5.) Projenin çalışma mimarisi için aşağıdaki görsele bakabilirsiniz.

```
asset/diyagram.jpg
```
