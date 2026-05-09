# AI Agent Guidelines (AGENTS.md)

Welcome to this repository! This file provides essential guidelines and context for any AI Coding Agent (e.g., Claude, Cursor, Copilot, Gemini) interacting with this codebase.

## Repository Overview
- **Structure:** See the `.ai/` directory for our central AI configurations, including MCP server settings and context files.
- **Language/Framework:** F# (F# Shared Core, Giraffe Backend, Fable/Vite Web (MVU), and Fabulous Mobile (MVU))
- **Architecture:** Vertical Slice Architecture (VSA) organized functionally by features.
- **Build System:** .NET (`TodoApp.slnx`)

## General Rules
1. **Explore First:** Always read the `README.md` and check relevant files in `.ai/context/` before starting significant work.
2. **Read-Only Verification:** Always verify changes after writing to files by reading them back or running `git status`/`git diff`.
3. **No Artifact Editing:** Never edit build artifacts directly (e.g., in `/dist` or `/build`). Always modify the source files.
4. **Environment:** If you encounter build errors, diagnose the logs and dependencies first before attempting arbitrary environment changes.
5. **Memory / Guidelines:** We follow TDD and domain modeling rules found in `.ai/skills/`.

## Testing & Validation
- **Run Tests:** Before proposing any changes or commits, run the test suite using `dotnet test TodoApp.slnx`.
- **Build:** Verify builds using `dotnet build TodoApp.slnx`.

## Model Context Protocol (MCP)
This repository leverages MCP for tooling. Check `.ai/agents/<agent_name>/` for your specific configurations to connect to local MCP servers.

## Pre-Commit Checklist
- [ ] Code has been tested and passes existing test suites.
- [ ] No secrets or API keys are hardcoded.
- [ ] Code is documented appropriately.
- [ ] Changes align with the architecture guidelines in `.ai/context/architecture.md` (if applicable).
