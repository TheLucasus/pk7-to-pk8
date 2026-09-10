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

The current PKHeX.Core conversion route converts PK7 to PK8 without requiring
a Sword/Shield save. A destination save is needed later if the Pokemon is
inserted into a save and needs that save's trainer or handler context; it is
not needed for producing the standalone PK8 file.