import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def load_json(path: Path):
    with path.open("r", encoding="utf-8") as f:
        return json.load(f)


def test_parts_have_required_fields():
    parts = load_json(ROOT / "Data/Samples/parts.json")
    assert isinstance(parts, list) and parts, "parts.json must be a non-empty array"
    for p in parts:
        for key in ("id", "name", "slot", "tier", "stats"):
            assert key in p, f"Missing '{key}' in part: {p}"


def test_enemy_loadout_is_complete():
    enemies = load_json(ROOT / "Data/Samples/enemies.json")
    assert isinstance(enemies, list) and enemies, "enemies.json must be a non-empty array"
    for e in enemies:
        loadout = e.get("loadout", {})
        for key in ("CPU", "Power", "Move", "ArmL", "ArmR", "Modules"):
            assert key in loadout, f"Enemy loadout missing '{key}': {e}"


def test_item_refs_point_to_existing_parts():
    part_ids = {p["id"] for p in load_json(ROOT / "Data/Samples/parts.json")}
    items = load_json(ROOT / "Data/Samples/items.json")
    for item in items:
        if item.get("type") == "Part":
            assert item.get("refId") in part_ids, f"Unknown part refId: {item}"
