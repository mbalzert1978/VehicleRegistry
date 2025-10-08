# PRD-0001: Register User Feature

**Date:** 8. Oktober 2025  
**Status:** Draft  
**Project:** VehicleRegistry - UserManagement Backend API  
**Version:** 1.0

---

## 1. Introduction/Overview

Das "Register User" Feature ermöglicht es neuen Benutzern, sich im VehicleRegistry System zu registrieren. Die Registrierung bildet die Grundlage für die spätere Nutzung der Fahrzeugregistrierungs-Funktionalitäten. Benutzer müssen persönliche Daten, Adressinformationen sowie Authentifizierungsdaten angeben. Nach erfolgreicher Registrierung erhalten Benutzer eine Email zur Verifizierung ihrer Email-Adresse.

**Problem:** Neue Benutzer benötigen einen sicheren und benutzerfreundlichen Weg, um sich im System zu registrieren.

**Ziel:** Implementierung eines RESTful API Endpoints (HATEOAS Level 3) zur Benutzerregistrierung mit Email-Verifizierung, sicherer Passwortspeicherung und strukturierter Datenverwaltung.

---

## 2. Goals

1. **Sichere Benutzerregistrierung:** Implementierung einer sicheren Registrierungsfunktionalität mit Argon2 Password-Hashing und OAuth2/OpenID Connect Unterstützung
2. **Datentrennung:** Klare Trennung zwischen Authentifizierungsdaten (User) und Profildaten (UserProfile/Address)
3. **Email-Verifizierung:** Implementierung eines RFC 6854 konformen Email-Verifizierungsprozesses
4. **Erweiterbarkeit:** Flexible Adressstruktur, die zukünftige Erweiterungen ermöglicht
4.5. **Type Safety:** Verwendung von ValueObjects und Strongly-Typed IDs anstelle primitiver Typen - "Make illegal states unrepresentable"
5. **Standards-Konformität:** RESTful API Design (HATEOAS Level 3) mit RFC 7807 Problem Details für Fehlerbehandlung
6. **Robuste Validierung:** Implementierung umfassender Validierungsregeln (Email-Format, Passwortlänge)
7. **API Discoverability:** HATEOAS-Links ermöglichen Client-Navigation ohne Hard-Coding von URLs

---

## 3. User Stories

### US-1: Benutzerregistrierung

**Als** neuer Benutzer  
**möchte ich** mich mit meinen persönlichen Daten, Adresse, Email und Passwort registrieren  
**damit** ich Zugang zum VehicleRegistry System erhalte.

**Akzeptanzkriterien:**

- Benutzer kann Vorname, Nachname, Adressdaten, Email und Passwort angeben
- System validiert alle Eingaben nach definierten Rules
- Bei erfolgreicher Registrierung wird HTTP 201 Created zurückgegeben
- Benutzer erhält automatisch die Rolle "User"

### US-2: Email-Verifizierung

**Als** registrierter Benutzer  
**möchte ich** meine Email-Adresse verifizieren  
**damit** ich mein Konto aktivieren und das System nutzen kann.

**Akzeptanzkriterien:**

- System sendet automatisch eine Verifizierungs-Email nach der Registrierung
- Email enthält einen eindeutigen Link mit Token
- Token hat eine begrenzte Gültigkeit (z.B. 24 Stunden)
- Nach Klick auf den Link ist die Email verifiziert
- Benutzer kann sich erst nach Email-Verifizierung einloggen

### US-3: Sichere Passwortverwaltung

**Als** neuer Benutzer  
**möchte ich** ein sicheres Passwort erstellen  
**damit** mein Account vor unbefugtem Zugriff geschützt ist.

**Akzeptanzkriterien:**

- Passwort muss mindestens 16 Zeichen lang sein
- Passwort wird mit Argon2 gehasht gespeichert
- Klartext-Passwort wird niemals in der Datenbank gespeichert

### US-4: Duplikat-Prävention

**Als** System  
**möchte ich** verhindern, dass sich Benutzer mehrfach mit derselben Email registrieren  
**damit** die Datenintegrität gewährleistet ist.

**Akzeptanzkriterien:**

- System prüft bei Registrierung, ob Email bereits existiert
- Bei Duplikat wird generische Fehlermeldung zurückgegeben (Security by obscurity)
- Keine Information darüber, ob Email bereits registriert ist

---

## 4. Functional Requirements

### FR-1: API Endpoint

- **FR-1.1:** Der Endpoint `POST /api/v1/users/register` muss implementiert werden
- **FR-1.2:** Der Endpoint muss JSON als Content-Type akzeptieren
- **FR-1.3:** Der Endpoint muss mit API-Versionierung (v1) versehen sein

### FR-2: Request Datenstruktur

- **FR-2.1:** Der Request muss folgende Pflichtfelder enthalten:
  - `firstName` (string): Vorname des Benutzers
  - `lastName` (string): Nachname des Benutzers
  - `email` (string): Email-Adresse
  - `password` (string): Passwort (mindestens 16 Zeichen)
  - `address` (object): Adressobjekt mit erweiterbarer Struktur

### FR-3: Datenbankstruktur

- **FR-3.1:** Drei separate Entities müssen erstellt werden:
  - **User Entity:** Enthält `UserId` (StronglyTypedId), `Email` (ValueObject), `PasswordHash` (ValueObject), `Role` (ValueObject), `EmailVerificationStatus` (Discriminated Union mit `VerificationToken`), `CreatedAt`, `UpdatedAt`
  - **UserProfile Entity:** Enthält `UserProfileId` (StronglyTypedId), `UserId` (Foreign Key), `FirstName` (ValueObject), `LastName` (ValueObject), `CreatedAt`, `UpdatedAt`
  - **Address Entity:** Separate Tabelle mit `AddressId` (StronglyTypedId), `UserProfileId` (Foreign Key) und ValueObjects für Adressfelder (`Street`, `City`, `PostalCode`, `Country`)
