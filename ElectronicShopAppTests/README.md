# Testy

Uruchomienie z katalogu głównego (wymagany .NET SDK 10 i runtime ASP.NET Core 8):

```powershell
dotnet test ElectronicShopAppTests/ElectronicShopAppTests.csproj
```

Raport pokrycia w formacie Cobertura:

```powershell
dotnet test ElectronicShopAppTests/ElectronicShopAppTests.csproj --collect:"XPlat Code Coverage"
```

## Zakres

- `ProductServiceTests`: zapis i odczyt produktów, brak produktu, granice cen, sortowanie przed stronicowaniem, dostępność, brak częściowej zmiany/usunięcia przy błędnym ID.
- `ShoppingCartServiceTests`: walidacja całej paczki, zapis ilości, rozdzielenie danych klientów przy zmianach i wyszukiwaniu, wyszukiwanie po nazwie i opisie.
- `OrderServiceTests`: zamówienie gościa, ceny z bazy zamiast sumy przesłanej przez klienta, pozycje zamówienia, zmniejszenie zapasu, odrzucenie braku towaru i pustego miasta.
- `ProductsClientTests`: ścieżka HTTP i parametry strony, odczyt JSON, błędne odpowiedzi i awaria sieci.
- `ShoppingCartsControllerTests`: przekazanie tożsamości klienta, oczekiwanie na operację asynchroniczną, obsługa jej błędu i niepoprawnej tożsamości.

Każdy test usług API ma oddzielną bazę EF Core InMemory. Testy zapisu sprawdzają wynik także przez nowy kontekst. Testy klienta używają własnego `HttpMessageHandler`, bez połączeń sieciowych. Nie potrzeba uruchomionego API, SQL Servera ani emulatora MAUI.

## Ograniczenia

Testy HTTP uruchamiają API wewnątrz procesu testowego. Przechodzą przez routing, middleware JWT i rzeczywiste kontrolery. Serwer testowy działa na .NET 10 z osobną bazą EF InMemory dla każdego przypadku. Ograniczenia relacyjne, transakcje i zgodność z SQL Serverem wymagają osobnych testów na SQL Serverze.

- `CustomerCheckoutTests` sprawdza zapis pozycji z koszyka właściciela, cenę katalogową, stan magazynu, odrzucenie całego zamówienia przy braku towaru oraz tożsamość kupującego na poziomie HTTP.
- `ListOwnershipTests` obejmuje odczyt list, zmianę nazwy i ilości, odrzucenie zerowej ilości oraz próbę zmiany cudzej listy przez podanie identyfikatora jej właściciela w treści żądania.
- `AccessTests` sprawdza logowanie przez e-mail lub telefon, odrzucenie nieprawidłowych danych oraz uprawnienie do modyfikacji produktu. Każdy scenariusz modyfikacji sprawdza też cenę w bazie po żądaniu.

## Błędy wykryte przez testy regresyjne

- Wyszukiwanie koszyka porównywało ID wiersza z ID klienta.
- Filtr `IsInStock = false` wybierał towary dostępne.
- Pobieranie produktów ignorowało parametry strony.
- Zmiana/usuwanie koszyka używały zapytania nieobsługiwanego przez EF.
- Kontroler wyszukiwania zwracał obiekt `Task` zamiast oczekiwać na dane; nie przechwytywał też błędów asynchronicznych.

Każdy z tych przypadków najpierw odtworzono testem, następnie poprawiono kod produkcyjny.
