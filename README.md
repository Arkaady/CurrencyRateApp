# CurrencyRateApp
Zadanie rekrutacyjne.

Aplikacja CurrencyRateApp
Wykorzystane technologie i biblioteki:
  - .Net core 3.1
  - MsSql + EntityFramework Core
  - Redis
  - FluentValidation
  - FluentAssertions
  - Serilog
  - Swagger
  - xUnit

Aplikacja zawiera 2 endpointy:
- GET "api/Auth" - służący do generowania i pobierania nowego klucza dla API. 
  Wartość hash klucza jest przechowywana w bazie danych MsSQL.
  Dostęp do bazy się poprzed ORM EntityFramework Core
- GET "api/Currency" - służy do pobierania kursu dla podanych w request par walut.
  Kursy są pobierane z zewnętrznego API: https://sdw-wsrest.ecb.europa.eu i cachowane za pomocą Redisa
  
Testy integracyjne znajdują się w projekcie CurrencyrateApp.IntegrationTests. 
Wykorzystują:
- WebApplicationFactory
- XUnit
- FluentAssertions

Testy obciążeniowe zostały wykonane przy użyciu oprogramowania "WebSurge". Zrzuty wyników znajdują się w folderze: "WebSurge"

Uruchomienie aplikacji:

Pod adresem https:localhost:44300/swagger znajduje się swagger, z którego można testować API

Do uruchomienia aplikacji na windowsie wymagany jest zainstalowany redis, ewentualnie można go uruchomić przy pomocy dockera komendą:

"docker run -p 6379:6379 --name redis-master -e REDIS_REPLICATION_MODE=master -e ALLOW_EMPTY_PASSWORD=yes redis"

Całą aplikację rónież można uruchomić za pomocą dockera, w tym celu jest stworzony docker-compose, który znajduje się w katalogu "CurrencyRateApp"






