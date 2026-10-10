# Simple Event Planner (ASP.NET Core MVC)

A small event-planning web app built for **Software Design and Development - Assignment 01: Plan It, Then Build It**.
Organizers create events, anyone can see what is coming up, and guests RSVP so the organizer knows who is attending.

**Stack:** C#, ASP.NET Core MVC (.NET 10), xUnit(for unit tests). Data is kept in memory for this assignment.

## Team

| Name            | GitHub username | Role                             |
|-----------------|-----------------|----------------------------------|
| Ifeanyi Ikefuna | @stormlex4all   | FullStack Engineer and Team lead |
| John Jimenez    | @johns-coding   | FullStack Engineer               |

## Requirements

|  ID  |      Requirement     |  Priority   |         Status        |                 Code            |
|------|----------------------|-------------|-----------------------|---------------------------------|
| FR01 | Create Event         | Must Have   |       Implemented     | `EventsController.Create`       |
| FR02 | View Upcoming Events | Must Have   |     Implemented       | `EventsController.Index`        |
| FR03 | Guest RSVP           | Should Have |     Implemented       | `RsvpController`, `RsvpService` |
| FR04 | Edit or Cancel Event | Should Have | Planned (next sprint) | - |

Full requirement cards and acceptance criteria are in the Part 1 planning report.

## Getting started

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

## Project structure

```
/EventPlanner/
  Controllers/   EventsController (FR01, FR02), RsvpController (FR03), HomeController
  Services/      RsvpService (FR03)
  Models/        Event, Rsvp, RsvpStatus
  Views/         Razor views
  Program.cs     service registration, /health endpoint (to be added later)
tests/EventPlanner.Tests/   xUnit tests for each requirement (to be added later)
```

## Environment configuration

Two files will be committed to git with placeholder values only:

- `/EventPlanner/appsettings.Development.json`
- `/EventPlanner/appsettings.Production.json`


## Branching and merging workflow

- `main` always contains working, tested code. Nobody commits directly to `main` after the initial scaffold.
- Each requirement is built on its own feature branch named `feature/<short-name>`:
  - `feature/create-event` (FR01) and `feature/list-events` (FR02) by John Jimenez
  - `feature/guest-rsvp` (FR03) by Ifeanyi Ikefuna
- Commits are small and say what changed, for example `Validate title and location when creating an event`.
- When a branch is ready, the author opens a pull request, the partner reviews it, and it is merged into `develop` first then later after tests is pushed into `main` branch.
- Before starting new work, each partner runs `git pull origin main`.

## Merge conflict

> I, Ifeanyi made a commit to the Events/Index.cshtml file, merged into `develop` branch. My colleague made an update too after to the same file
and when he tried to create a pull request, we experienced a merge conflict as he forgot to pull first and fetch my recent updates.

**What caused it:** `feature/create-event-view-events` and `feature/create-event-view-events-conflict` both added a table row to the event list in `Views/Events/Index.cshtml`, directly under the same placeholder line. After `feature/create-event-view-events-conflict` was merged, merging `feature/create-event-view-events` stopped with a conflict.

**How we resolved it:** We opened the file on my colleagues PC.
* Pulled and updated the develop branch.
* Merged the update into his `feature/create-event-view-events` branch.
* Fixed the conflict on his local branch by select the desired changes and committing the changes to git.
* Pushed the fix update and merged the pull request

## Deployment strategy: Blue-Green

The production server will run two identical copies of the app, **blue** (live) and **green** (idle). `DeploySlot` in `appsettings.Production.json` will show which copy a server is running, and the page footer will display it.

1. A merge into `main` is built and deployed to the idle environment (for example green) while users stay on blue.
2. We verify green: unit tests pass in the pipeline, `GET /health` (to be added later) should return 200 with `"slot":"green"`, and a smoke test creates an event, lists it, and records an RSVP.
3. The router is switched so all traffic goes to green. Blue stays running and untouched.
4. **Rollback:** if errors appear, the router is switched back to blue immediately, and the faulty commit is reverted on `main` with `git revert`.

Blue is kept for at least 24 hours after a release before it becomes the idle environment for the next one.