- **FR-3.2:** Alle primitiven Typen müssen durch ValueObjects ersetzt werden:
  - `Email`: ValueObject mit Validierung gemäß Rust Test-Suite
  - `PasswordHash`: ValueObject, wird nur durch Factory Method erstellt
  - `Name` (FirstName, LastName): ValueObject mit Validierung (nicht leer, max. Länge)
  - `UserRole`: ValueObject mit vordefinierten Werten (User, Admin)
  - `VerificationToken`: ValueObject mit Token-Wert und Ablaufzeit
  - Strongly-Typed IDs für alle Entity-IDs (UserId, UserProfileId, AddressId)
- **FR-3.3:** `EmailVerificationStatus` muss als Discriminated Union implementiert werden:
  - `NotVerified(VerificationToken Token)`: Initialer Status mit Token
  - `Verified(DateTimeOffset VerifiedOn)`: Status nach erfolgreicher Verifizierung
  - `Expired(VerificationToken ExpiredToken)`: Wenn Token abgelaufen ist
- **FR-3.4:** Keine nullable Types verwenden - "Make illegal states unrepresentable":
  - Entities haben immer vollständige, gültige Daten
  - Fehlende oder ungültige Daten führen zu Fehlern bei der Erstellung (Factory Methods mit Result<T>)
  - Kein `null` für optionale Werte - stattdessen **Null Object Pattern**:
    - Optionale Daten werden durch spezielle Empty-Objekte repräsentiert (z.B. `Address.Empty`)
    - Empty-Objekte haben klare Semantik und können gezielt abgefragt werden
    - EF Core mappt Empty-Objekte auf `NULL` in der Datenbank (via Value Converters)
    - Beispiel: Wenn User keine Adresse angibt → `Address.Empty` statt `Address? = null`
- **FR-3.5:** Factory Methods für alle Entities:
  - Entities können nicht mit `new` erstellt werden
  - Factory Methods validieren und geben `Result<Entity>` zurück
  - Sicherstellen, dass Entities immer in gültigem Zustand sind
- **FR-3.6:** PostgreSQL muss als Datenbank verwendet werden
- **FR-3.7:** Alle Entities müssen entsprechende Indizes für Performance haben (z.B. Unique Index auf User.Email.Value)

### FR-4: Email-Validierung

- **FR-4.1:** Email-Adressen müssen gemäß den bereitgestellten Test-Cases validiert werden (Rust-basierte Tests)
- **FR-4.2:** RFC 5321 Limits müssen eingehalten werden:
  - Lokaler Teil (vor @): maximal 64 Zeichen
  - Domain Teil (nach @): maximal 255 Zeichen
- **FR-4.3:** Folgende Email-Formate müssen unterstützt/abgelehnt werden (siehe Test-Cases):
  - Gültig: Standard-Emails, IP-Adressen in Brackets, IDN Domains
  - Ungültig: Trailing dots, Newlines, Underscores in Domain, ungültige IP-Adressen

### FR-5: Passwort-Handling

- **FR-5.1:** Passwort muss mindestens 16 Zeichen lang sein
- **FR-5.2:** Passwort muss mit Argon2 Algorithmus gehasht werden
- **FR-5.3:** Das Password-Hashing muss hinter einem Interface implementiert werden (`IPasswordHasher` oder ähnlich)
- **FR-5.4:** Salt muss automatisch generiert und zum Hash hinzugefügt werden
- **FR-5.5:** Klartext-Passwörter dürfen niemals geloggt oder gespeichert werden

### FR-6: Email-Verifizierung

- **FR-6.1:** Nach erfolgreicher Registrierung muss automatisch eine Verifizierungs-Email gesendet werden
- **FR-6.2:** Email muss einen eindeutigen Token-Link enthalten
- **FR-6.3:** Token muss kryptographisch sicher generiert werden (z.B. GUID oder kryptografisch sicherer Random String)
- **FR-6.4:** Token muss zeitlich begrenzt gültig sein (empfohlen: 24 Stunden)
- **FR-6.5:** Ein separater Endpoint `GET/PUT /api/v1/users/verify-email?token={token}` muss den Verifizierungsprozess abschließen
- **FR-6.6:** Nach erfolgreicher Verifizierung muss `EmailVerificationStatus` auf `EmailVerified(DateTimeOffset.UtcNow)` gesetzt werden
- **FR-6.7:** Bei der Registrierung wird `EmailVerificationStatus` initial auf `EmailNotVerified` gesetzt
- **FR-6.8:** Email-Verifizierung muss gemäß RFC 6854 implementiert werden

### FR-7: Benutzerrollen

- **FR-7.1:** Bei der Registrierung muss automatisch die Rolle "User" zugewiesen werden
- **FR-7.2:** Die Rolle muss in der User Entity gespeichert werden

### FR-8: Duplikat-Prävention

- **FR-8.1:** System muss vor dem Anlegen prüfen, ob Email bereits existiert
- **FR-8.2:** Bei existierender Email muss eine generische Fehlermeldung zurückgegeben werden
- **FR-8.3:** Die Fehlermeldung darf NICHT offenlegen, dass die Email bereits existiert (Security Requirement)
- **FR-8.4:** HTTP Status Code 400 Bad Request mit RFC 7807 Problem Details

### FR-9: Response Format

- **FR-9.1:** Bei erfolgreicher Registrierung (HTTP 201):
  - `userId`: GUID des neu erstellten Users
  - `email`: Bestätigung der registrierten Email
  - `message`: Erfolgs-Nachricht mit Hinweis auf Email-Verifizierung
  - `links`: Dictionary mit HATEOAS Links (siehe FR-14)
