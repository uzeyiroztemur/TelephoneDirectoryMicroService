## Proje Hakkýnda

Birbirleri ile haberleþen minimum iki microservice'in olduðu bir yapý tasarlayarak, basit
bir telefon rehberi uygulamasý oluþturulmasý saðlanacaktýr.

Detaylý bilgilere aþaðýdaki adres yolundan belirtilen pdf dosyasýndan ulaþabilirsiniz.

```
asset/Assessment.pdf
```

<h3><b>Telefon Rehberi Projesinde Kullanýlan Teknolojiler</b></h2>
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
	<li>Docker</li>
</ul>

<h3><b>Telefon Rehberi Projesi Gereklilikler</b></h2>
<br>

Bilgisayarýnýzda RabbitMQ Postgres kurulu olmalýdýr. Eðer deðilse Docker kurulumu yapýlarak aþaðýdaki yol izlenebilir.

1.) Aþaðýdaki adres üzerinden docker indirilip kurulmalýdýr.

```
https://www.docker.com/products/docker-desktop/
```

<br>

2.) Docker üzerinde Postgres kurulumu için aþaðýdaki komutu kullanabilirsiniz.

```
docker run --name postgresql -e POSTGRES_PASSWORD=rI1l9j!p7gO@ -p 5432:5432 -d postgres
```

<br>

3.) Docker üzerinde RabbitMQ kurulumu için aþaðýdaki komutu kullanabilirsiniz.

```
docker run --name rabbitmq -d -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

<h3><b>Telefon Rehberi Projesinin Çalýþtýrýlmasý</b></h2>

<br>

1.) Öncelikle aþaðýdaki dizinlerde bulunan <b>appsettings.json</b> config ve path ayarlamalarýný kendinize göre ayarlayýnýz.

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

2.) Aþaðýda belirtilen pathler için migrationlarýnýn çalýþtýrýlmasý saðlanacaktýr. 

```
src/Auth/DataAccess
src/Contact/DataAccess
src/Report/DataAccess
```

```
update-database
```

<br>

3.) Bu iþlemler yapýldýktan sonra aþaðýdaki servisler çalýþtýrýlarak tüm süreçler gateway api servisi üzerinden yapýlabilir.

<br><br>

Token Alma 
```
{
  "userName": "uzeyiroztemur@gmail.com",
  "password": "123456aA!"
}
```


