# PK7 to PK8 Converter

Small command-line converter for Gen 7 `.pk7` files to Gen 8 `.pk8` files.
It uses the current `PKHeX.Core` `EntityConverter` API; it does not reimplement
the Pokemon formats.

## Requirements

- .NET 10 SDK

## Usage

Create an `input/` folder beside the project and put `.pk7` files in it:

```text
input/
	Phoenix.pk7
	Arcanine.pk7
```

Run the converter from the project directory:

```bash
dotnet run --project Pk7ToPk8.csproj
```

Converted files are written to `output/` with the same base names:

```text
output/
	Phoenix.pk8
	Arcanine.pk8
```

Custom folders can be supplied as the first two arguments:

```bash
dotnet run --project Pk7ToPk8.csproj -- path/to/input path/to/output
```

The original files are never modified. Each file reports success or failure,
and processing continues after an individual failure. The process exits with
code `1` if at least one file fails.

## Destination save context

The converter uses the same `EntityConverter.TryMakePKMCompatible` path that
PKHeX uses when a PK7 is loaded into a PK8/Sword & Shield editor. It creates a
format-correct PK8 without inventing a HOME tracker. The tracker remains zero
unless the source already contains a tracker; Pokémon HOME assigns it during
an actual HOME transfer. A destination Sword/Shield save is not required for
this local conversion, though it is required later for save-specific handler
adaptation.