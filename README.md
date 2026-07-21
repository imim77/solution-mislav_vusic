# Proizvodi — Abysalto akademija

Full-stack aplikacija za pregled proizvoda, pretragu, filtriranje i spremanje favorita. Podaci o proizvodima dolaze iz [DummyJSON](https://dummyjson.com) API-ja; korisnici i favoriti pohranjuju se lokalno u SQLite bazi.

## Preduvjeti

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker](https://www.docker.com/) (opcionalno)

## Pokretanje

### Docker Compose (preporučeno)

```bash
docker compose up --build
```

- Frontend: http://localhost:8080
- Backend: http://localhost:5188

### Lokalno

**Backend**

```bash
cd backend
dotnet restore
dotnet run --project src/Proizvodi.Api
```

**Frontend** (u drugom terminalu)

```bash
cd frontend
npm install
npm run dev
```

Frontend dev server proxy-ira `/api` pozive na backend (`http://localhost:5188`).

### Prijava

| Polje    | Vrijednost   |
| -------- | ------------ |
| Username | `emilys`     |
| Password | `emilyspass` |

## Struktura projekta

```
backend/
  src/Proizvodi.Api/     ASP.NET Core 10 Minimal API
  tests/                 unit i integration testovi
frontend/
  src/                   React 19 + TypeScript
```

### Backend

- **Vertical slice arhitektura** — endpointi i logika organizirani po featureima (`Features/Proizvodi`, `Features/Categories`)
- **DummyJSON integracija** — proizvodi, kategorije i autentifikacija preko vanjskog API-ja
- **SQLite + EF Core** — lokalna baza za korisnike i favorite; migracije se primjenjuju pri pokretanju
- **Global exception handler** — konzistentni `ProblemDetails` odgovori

### Frontend

- **React 19, Vite, Tailwind CSS 4** — s React Compilerom
- **TanStack Query** — dohvat i cache podataka
- **React Router** — zaštićene rute za favorite (`/favorites`)
- Stranice: lista proizvoda (pretraga, kategorija, cijena, paginacija), detalj proizvoda, favoriti, login

## Testovi

```bash
# Backend
cd backend && dotnet test

# Frontend
cd frontend && npm run test
```

## AI alati

Tijekom razvoja korišten je GitHub Copilot (GPT-5.5) za autocomplete, testne slučajeve i refaktoriranje. Na relevantnim mjestima u kodu ostavljeni su komentari s modelom i promptom.
