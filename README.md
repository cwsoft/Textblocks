# Textblocks (WinForms, C#10)
Textblocks ist eine **Textbausteinverwaltung für MS-Word**. Textbausteine werden in MS-Word erstellt, formatiert, kategorisiert und in Katalogdateien (.docx) verwaltet. Die erstellten Katalogdateien (.docx) können von Textblocks geladen, angezeigt und durchsucht werden. Der in der Vorschau angezeigte Textbaustein steht im Originallayout in der Zwischenablage zur Verfügung und kann über **(STRG+V)** in andere Word-Dokumente eingefügt werden.

![Screenshot](./Dokumentation/screenshots/screenshot.png)

Textblocks spielt seine Stärke vor allem da aus, wo große Word-Dokumente nach Textbausteinen durchsucht werden, um diese anschließend manuell ins Zieldokument zu kopieren. Weitere Informationen enthält die [Bedienungsanleitung](./Dokumentation/Textblocks.pdf).

## Mindestanforderungen
- Windows 10 Betriebssystem
- Microsoft Word 2010-2021 (Office-365 nicht unterstützt)
- Katalogdatei (.docx) mit Textbausteinen
- **Für Selbst-Kompilierer:**
  - [Office PIA](./Textblocks/PIA/Howto-Office-PIA.md) `Microsoft.Office.Interop.Word.dll`
  - MS Visual Studio oder kompatiblen C#-Compiler
  - Kompilierbar für NET Framework 4.8 bis Net 7
  - `<TargetFramework>net48|net7.0-windows</TargetFramework>` 
  - Textblocks nutzt abwärtskompatible C#10 Sprachfeatures

## Aufbau der Katalogdateien
Informationen zum Aufbau der Katalogdateien .docx sind im Beispielkatalog [Textblocks_Katalog_Vorlage.dotx](https://github.com/cwsoft/Textblocks/blob/main/Kataloge/Textblocks_Katalog_Vorlage.docx?raw=true) enthalten. Jede Katalogdatei muss mindestens zwei Word-Formatvorlagen enthalten damit `Textblocks` die benötigten Metadaten der enthaltenen **Kategorien** und **Textblöcke** ermitteln kann.

Sofern über die erweiterten Eigenschaften einer Word-Katalogdatei: `MS-Word: Datei -> Informationen -> Eigenschaften -> Erweiterte Eigenschaften -> Anpassen` keine eigenen Formatvorlagen-Namen definiert wurden, werden folgende Formatvorlagen-Namen für die Extraktion der Metadaten verwendet:
- **tb_Kategorie**: `tb_Kategorie`
- **tb_Textblock**: `tb_Textblock`

## Lizenz
Textblocks wurde vom Autor im Oktober 2018 als rein privates Projekt gestartet und im Mai 2022 unter der [GNU General Public License](./LICENSE.txt) (Version 3) auf GitHub für Interessierte zur Verfügung gestellt. 

Viel Spaß
cwsoft
