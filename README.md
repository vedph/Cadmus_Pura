# Cadmus PURA Parts

Components for the [PURA project in Cadmus](https://github.com/vedph/cadmus_pura_doc).

## History

- 2021-11-11: upgraded to NET 6.
- 2021-11-09: added graph resources to parts test and refactored tests to use them.
- 2021-10-24: added essential SQL seed code for graph.
- 2021-10-23: refactored word form's pins to be graph-compliant.
- 2021-10-16: updated removing dependency from Itinera (`MsSignaturesPart` is now in TGR).

## Setting up Graph

Preload nodes, mappings, and a selection of thesauri:

```ps1
.\cadmus-tool graph-add C:\Users\dfusi\Desktop\PresetNodes.json cadmus-pura .\plugins\Cadmus.Cli.Plugin.Pura\seed-profile.json repository-factory-provider.pura

.\cadmus-tool graph-add C:\Users\dfusi\Desktop\PresetMappings.json cadmus-pura .\plugins\Cadmus.Cli.Plugin.Pura\seed-profile.json repository-factory-provider.pura -t M

.\cadmus-tool graph-add C:\Users\dfusi\Desktop\PresetThesauri.json cadmus-pura .\plugins\Cadmus.Cli.Plugin.Pura\seed-profile.json repository-factory-provider.pura -t T -p x:categories/
```

Usually, this should be done only once.

You can find these files under `Cadmus.Pura.Parts.Test/Assets`. Note that currently the `x:` URIs are just for the sake of testing.