- **FR-9.2:** Location Header muss auf die Ressource zeigen: `Location: /api/v1/users/{userId}`
- **FR-9.3:** HATEOAS Links müssen alle verfügbaren Aktionen für den aktuellen Zustand des Benutzers enthalten
- **FR-9.4:** Links müssen dynamisch basierend auf dem Zustand generiert werden (z.B. "verify-email" nur wenn noch nicht verifiziert)

### FR-10: Fehlerbehandlung (RFC 7807 Problem Details)

- **FR-10.1:** Alle Fehler müssen im RFC 7807 Format zurückgegeben werden:
  - `type`: URI zur Problem-Typ Dokumentation
  - `title`: Kurze, lesbare Zusammenfassung
  - `status`: HTTP Status Code
  - `detail`: Detaillierte Beschreibung des Problems
  - `instance`: URI der betroffenen Ressource
  - `errors`: Dictionary mit feldspezifischen Validierungsfehlern (optional)
- **FR-10.2:** HTTP Status Codes:
  - `201 Created`: Erfolgreiche Registrierung
  - `400 Bad Request`: Validierungsfehler
  - `409 Conflict`: Email bereits registriert (mit generischer Nachricht)
  - `500 Internal Server Error`: Unerwarteter Serverfehler

### FR-11: Authentifizierung

- **FR-11.1:** OAuth2/OpenID Connect muss als Authentifizierungsframework implementiert werden
- **FR-11.2:** JWT Tokens müssen für die Token-basierte Authentifizierung verwendet werden
- **FR-11.3:** OAuth2 Flow muss die Registrierung unterstützen (Resource Owner Password Credentials Flow oder Authorization Code Flow)

### FR-12: Validation Rules Pattern

- **FR-12.1:** Alle Validierungsregeln müssen nach dem **Composite Pattern** implementiert werden
- **FR-12.2:** Jede Validierungsregel ist **atomic** und prüft **genau eine Bedingung**
- **FR-12.3:** Beispiele für atomic Rules:
  - `EmailNotEmptyRule`: Prüft nur, ob Email nicht leer ist
  - `OnlyOneAtSymbolRule`: Prüft nur, ob genau ein @ vorhanden ist
  - `LocalPartMaxLengthRule`: Prüft nur die Länge des lokalen Teils
  - `NoTrailingDotRule`: Prüft nur, ob Email nicht mit Punkt endet
  - Etc. für alle 47 Test-Cases
- **FR-12.4:** `RuleComposer<T>` komponiert mehrere atomic Rules:

  ```csharp
  new RuleComposer<string>(
      new EmailNotEmptyRule(),
      new OnlyOneAtSymbolRule(),
      new LocalPartMaxLengthRule(),
      new DomainPartMaxLengthRule(),
      new NoTrailingDotRule(),
      // ... weitere Rules
  )
  ```

- **FR-12.5:** Jede Rule gibt `Result` zurück (Success oder Failure mit Error-Message)
- **FR-12.6:** `RuleComposer` sammelt alle Fehler und gibt kombiniertes Result zurück
- **FR-12.7:** Für jeden der 47 Email-Test-Cases muss eine entsprechende Rule-Klasse existieren
- **FR-12.8:** Rules sind wiederverwendbar und können in verschiedenen Kontexten eingesetzt werden
- **FR-12.9:** Jede Rule-Klasse muss einen entsprechenden Unit-Test haben
- **FR-12.10:** VERBOTEN: Eine einzelne `EmailValidationRule`-Klasse, die alle Validierungen enthält

### FR-13: Adress-Management

- **FR-13.1:** Adresse muss in separater Tabelle gespeichert werden
- **FR-13.2:** Adress-Schema muss erweiterbar sein (z.B. für zukünftige Felder wie Bundesland, Land, etc.)
- **FR-13.3:** Basis-Adressfelder für MVP können minimal sein, müssen aber Erweiterungen unterstützen
- **FR-13.4:** Adresse ist **optional** bei der Registrierung (Null Object Pattern):
  - Wenn User keine Adresse angibt → `Address.Empty` wird verwendet
  - `Address.Empty` ist ein spezielles ValueObject, das "keine Adresse" repräsentiert
  - EF Core Value Converter mappt `Address.Empty` auf `NULL` in der Datenbank
  - VERBOTEN: `Address? address = null` verwenden

### FR-14: HATEOAS (Hypermedia as the Engine of Application State)

- **FR-14.1:** Alle API Responses müssen HATEOAS-konforme Links enthalten
- **FR-14.2:** Links müssen alle verfügbaren Aktionen für den aktuellen Ressourcen-Zustand anzeigen
- **FR-14.3:** Jeder Link muss folgende Eigenschaften haben:
  - `href`: Die URL zur Ressource/Aktion
  - `method`: HTTP Methode (GET, POST, PUT, DELETE, etc.)
  - `description` (optional): Beschreibung der Aktion
- **FR-14.4:** Link-Namen müssen semantisch aussagekräftig sein (z.B. "self", "verify-email", "resend-verification")
- **FR-14.5:** Links müssen zustandsabhängig sein:
  - Nach Registrierung: "self", "verify-email", "resend-verification", "login"
  - Nach Email-Verifizierung: "self", "login", "update-profile"
  - Etc.
- **FR-14.6:** URLs in Links müssen absolute oder relative Pfade sein (konsistent im gesamten System)

---

## 5. Non-Goals (Out of Scope)

