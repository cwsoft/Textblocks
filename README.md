# Textblocks
Textblocks ist eine **Textbausteinverwaltung auf Grundlage von MS-Word**. Textbausteine werden in MS-Word erstellt, formatiert, kategorisiert und in Katalogdateien (.docx) verwaltet. Die erstellten Katalogdateien (.docx) können von Textblocks geladen, angezeigt und durchsucht werden. Der in der Vorschau angezeigte Textbaustein steht im Originallayout in der Zwischenablage zur Verfügung und kann über die **(STRG+V)** in andere Word-Dokumente eingefügt werden.

![Screenshot](./Dokumentation/screenshots/screenshot.png)

Textblocks spielt vor allem da seine Stärke aus, wo große Word-Dokumente ansonsten manuell nach dem richtigen Textbaustein durchsucht werden müssten, um im Anschluss  manuell in das Zieldokument kopiert zu werden. Weitere Informationen enthält die [Bedienungsanleitung](./Dokumentation/Textblocks.pdf).

## Mindestanforderungen
- Windows Betriebssystem (ab Windows 10)
- Microsoft Word (Word 2010-2019, Office-365 wird nicht unterstützt)
- Katalogdatei (\*.docx) mit Textbausteinen
- **Optional (wenn Textblocks selbst kompiliert wird):**
  - [Office PIA](./Textblocks/PIA/Howto-Office-PIA.md) `Microsoft.Office.Interop.Word.dll` (in MS-Word enthalten)
  - MS Visual Studio (oder kompatiblen C# Compiler) 
  - Textblocks lässt sich für NET Framework 4.8 bis Net 6 kompilieren und nutzt einige C# 10 Sprachfeatures

## Aufbau der Katalogdateien
Informationen zum Aufbau der Katalogdateien `Textblocks_Katalog.docx` sind im Beispielkatalog [Textblocks_Katalog_Vorlage.dotx](https://github.com/cwsoft/Textblocks/blob/main/Kataloge/Textblocks_Katalog_Vorlage.docx?raw=true) enthalten. Jede Katalogdatei muss mindestens zwei Word-Formatvorlagen enthalten, damit die Anwendung `Textblocks.exe` die jeweiligen Start- und Endpositionen aller enthaltenen **Kategorien** und **Textblöcke** ermitteln kann. 

Sofern über die erweiterten Eigenschaften der jeweiligen Katalogdatei (.docx) unter: `Datei -> Informationen -> Eigenschaften -> Erweiterte Eigenschaften -> Anpassen` keine eigenen Namen für die beiden Word-Formatvorlagen angegeben wurde, werden standardmäßig folgende Word-Formatvorlagen-Namen verwendet:
- **categoryStyleName**: `tb_Kategorie`
- **textblockStyleName**: `tb_Textblock`

## Lizenz
Die Anwendung `Textblocks.exe` wurde vom Autor im Oktober 2018 als rein privates Projekt erstellt und im Mai 2022 unter der [GNU General Public License](./LICENSE.txt) (Version 3) auf GitHub für Interessierte öffentlich zur Verfügung gestellt. 

Viel Spaß
cwsoft
