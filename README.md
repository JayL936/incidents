# incidents
Application for storing and displaying data about incidents. Deployed with Microsoft Azure on http://incidents2016.azurewebsites.net/ 

*Polish version can be found below.*

Account is required to use the application. You can create new account with limited functionality using "Register as a new user" link on first displayed page. After creating an account you must relogin to gain access to view incidents on main map in Home page. Viewing incidents is all you can do on that account without additional privilages, which can be added by administrator of the application.

Another way is to **use previously created account (qwe@qwe.pl, 123456)**. It was given an Emergency privilages, so with that account you can see only incidents connected with Emergency service. It also means that **while adding new incidents, you must check the checkbox next to "Emergency" field** in order to gain further access to this incident (including it's visibility). 

If an error occurs, please wait a moment and try again. It's mostly because Azure database has not finished launching.

Do korzystania z aplikacji wymagane jest konto. Możliwe jest utworzenie nowego konta z limitowaną funkcjonalnością poprzez wybranie linku "Register as a new user" na pierwszej wyświetlanej stronie. Po utworzeniu konta konieczne jest ponowne zalogowanie w celu uzyskania dostępu do wyświetlania incydentów na mapie na głównej stronie aplikacji. Wyświetlanie incydentów to wszystko, co można zrobić na takim koncie bez dodatkowych uprawnień przyznawanych przez administratora.

Innym sposobem na korzystanie z aplikacji jest **użycie utworzonego wcześniej konta (qwe@qwe.pl, 123456)**. Posiada ono uprawnienia Pogotowia ratunkowego, dzięki czemu możliwe jest wyświetlanie jedynie incydentów powiązanych z tą służbą. Oznacza to, że **w trakcie dodawania nowych incydentów konieczne jest zaznaczenie pola obok opcji "Emergency"**, co zapewni dalszy dostęp do tego incydentu (wliczając możliwość jego wyświetlenia).

W razie wystąpienia błędu proszę chwilę poczekać i spróbować ponownie. Zazwyczaj powodem tego jest fakt, że baza danych Azure nie zdążyła się uruchomić.

# Funkcjonalności
## Dodawanie incydentów
Incydenty można dodawać: 
1. Z poziomu strony głównej,
2. Z poziomu listy incydentów otwieranej po wybraniu opcji *Incidents* z poziomu głównego menu na górnej strony.

### Sposób pierwszy
Należy wyświetlić główną stronę aplikacji. Można to osiągnąć poprzez wybranie z głównego menu opcji *Home* lub poprzez kliknięcie nazwy aplikacji - *Incidents 2016*. Na stronie głównej należy wskazać na mapie miejsce, gdzie ma zostać dodany incydent, a następnie rozwinąć pole *Create incident...* poprzez kliknięcie znajdującego się obok napisu znaku +. W polu tekstowym wyświetla się wybrany adres, jeśli jest prawidłowy, należy kliknąć przycisk *Create incydent*, po czym nastąpi przeniesienie do formularza dodawania incydentu. Tam można uzupełnić pozostałe dane na temat incydentu oraz zaznaczyć powiązane z nim służby. Należy koniecznie zaznaczyć służbę, do której należy konto, na którym jesteśmy zalogowani, gdyż w przeciwnym przypadku incydent nie będzie później widoczny. W przypadku konta qwe@qwe.pl jest to *Emergency*. 

### Sposób drugi
Inny sposób dodawania incydentu to wybranie opcji *Incidents* na górze strony, a następnie kliknięcie linku *Create new* nad listą incydentów. Nastąpi przeniesienie na formularz dodawania incydentu, gdzie należy uzupełnić wszystkie pola. Adres może zostać dodany poprzez wybranie miejsca na mapie. Analogicznie jak w sposobie pierwszym, należy koniecznie zaznaczyć służbę, do której należy konto, na którym jesteśmy zalogowani, gdyż w przeciwnym przypadku incydent nie będzie później widoczny. W przypadku konta qwe@qwe.pl jest to *Emergency*. 

## Wyświetlanie, edycja i usuwanie incydentów
Dostęp do tych funkcji możliwy jest z poziomu list incydentów. Te znajdują się w dwóch miejscach: na stronie głównej (po kliknięciu opcji *Incidents 2016* lub *Home*), a także na stronie listy incydentów (*Incidents* w górnym menu). Aby z nich skorzystać należy wybrać incydent z listy, a następnie kliknąć przycisk odpowiedzialny za daną opcję.

Jeśli incydent widnieje na liście, ale nie ma dostępu do jego edycji, oznacza to, że został dodany przez inną służbę, która zadeklarowała nasze uczestnictwo. Symbolizuje to kolumna *Confirmed* na liście incydentów (w widoku *Incidents*). Jeśli widnieje tam wartość *False*, należy najpierw wyświetlić detale incydentu (po wybraniu incydentu i kliknięciu przycisku *Details*), gdzie można dokonać potwierdzenia uczestnictwa. Po tym możliwe jest już edytowanie jego danych.

## Zarządzanie uczestnikami incydentu
Analogicznie korzysta się z funkcji dotyczących uczestników incydentów. Dodać ich można z poziomu listy incydentów (po wybraniu w menu opcji *Incidents*, wskazaniu incydentu i kliknięciu przycisku *New Participant*) lub z poziomu listy uczestników (opcja *Participants* w głównym menu, następnie zaś link *Create new*). Pierwszy sposób skutkuje automatycznym uzupełnieniem ID incydentu, do którego ma zostać przypisany uczestnik.

Pozostałe opcje są dostępne tak samo, jak w przypadku incydentów. Należy otworzyć listę uczestników (*Participants* w głównym menu), a następnie wybrać uczestnika i przycisk odpowiedzialny za funkcję, z której chcemy skorzystać. 

## Zarządzanie szczegółowymi opisami incydentów
Możliwe jest również dodanie rozbudowanego opisu incydentu z perspektywy służby, do której należy użytkownik. W tym celu należy przejść do zakładki *My participations* (dostępna w głównym menu), gdzie można wybrać uczestnictwo i uzupełnić jego opis. Nie jest możliwe jego całkowite usunięcie (choć można usunąć opis), gdyż uczestnictwo wynika z oznaczenia danej służby w opisie incydentu. Dopiero odznaczenie w podstawowym opisie incydentu pola wyboru, które wskazuje na uczestnictwo służby, poskutkuje usunięciem tego opisu. 

## Dodatkowe opcje
### Ustalanie zakresu czasu wyświetlanych danych
Domyślnie wyświetlane są incydenty z ostatniego miesiąca. Aby to zmienić należy zmodyfikować pola dat. W przypadku list incydentów i uczestnictw należy w tym celu ustawić daty w polach znajdujących się nad nimi, co poskutkuje natychmiastowym ograniczeniem zakresu czasu, z jakiego wyświetlane są dane. 

Inaczej wygląda to na stronie głównej, gdzie należy rozwinąć pole *Set date range*, wybrać daty, a następnie kliknąć znajdujący się w polu przycisk. Poskutkuje to przeładowaniem strony i wyświetleniem danych z określonego zakresu tak w tabeli, jak i na mapie.

### Filtrowanie i sortowanie danych
W każdej tabeli możliwe jest dowolne filtrowanie i sortowanie danych. Pierwsze realizuje się poprzez wprowadzanie szukanych danych w polu wyszukiwania, drugie zaś poprzez kliknięcie strzałek w dowolnej kolumnie tabeli.

### Zliczanie incydentów w obszarze
Mapa znajdująca się na stronie głównej umożliwia zliczanie incydentów w konkretnym obszarze. W tym celu w górnej części mapy należy ze znajdującego się tam menu wybrać jeden z kształtów, a następnie narysować go na mapie. Ilość incydentów zostanie wyświetlona pod lewym dolnym rogiem mapy. Pojedyncze kliknięcie na narysowany obszar powoduje jego usunięcie.

### Wyszukiwanie adresu
W dolnej środkowej części mapy znajduje się pole tekstowe, które umożliwia wyszukanie żądanego adresu. Po kliknięciu jednej z opcji w tym miejscu automatycznie zostanie ustawiony marker.

### Legenda oznaczeń
Każdy z typu incydentu jest na mapie symbolizowany odpowiednią ikoną. Ich opisy są dostępne po wybraniu z głównego menu strony opcji *Legend*.