1. **Login-Funktionalität:** Wird in separatem Feature implementiert (PRD-0002)
2. **Passwort-Reset:** Wird in separatem Feature implementiert
3. **Profil-Bearbeitung:** Wird in separatem Feature implementiert
4. **Social Login (Google, Facebook, etc.):** Kommt in späterer Version
5. **2-Faktor-Authentifizierung (2FA):** Nicht Teil des MVP
6. **Account-Löschung:** Wird in separatem Feature implementiert
7. **GDPR-Compliance Features:** Basis-Compliance wird sichergestellt, erweiterte Features kommen später
8. **Rate Limiting:** Wird auf Infrastructure-Ebene separat implementiert
9. **CAPTCHA:** Nicht im initialen MVP
10. **Detaillierte Adressvalidierung:** Initiale Version ohne PLZ-Validierung oder Adress-Verifikation
11. **Multi-Language Support:** Erstmal nur englische Fehlermeldungen

---

## 6. Design Considerations

### 6.1 Clean Architecture

Das Feature folgt dem Clean Architecture Ansatz:

- **Domain Layer:** Entities (User, UserProfile, Address), ValueObjects, Domain Events
- **Application Layer:** Use Cases (Commands/Queries mit Handlers), DTOs, Validation Rules, Request/Response Interfaces
- **Infrastructure Layer:** Repository Implementations, Email Service, Password Hasher, EF Core Configurations
- **API Layer:** Controllers, Request/Response Models, Middleware

### 6.2 Domain Model Struktur

#### 6.2.1 Entities & ValueObjects

**User Entity:**

- Properties: `UserId` (StronglyTypedId), `Email` (ValueObject), `PasswordHash` (ValueObject), `Role` (ValueObject), `EmailVerificationStatus` (Discriminated Union), Timestamps
- Navigation: `UserProfile`
- Factory Method: `Create()` gibt `Result<User>` zurück
- Business Methods: `VerifyEmail()`, `IsEmailVerified()`

**UserProfile Entity:**

- Properties: `UserProfileId`, `UserId` (FK), `FirstName` (ValueObject), `LastName` (ValueObject), Timestamps
- Navigation: `User`, `Address`
- Factory Method: `Create()` gibt `Result<UserProfile>` zurück
- Business Methods: `HasAddress()`

**Address Entity:**

- Properties: `AddressId`, `UserProfileId` (FK), `Street`, `City`, `PostalCode`, `Country` (alle als ValueObjects), Timestamps
- Null Object Pattern: `Address.Empty` für optionale Adresse, `IsEmpty` Property
- Navigation: `UserProfile`
- Factory Method: `Create()` gibt `Result<Address>` zurück

**ValueObjects:**

- `Email`: Mit RuleComposer für 47+ Validation Rules
- `PasswordHash`: Nur über Factory Method erstellbar
- `Name` (FirstName, LastName): Mit Validierung
- `UserRole`: Vordefinierte Werte (User, Admin)
- `VerificationToken`: Mit Token-Wert und Ablaufzeit
- Strongly-Typed IDs: `UserId`, `UserProfileId`, `AddressId`
- Address Components: `AddressLine`, `City`, `PostalCode`, `Country`

**EmailVerificationStatus (Discriminated Union):**

- `NotVerified(VerificationToken)`: Initial mit Token
- `Verified(DateTimeOffset)`: Nach erfolgreicher Verifizierung
- `Expired(VerificationToken)`: Bei abgelaufenem Token

### 6.3 Request/Response Pattern (CQRS)

**Eigene Implementierung - KEIN Dispatcher:**

- `IRequestHandler<TRequest, TResponse>` Interface für Handler
- Handler werden direkt vom DI Container in Controller injiziert
- `RegisterUserHandler` implementiert Business Logic
- `RegisterUserRequest` als Request DTO
- `RegisterUserResponse` mit HATEOAS Links
- Direkte Injection eliminiert Dispatcher-Overhead

**Vorteile:**

- Keine externe Library (MediatR) benötigt
- Klare Separation of Concerns
- Einfach testbar
- Type-safe durch generische Interfaces
- Explizite Handler-Registrierung im DI Container

### 6.4 Validation Rules Pattern (Composite Pattern)

**Atomic Rules:**

- Jede Rule implementiert `IValidationRule<T>` Interface
- Jede Rule prüft **genau eine** Bedingung
- Gibt `Result` zurück (Success/Failure mit Error)

**RuleComposer:**

- Komponiert mehrere atomic Rules
- Sammelt alle Fehler
- Gibt kombiniertes Result zurück

**Email Validation:**

- 47+ atomic Rules (eine pro Test-Case)
- Beispiele: `EmailNotEmptyRule`, `OnlyOneAtSymbolRule`, `LocalPartMaxLengthRule`, `NoTrailingDotRule`, etc.
- Rules sind wiederverwendbar und isoliert testbar

### 6.5 Result<T> Pattern (Railway-Oriented Programming)

**Eigene Implementierung:**

- `Result` für Operations ohne Rückgabewert
- `Result<T>` für Operations mit Rückgabewert
- Properties: `IsSuccess`, `IsFailure`, `Error`, `Value`
- Static Factory Methods: `Success()`, `Failure(error)`
- Eliminiert Exception-Handling für Business Logic Fehler

### 6.6 API Response Format

**RegisterUserResponse:**

- `userId`: GUID
- `email`: String
- `message`: Erfolgs-Nachricht
- `links`: Dictionary mit HATEOAS Links

**HATEOAS Links:**

- `self`: GET User Details
- `verify-email`: POST Email Verification
- `resend-verification`: POST Resend Verification Email
- `login`: POST Login Endpoint
- Dynamisch basierend auf User-Status

**HateoasLink Struktur:**

- `href`: URL zur Ressource
- `method`: HTTP Method (GET, POST, etc.)
- `description`: Optional, Beschreibung der Aktion

---

## 7. Technical Considerations

### 7.1 Dependencies

- **ASP.NET Core 9.0+**: Web API Framework
- **Entity Framework Core**: ORM für PostgreSQL
- **Npgsql.EntityFrameworkCore.PostgreSQL**: PostgreSQL Provider
- **Konscious.Security.Cryptography.Argon2**: Argon2 Password Hashing
- **IdentityServer** oder **Duende IdentityServer**: OAuth2/OpenID Connect

