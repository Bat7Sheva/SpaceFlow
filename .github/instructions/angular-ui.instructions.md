---
description: "Use when creating or modifying Angular components, services, modules, or templates. Covers component structure, form patterns, API communication for this CRM project."
applyTo: "**/*.ts, **/*.html"
---
# Angular UI Guidelines

## Component Structure
- Use standalone components
- Reactive Forms only (no template-driven)
- Inject services via `inject()` function

## API Communication
- ONE central `ApiService` that wraps HttpClient
- Each feature has its own service (e.g. `LeadService`) that calls `ApiService`
- Always handle loading state and error state in components

## UI Rules
- Use Angular Material components only - no custom CSS framework
- Every list screen has: search input, status filter, loading spinner, empty state
- Every form has: validation messages, save button with loading state, cancel button

## Navigation
- Use Angular Router with lazy loading per feature module
- Route structure: `/leads`, `/leads/:id`, `/today`
