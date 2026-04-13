---
description: "Build a new CRM feature end-to-end. Use when you want to scaffold a complete feature from database to UI."
---
Build the following CRM feature end-to-end:

Feature: ${input:featureName:e.g. Lead Management}
Entities: ${input:entities:e.g. Lead, Interaction}
Key fields: ${input:fields:e.g. FullName, Phone, Status, NextFollowUpAt}
Scope includes: ${input:includes:e.g. list screen, detail screen, CRUD API}
Scope excludes: ${input:excludes:e.g. file uploads, email integration}

Follow the mvp-builder agent sequence: DB → API → UI.
Stop and fix build errors at each step before continuing.
---