**VERBOTEN - Keine externen Dependencies für:**

- **FluentValidation** - Wird durch eigenes Rules Pattern mit RuleComposer ersetzt
- **MediatR** - Wird durch eigenes Request/Response Pattern mit DI Container ersetzt
- **CSharpFunctionalExtensions/LanguageExt** - Eigene Result<T> Implementierung

### 7.2 Email Service

- Email-Versand muss über abstraktes `IEmailService` Interface erfolgen
- Initiale Implementierung kann SMTP sein
- Muss asynchron erfolgen (z.B. über Background Service oder Message Queue)

### 7.3 Security Considerations

- **Password Hashing:** Argon2id mit empfohlenen Parametern (memory: 64MB, iterations: 3, parallelism: 4)
- **Token Generation:** Kryptographisch sichere Token-Generierung (256-bit minimum)
- **SQL Injection Prevention:** EF Core Parametrisierung nutzen
- **Email Enumeration:** Generische Fehlermeldungen bei Duplikaten
- **Rate Limiting:** Sollte auf Infrastructure-Level (API Gateway, Middleware) implementiert werden

### 7.4 Database Migrations

- EF Core Migrations für Schema-Erstellung
- Seed-Daten für Rollen (falls als separate Tabelle)
- **EF Core Value Converters:**
  - Value Converters für alle ValueObjects (Email, Name, etc.)
  - Converter für Null Object Pattern: `Address.Empty` ↔ NULL in Datenbank
  - Query Filter für Empty-Objekte (optional)

### 7.5 Testing Requirements

- **Unit Tests:** Für alle Validation Rules, Domain Logic, Application Layer
- **Integration Tests:** Für API Endpoints, Database Interactions
- **Email Validation Tests:** Alle bereitgestellten Test-Cases müssen durchlaufen
- **Security Tests:** Für Password Hashing, Token Generation

### 7.6 Logging & Monitoring

- Strukturiertes Logging (Serilog empfohlen)
- PII (Personally Identifiable Information) darf nicht geloggt werden
- Erfolgreiche Registrierungen loggen (ohne Details)
- Fehler mit Context loggen

---

## 8. Success Metrics

1. **Funktionalität:**
   - 100% der Email Validation Test-Cases bestanden
   - API-Endpoint funktioniert wie spezifiziert (201 Created bei Erfolg)
   - Email-Verifizierung funktioniert End-to-End

2. **Sicherheit:**
   - Passwörter werden ausschließlich mit Argon2 gehasht
   - Keine Klartext-Passwörter in Logs oder Datenbank
   - Kein Email-Enumeration möglich

3. **Performance:**
   - Registrierung dauert < 500ms (ohne Email-Versand)
   - Email-Versand erfolgt asynchron
   - Database Queries sind optimiert (max. 3 Queries für Registrierung)

4. **Code Quality:**
   - Unit Test Coverage > 80%
   - Alle Integration Tests bestanden
   - Code Review ohne kritische Findings

5. **User Experience:**
   - Klare, verständliche Fehlermeldungen (RFC 7807)
   - Erfolgreiche Registrierung mit klaren Next Steps

---

## 9. Open Questions

1. **Email-Template:** Welches Design/Template soll die Verifizierungs-Email haben?
2. **Email-Provider:** Welcher Email-Service soll verwendet werden? (SMTP, SendGrid, AWS SES, etc.)
3. **Token-Ablaufzeit:** Ist 24 Stunden für Email-Verifikation angemessen oder soll es konfigurierbar sein?
4. **Resend Verification:** Soll es einen Endpoint geben, um die Verifizierungs-Email erneut zu senden?
5. **Address Mandatory:** Ist die Adresse ein Pflichtfeld bei der Registrierung oder optional?
6. **OAuth2 Flows:** Welche OAuth2 Flows sollen konkret unterstützt werden? (Authorization Code, Resource Owner Password, Client Credentials?)
7. **API Documentation:** Soll OpenAPI/Swagger Documentation automatisch generiert werden?
8. **Localization:** Sollen Fehlermeldungen von Anfang an für Internationalisierung vorbereitet werden?
9. **Audit Trail:** Soll jede Registrierung in einem separaten Audit-Log gespeichert werden?
10. **Environment Configuration:** Wie unterscheiden sich die Konfigurationen zwischen Development, Staging und Production? (z.B. Email-Versand nur in Prod)

---

## 10. Implementation Notes for Junior Developers

### Getting Started

1. Beginne mit der Domain Layer:
   - **Erstelle `Result` und `Result<T>` Klassen (eigene Implementierung)**
   - Erstelle Strongly-Typed IDs (UserId, UserProfileId, AddressId)
   - **Erstelle `IValidationRule<T>` Interface**
   - **Erstelle `RuleComposer<T>` Klasse**
   - **Erstelle atomic Email Validation Rules (eine Rule pro Test-Case)**:
     - `EmailNotEmptyRule`
     - `OnlyOneAtSymbolRule`
     - `LocalPartMaxLengthRule`
     - `DomainPartMaxLengthRule`
     - `NoTrailingDotRule`
     - `NoNewlineCharactersRule`
     - `NoUnderscoreInDomainRule`
     - `ValidIpAddressRule`
     - ... (alle 47 Test-Cases als separate Rules)
   - Erstelle ValueObjects mit `RuleComposer` (`Email`, `Name`, etc.)
   - Erstelle `PasswordHash` ValueObject
   - Erstelle `UserRole` ValueObject
   - Erstelle `VerificationToken` ValueObject
   - Erstelle `EmailVerificationStatus` Discriminated Union
   - Erstelle Entities (User, UserProfile, Address) mit Factory Methods
