## Setup
1. Verbinden der DB mit dem Projekt
>1. SQLServer DB erstellen
>2. Im Explorer die versteckten Ordner anzeigen lassen
>3. C:\ProgramData\AppProject\appsettings.json erstellen
>4. Bsp. fuer den Inhalt der .json<br/>
>{<br/>
>&nbsp;&nbsp;&nbsp;"ConnectionStrings": {<br/>
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"Db": "data source=Localhost;initial catalog=AppProject;App=EntityFramework;Integrated Security=True;TrustServerCertificate=True"<br/>
>&nbsp;&nbsp;&nbsp;}<br/>
>}<br/>

2. Befuellen der DB
>1. Konsole in .\PFAX422A-AppProjekt\Shared\DatabaseDefintion oeffnen
>2. Updaten der DB mit gegebenen Migrationen<br/>
>"dotnet ef database update"

3. Freuen