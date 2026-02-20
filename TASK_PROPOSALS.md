# Task Proposals from Codebase Review

## 1) Typo Fix Task — Align namespace/project naming (`OdiGame` vs `BotGame`)

**Issue found:** The Godot project and assembly are named `BotGame`, but most C# namespaces are `OdiGame`, which looks like a leftover naming typo/inconsistency.

**Task:** Rename `OdiGame.*` namespaces to `BotGame.*` (or update project naming to match one canonical name), then update all using/import references.

**Why this helps:** Reduces confusion for contributors, avoids mixed naming in stack traces and generated docs, and prevents accidental creation of duplicate namespaces.

**Acceptance criteria:**
- No `OdiGame` namespace references remain unless explicitly intentional and documented.
- Project builds and scenes run with updated script bindings.

## 2) Bug Fix Task — Prevent out-of-bounds wall placement on small maps

**Issue found:** `TurnController` hard-codes wall placement at `y = 4` during `_Ready`. If `MapHeight <= 4`, this can throw `ArgumentException` via `GridWorld.SetBlocked` because the position is outside the grid.

**Task:** Guard or compute the wall row dynamically (for example `Mathf.Clamp(MapHeight / 2, 0, MapHeight - 1)`), and skip generation when dimensions are too small.

**Why this helps:** Prevents startup crashes when exported `MapHeight` is edited to small values in the inspector.

**Acceptance criteria:**
- No exception when `MapHeight` is 1–4.
- Scene still initializes and actor rendering works on small map dimensions.

## 3) Comment/Documentation Discrepancy Task — Make wall-generation comment match real behavior

**Issue found:** Comment says `Simple wall strip with one gap`, but for narrow maps (`MapWidth < 5`) the loop never runs, so there is no strip and no gap.

**Task:** Update comment to describe conditional behavior, or update logic so one gap wall strip is always generated when possible.

**Why this helps:** Keeps maintenance docs/comments accurate and prevents wrong assumptions during gameplay tuning.

**Acceptance criteria:**
- Inline comment reflects all map-size cases **or** generation logic guarantees the documented shape.

## 4) Test Improvement Task — Add unit tests for grid bounds and small-map startup safety

**Issue found:** There are currently no automated tests validating grid bounds behavior or startup behavior with small exported map sizes.

**Task:** Add tests covering:
- `GridWorld.IsBlocked` for inside/outside cells.
- `GridWorld.SetBlocked` throws on outside positions.
- Turn setup behavior for small map dimensions (regression coverage for the wall-row bug).

**Why this helps:** Locks in expected behavior and prevents regressions when map generation logic changes.

**Acceptance criteria:**
- Tests execute in CI/local with deterministic pass/fail.
- New tests fail before bug fix and pass after bug fix.