2. **Erstelle Application Layer Request/Response Pattern (eigene Implementierung):**
   - `IRequestHandler<TRequest, TResponse>` Interface
   - `RegisterUserRequest` Record
   - `RegisterUserHandler` : `IRequestHandler<RegisterUserRequest, Result<RegisterUserResponse>>`
   - Handler wird direkt vom DI Container in Controller injiziert (kein Dispatcher!)
3. Erstelle die Interfaces (IUserRepository, IPasswordHasher, IEmailService)
4. Implementiere die Infrastructure Layer: Repository, Password Hasher, Email Service, EF Core Value Converters
5. Erstelle den API Controller mit injiziertem Handler
6. **DI Container Registration für alle Handler**
7. **Schreibe Unit Tests für jede einzelne Validation Rule (atomic tests!)**
8. Schreibe Unit Tests für RuleComposer
9. Schreibe Unit Tests für Result<T>
10. Schreibe Unit Tests für Request/Response Pattern
11. Schreibe Unit Tests für ValueObjects
12. Schreibe Integration Tests für den gesamten Flow

### Best Practices

- **Dependency Injection:** Nutze ASP.NET Core DI Container
- **Async/Await:** Alle I/O Operationen asynchron durchführen
- **Error Handling:** Nutze Global Exception Handler für RFC 7807 Format
- **Validation:** Validiere bereits im Controller (Model Validation) und zusätzlich im Application Layer
- **Separation of Concerns:** Jede Schicht hat klare Verantwortlichkeiten
- **Test First:** Schreibe Tests bevor du Code implementierst (TDD)
- **ValueObjects everywhere:** Niemals primitive Typen für Domain-Konzepte verwenden
- **No nulls:** "Make illegal states unrepresentable" - nutze Result<T>, Discriminated Unions
- **Factory Methods:** Entities nur über Factory Methods erstellen, die validieren
- **Immutability:** ValueObjects sind immutable (records), Entities haben private setters
- **EF Core Converters:** Nutze Value Converters für ValueObjects in der Datenbank
- **Atomic Rules:** Jede Validation Rule testet **genau eine** Bedingung
- **Rule Composition:** Nutze `RuleComposer<T>` um atomic Rules zu kombinieren
- **Single Responsibility:** Eine Rule-Klasse = eine Verantwortlichkeit = ein Test-Case
- **Testability:** Jede atomic Rule ist isoliert testbar
- **Null Object Pattern:** Nutze Empty-Objekte statt `null` für optionale Werte:
  - `Address.Empty` statt `Address? = null`
  - `address.IsEmpty` statt `address == null`
  - Empty-Objekte haben klare Semantik und Verhalten
- **Eigene Implementierungen:** Keine externen Dependencies für Validation, CQRS, Result<T>
  - Request/Response Pattern mit `IRequestHandler<TReq, TResp>` und DI Container (kein Dispatcher!)
  - Result<T> für Railway-Oriented Programming
  - Rules Pattern mit RuleComposer statt FluentValidation

### Common Pitfalls

- ❌ Passwörter im Klartext loggen
- ❌ Synchrone Email-Versand (blockiert Request)
- ❌ Email-Existenz durch unterschiedliche Fehlermeldungen offenlegen
- ❌ Validierung nur im Controller (muss auch im Application Layer sein)
- ❌ EF Core Tracking Issues (vergessen `AsNoTracking()` zu nutzen bei Read-Only Queries)
- ❌ Primitive Typen (string, int, Guid) für Domain-Konzepte verwenden
- ❌ Nullable Types verwenden statt explizite Zustände zu modellieren
- ❌ Entities mit `new` erstellen statt Factory Methods
- ❌ Vergessen, EF Core Value Converters für ValueObjects zu konfigurieren
- ❌ ValueObjects mutable machen (immer records oder readonly verwenden)
- ❌ **Eine große Validation-Klasse für alle Email-Regeln** (stattdessen atomic Rules!)
- ❌ Rules direkt in ValueObject-Logik schreiben statt separate Rule-Klassen
- ❌ RuleComposer nicht nutzen und Rules manuell iterieren
- ❌ Mehrere Bedingungen in einer Rule prüfen (nicht atomic!)
- ❌ Rules ohne entsprechende Unit-Tests
- ❌ `null` verwenden statt Null Object Pattern für optionale Werte
- ❌ `Address?` oder `if (address == null)` statt `Address.Empty` und `address.IsEmpty`
- ❌ Empty-Objekte ohne `IsEmpty` Property, sodass man sie nicht erkennen kann
- ❌ FluentValidation verwenden statt eigenes Rules Pattern
- ❌ MediatR verwenden statt eigener Request/Response Pattern
- ❌ CSharpFunctionalExtensions/LanguageExt verwenden statt eigener Result<T> Implementierung
- ❌ Dispatcher bauen - einfach Handler direkt in Controller injizieren!
- ❌ Vergessen, Handler im DI Container zu registrieren
- ❌ Request/Response Pattern nicht nutzen und Logic direkt in Controller schreiben

---

## 11. Appendix

### A. Email Validation Test Cases

Die vollständige Test-Suite aus Rust (ursprünglich von Django übernommen) ist im Projekt verfügbar und muss 1:1 in C# übersetzt und implementiert werden. Alle 47 Test-Cases müssen erfolgreich durchlaufen.

**Wichtig:** Für jeden der 47 Test-Cases muss eine entsprechende atomic `IValidationRule<string>` Klasse erstellt werden. Jede Rule testet genau einen Aspekt der Email-Validierung.

**Beispiel-Mapping:**

