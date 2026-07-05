# Abysalto akademija - zadatak

## Postavljanje okruženja i instalacija

### Preduvjeti

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker](https://www.docker.com/) (opcionalno)

### Backend

```bash
cd backend
dotnet restore
```

### Frontend

```bash
cd frontend
npm install
```

## Konfiguracija


### Frontend

Frontend koristi Vite proxy za preusmjeravanje `/api` poziva na backend (`http://localhost:5188`). Konfiguracija se nalazi u `frontend/vite.config.ts`.

## Pokretanje


### Docker Compose

```bash
docker compose up --build
```

- Backend: `http://localhost:5188`
- Frontend: `http://localhost:8080`

### Login credentials

Za username koristiti `emilys`, a za password `emilyspass`

## Testovi

### Backend

```bash
cd backend
dotnet test
```

### Frontend

```bash
cd frontend
npm test
```

### Korištenje AI alata

Tijekom razvoja aplikacije koristio sam GitHub Copilot s GPT-5.5 modelom putem AI automatskog dovršavanja koda (AI autocomplete). Alat mi je služio za bržu izradu boilerplate koda, osmišljavanje dodatnih testnih slučajeva te pomoć pri refaktoriranju određenih dijelova koda. AI je također korišten tijekom razvoja frontend dijela aplikacije. Na mjestima gdje je korišten AI ostavljeni su komentari u izvornom kodu koji navode korišteni model i odgovarajući prompt