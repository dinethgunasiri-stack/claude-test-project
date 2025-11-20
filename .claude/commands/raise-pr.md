---
description: Creating a PR
---

## Task

- Create a PR for the changes ditected.

## Instructions

**Check repository status**
   - Show me the current `git status`.
   - Confirm which branch I am on.

**Update from the remote**
   - Fetch and pull the latest changes from the default branch (`main` or `master`).
   - If the current branch is not up to date, merge or rebase from `main`/`master`.
   - **If there are any merge conflicts**, stop and:
     - Show me the conflicting files and hunks.
     - Ask me which changes to keep before resolving the conflicts.

**Clean up the code**
   - Scan the changed files for any `debugger` statements or obvious temporary debug code (e.g., unnecessary `console.log` / `print` used only for debugging).
   - Remove them unless they are intentionally required.

**Run checks**
   - If the project has tests, linters, or a build step (e.g., `npm test`, `dotnet test`, `npm run build`), run the appropriate commands.
   - If any command fails, show me the errors before continuing.

**Prepare the commit**
   - Show me the diff of all changes that will be committed.
   - Stage only the relevant files for this task.
   - Create a commit with a **single-line message** that clearly describes the changes (in the imperative form, e.g., `Fix X`, `Add Y`, `Refactor Z`).

**Push the branch**
   - Push the current branch to the remote.
   - If this is a new branch, set the upstream tracking branch.

**Create the PR**
   - Create a PR targeting `main`/`master`.
   - Use a clear title based on the commit message.
   - In the PR description, include:
     - A short summary of the changes.
     - Any relevant context or linked task/ticket ID.
     - A brief note on tests run and their status.


**Confirmation**
   - After creating the PR, show me:
     - The branch name.
     - The PR title and description.
     - The PR link (if available).