- Test: "email must have exactly one @" → Rule: `OnlyOneAtSymbolRule`
- Test: "local part max 64 chars" → Rule: `LocalPartMaxLengthRule`
- Test: "domain part max 255 chars" → Rule: `DomainPartMaxLengthRule`
- Test: "no trailing dot" → Rule: `NoTrailingDotRule`
- Test: "no newline characters" → Rule: `NoNewlineCharactersRule`
- Test: "no underscore in domain" → Rule: `NoUnderscoreInDomainRule`
- Test: "valid IP address format" → Rule: `ValidIpAddressRule`
- ... (40 weitere Rules für restliche Test-Cases)

### B. RFC References

- **RFC 6854:** Updated IMAP LIST Command (für Email-Verifizierung)
- **RFC 7807:** Problem Details for HTTP APIs
- **RFC 5321:** Simple Mail Transfer Protocol (Email Format)

### C. Design Patterns

**1. Null Object Pattern:** Verwendung von `Address.Empty` statt `null`

- Eliminiert Null-Checks im gesamten Code
- Klare Semantik: "keine Adresse" vs. "vergessen zu setzen"
- Empty-Objekte können Standardverhalten haben
- EF Core mappt Empty auf NULL in DB (transparent für Domain)

**2. Request/Response Pattern (CQRS):** Eigene Implementierung OHNE Dispatcher!

- `IRequestHandler<TRequest, TResponse>` für Handler
- Handler werden direkt vom DI Container in Controller injiziert
- KEIN `IRequestDispatcher` - unnötiger Overhead!
- Klare Trennung zwischen Request und Handler
- Testbar durch Dependency Injection
- Beispiel: `IRequestHandler<RegisterUserRequest, Result<RegisterUserResponse>>` wird direkt in `UsersController` injiziert

**3. Railway-Oriented Programming:** Eigene `Result<T>` Implementierung

- `Result.Success()` und `Result.Failure(error)` für Operations ohne Rückgabewert
- `Result<T>.Success(value)` und `Result<T>.Failure(error)` für Operations mit Rückgabewert
- Eliminiert Exception-Handling für Business Logic Fehler
- Explizites Error-Handling erzwungen durch `IsSuccess`/`IsFailure`

**4. Composite Pattern für Validation:**

- `IValidationRule<T>` Interface für atomare Regeln
- `RuleComposer<T>` komponiert mehrere Rules
- Jede Rule = eine Verantwortlichkeit = ein Test-Case
- Beispiel: 47+ atomare Rules für Email-Validierung

### D. Related PRDs

- **PRD-0002:** User Login Feature (folgt nach diesem PRD)
- **PRD-0003:** Password Reset Feature
- **PRD-0004:** User Profile Management

### E. Coding Guidelines & Safety Rules

Diese Richtlinien sind für **alle** Implementierungen im VehicleRegistry Projekt bindend und müssen strikt eingehalten werden.

#### E.1 Code Style & Organization

**File & Namespace Organization:**

- File-scoped namespaces, die der Ordnerstruktur entsprechen
- Ein Typ pro Datei
- Klassen über 100 Zeilen müssen mit partial classes refaktoriert werden
- Verwandte Dateien logisch in Ordnern gruppieren

**Code Style:**

- Pattern Matching und Switch Expressions wo möglich
- LINQ für Datenmanipulation (Lesbarkeit > Cleverness)
- Expression-bodied Members für einfache, einzeilige Methoden/Properties
- `var` nur wenn Typ offensichtlich ist, sonst explizite Typen
- Variablen im kleinstmöglichen Scope deklarieren
- Max. 70 Zeilen pro Funktion (Hard Limit!)

#### E.2 Record Design (für ValueObjects & Entities)

**Grundregeln:**

- Properties in Primary Constructor Syntax (inline)
- Jedes Record `<Name>` bekommt `<Name>Factory` Static Factory Class
- Factory und Record in **derselben Datei**
- Factory muss `Create()` Method exponieren
- **Argument-Validierung NUR in Factory.Create()**
- Immutable Collections: `ImmutableList<T>` bevorzugen
- Behavior via Extension Methods in separaten Static Classes

**Beispiel-Struktur:**

```csharp
Email.cs:
  - record Email(string Value)
  - static class EmailFactory
    - static Result<Email> Create(string email)
```

#### E.3 Discriminated Unions Design

**Struktur-Regeln:**

- Base: abstract record
- Variants: records, die von Base erben
- **NICHT nested** in Base Type
- Gesamte Union (Base + Variants) in **einer Datei**
- Eine Factory Class für gesamte Union
- Factory exponiert je eine Method pro Variant

**Beispiel:**

```csharp
EmailVerificationStatus.cs:
  - abstract record EmailVerificationStatus
  - sealed record NotVerified(...) : EmailVerificationStatus
  - sealed record Verified(...) : EmailVerificationStatus
  - sealed record Expired(...) : EmailVerificationStatus
  - static class EmailVerificationStatusFactory
```

#### E.4 Safety-Critical Rules (NASA Power of Ten inspiriert)

**Control Flow:**

- Explizite, einfache Control Flow (keine Rekursion!)
- Alle Loops/Queues müssen **bounded** sein (fixed upper bound)
- Event Loops müssen assertion für Nicht-Terminierung haben
- Zentrale Control Flow in Parent-Function, Logic in Helper-Functions

**Assertions (Critical!):**

- **Minimum 2 Assertions pro Function** (Assertion Density)
- Assert alle Function Arguments & Return Values
- Assert Pre/Postconditions & Invariants
- **Paired Assertions:** Property an 2+ Code-Pfaden prüfen
- **Positive UND Negative Space** assertieren (Boundary Testing!)
- Split Compound Assertions: `assert(a); assert(b);` statt `assert(a && b);`
- Implications mit single-line if: `if (a) assert(b);`
- Compile-Time Assertions für Constants & Type Sizes

**Assertions als Dokumentation:**

```csharp
// ✅ RICHTIG: Assertion dokumentiert kritische Invariante
if (index < length) { 
    assert(index >= 0);  // Positive space
    // ... 
} else {
    assert(index >= length);  // Negative space
}

// ❌ FALSCH: Compound Assertion
assert(index >= 0 && index < length);
```

