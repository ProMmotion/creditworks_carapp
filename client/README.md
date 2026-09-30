# CreditWorks CarApp — Client

The frontend for CreditWorks CarApp, a vehicle inventory management application. It is a Vue 3 and TypeScript single-page app built with Vite. The current application provides one route (`/`) for viewing and managing a car inventory and its weight-based categories.

## Current application state

### Implemented

- Paginated car inventory with selectable page sizes of 10, 25, or 50.
- Inventory display for owner, brand, model, year, weight, and matching categories.
- Car creation with brand, model, and owner selectors plus year and weight fields.
- Category listing with icon and weight-range display.
- Category creation, editing, and deletion through the category management UI.
- API-backed data access for cars, brands, models, owners, ownerships, and categories.
- Pinia stores for app data and Vue Router for route handling.
- Vitest component and utility tests (see [Testing](#testing)).

### Current limitations and known issues

- The client requires a compatible backend; it does not include a backend service or local mock data.
- The router currently defines only the home route (`/`).
- The car form currently creates cars; it does not implement car editing or deletion.
- Sorting controls are present in the table component, but the inventory view does not currently handle sort events.
- `getProperty` resolves a top-level property or one nested level. Deeper paths return the remaining nested object rather than resolving the final segment.
- Category creation currently emits its `submitted` event twice. This behavior is covered by a test and should be corrected when the form flow is next changed.
- The project-wide type check currently reports TypeScript errors in existing components. Run `npm run type-check` to see the current diagnostics; a successful test run does not imply a clean type check.

## Requirements

- Node.js `^22.18.0` or `>=24.12.0` (see `package.json` `engines`).
- npm.
- A compatible backend API. The client reads the API base URL from `VUE_APP_API_URL`.

## Getting started

From this directory, install dependencies:

```sh
npm install
```

Create a local `.env` file with your backend URL (do not commit environment-specific values):

```dotenv
VUE_APP_API_URL=http://localhost:YOUR_API_PORT
```

Replace the example with the backend's base URL. Vite is configured to expose variables prefixed with `VUE_APP_`; API requests use this value as the root for resource paths.

Start the development server:

```sh
npm run dev
```

Vite prints the local URL in the terminal. The application is available at `/`.

## Available scripts

| Command                      | Description                                                                                                                      |
| ---------------------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| `npm run dev`                | Start the Vite development server with hot reload.                                                                               |
| `npm run build`              | Run the Vue/TypeScript type check and create a production build. The command currently fails if the existing type errors remain. |
| `npm run build-only`         | Create a production build without running the type check.                                                                        |
| `npm run preview`            | Preview the production build locally.                                                                                            |
| `npm run type-check`         | Run `vue-tsc --build`.                                                                                                           |
| `npm run test:unit`          | Run unit tests in Vitest watch mode.                                                                                             |
| `npm run test:unit -- --run` | Run unit tests once (CI-friendly).                                                                                               |
| `npm run lint`               | Run Oxlint and ESLint. The configured lint commands use `--fix`.                                                                 |
| `npm run format`             | Format files under `src/` with Prettier.                                                                                         |

## Testing

Tests live alongside implementation files as `*.spec.ts`. Vitest runs in the `jsdom` environment and Vue component tests use Vue Test Utils. Current coverage includes:

- Utility behavior: `isUnique`, `getProperty`, and `isInRange`.
- Shared UI components: buttons, text/number input, list table, pagination, and modal behavior.
- Category creation/editing and category management flows, with backend/store boundaries mocked.
- Home view rendering.

Run the suite once with:

```sh
npm run test:unit -- --run
```

## Backend API

The API base URL is read from `VUE_APP_API_URL` in `src/requests/api.ts`. Requests use Axios and these resource paths:

| Resource      | Client operations                                                          |
| ------------- | -------------------------------------------------------------------------- |
| `/cars`       | List with pagination and optional filters/sorting; create and update cars. |
| `/brands`     | List and create brands.                                                    |
| `/models`     | List models by brand and create models.                                    |
| `/owners`     | List and create owners.                                                    |
| `/ownerships` | Retrieve ownership records for car IDs and create an ownership record.     |
| `/categories` | List, create, update, and delete categories.                               |

The expected request and response types are represented in `src/models/`, `src/requests/`, and `src/utils/Paginated.ts`. Backend implementation and deployment are outside this client repository.

## Project structure

```text
src/
├── assets/      Global styles, logos, and icon assets
├── components/  Reusable controls and car/category management interfaces
├── models/      TypeScript data models for API resources
├── requests/    Axios API request functions
├── router/      Vue Router configuration
├── scripts/     Shared helper functions and tests
├── stores/      Pinia stores for API-backed application state
├── utils/       Pagination and range utilities
└── views/       Route-level views and tests
```

`@` is configured as an alias for `src/`. Vue single-file components use TypeScript with `<script setup lang="ts">`; SCSS is used where needed. Build output, installed dependencies, and test coverage files are generated artifacts ignored by Git.