**Wichtig:** Assertions sind Safety Net, **KEIN Ersatz für Verständnis**!

1. Mentales Modell des Codes aufbauen
2. Verständnis als Assertions encodieren
3. Code & Comments zur Rechtfertigung schreiben
4. Tests als Final Defense nutzen

#### E.5 Memory & Resource Management

- **Statische Allocation:** Gesamter Memory bei Startup allokieren
- **Keine dynamische Allocation** nach Initialization
- Vermeidet Unpredictable Behavior & Use-After-Free
- Nutze `using` Statements für `IDisposable` Objects
- `try-finally` für Resource Cleanup

#### E.6 Type Safety

- Explizit-sized Types: `int`, `long`, `byte` etc.
- **Keine architecture-specific Types** wie `nint`/`nuint` ohne Begründung
- Strongly-Typed IDs statt `Guid`/`int`
- ValueObjects statt primitive Strings

#### E.7 Error Handling

- **Alle Errors MÜSSEN behandelt werden**
  - 92% aller catastrophic failures kommen von unbehandelten Non-Fatal Errors!
- Exceptions nur für exceptional situations
- Catch spezifische Exceptions (nicht `System.Exception`)
- Custom Exceptions von `System.Exception` ableiten
- **Result<T> Pattern für Business Logic Errors** (keine Exceptions!)

#### E.8 Asynchronous Programming

- `async`/`await` für I/O-bound Operations
- **KEIN `async void`** außer Event Handlers (nutze `async Task`)
- `ConfigureAwait(false)` in Library Code
- Alle async Operations müssen awaited werden

#### E.9 LINQ Guidelines

- LINQ für Lesbarkeit bevorzugen
- Deferred Execution beachten (Vorsicht bei Multiple Enumeration!)
- `.ToList()`/`.ToArray()` zum Materialisieren wenn nötig
- Lesbarkeit > Performance (außer in Hot Paths)

#### E.10 Dependency Injection

- Constructor Injection für Pflicht-Dependencies
- Property Injection nur für optionale Dependencies
- Abhängigkeiten auf **Abstractions** (Interfaces/Abstract Classes)
- Niemals auf Concrete Implementations

#### E.11 Testing (xUnit v3)

- **KEINE** 'Arrange', 'Act', 'Assert' Comments
- Naming Convention aus bestehenden Test-Files übernehmen
- Tests müssen **independent & repeatable** sein
- Public API testen (nicht Internals)
- Balance: Unit/Integration/E2E Tests

#### E.12 Security (Critical!)

- **SQL Injection Prevention:** Parametrisierte Queries (EF Core!)
- **XSS Prevention:** Input Sanitization
- **Sensitive Data:** Niemals loggen oder exposen
- **Email Enumeration:** Generische Error Messages
- **Password Hashing:** Argon2id, niemals Plain-Text
- **Token Generation:** Kryptographisch sicher (256-bit min)

#### E.13 Explicit > Implicit

- **Always motivate, always say why!**
- Explizite Options an Library Functions übergeben
- Vermeidet latente Bugs wenn Defaults sich ändern
- Rationale für Decisions teilen (erhöht Compliance & Verständnis)

#### E.14 Code Quality Principles

**Positive State Invariants:**

```csharp
// ✅ RICHTIG: Positive Bedingung
if (index < length) {
    // Invariant holds
} else {
    // Invariant doesn't hold
}

// ❌ FALSCH: Negation (schwerer zu verstehen)
if (index >= length) {
    // It's not true that invariant holds
}
```

**Function Design:**

- "Hourglass-Inversion": Wenige Parameter, simple Return Type, viel Logic
- Control Flow zentralisieren: if/switch in Parent, Logic in Helpers
- "Push ifs up, fors down"
- State Manipulation zentralisieren: Parent hält State, Helpers berechnen Changes
- Leaf Functions pure halten

**Compound Conditions vermeiden - Nutze Pattern Matching:**

```csharp
// ❌ FALSCH: Compound Condition
if (a && b || c) { }

// ✅ RICHTIG: Pattern Matching & Switch Expression
var result = (a, b, c) switch
{
    (true, true, _) => HandleAAndB(),      // Case: a && b
    (true, false, _) => HandleAOnly(),     // Case: a && !b
    (false, _, true) => HandleCOnly(),     // Case: !a && c
    _ => HandleDefault()                    // All other cases
};

// Alternative mit when Guards (wenn komplexere Logic nötig):
var result = state switch
{
    var s when s.A && s.B => HandleAAndB(),
    var s when s.A => HandleAOnly(),
    var s when s.C => HandleCOnly(),
    _ => HandleDefault()
};

// ❌ VERMEIDEN: Nested if-statements
// Nutze stattdessen Switch Expressions und Pattern Matching
```

#### E.15 Compiler & Warnings

- **Alle Compiler Warnings** bei strictest Setting behandeln
- Warnings sind Bugs in Wartestellung
- Zero-Warning Policy

#### E.16 Control Flow & External Interaction

- Programm läuft in eigenem Pace (nicht reaktiv auf externe Events)
- Batching bevorzugen (statt Context Switch bei jedem Event)
- Control Flow bleibt unter eigener Kontrolle
- Verbessert Safety UND Performance

---

**Anmerkung:** Diese Guidelines sind nicht optional. Sie basieren auf Jahrzehnten von Erfahrung in Safety-Critical Systems (NASA, Aviation, Medical Devices) und haben sich in der Praxis bewährt, um catastrophic failures zu vermeiden.

*"The rules act like the seat-belt in your car: initially they are perhaps a little uncomfortable, but after a while their use becomes second-nature and not using them becomes unimaginable."* — Gerard J. Holzmann

---
