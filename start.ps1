# start.ps1 - Sobe o backend e o frontend em paralelo
$root = $PSScriptRoot

Write-Host "Iniciando Backend (ASP.NET Core)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$root\src\ToDo.API'; dotnet run"

Write-Host "Iniciando Frontend (Angular)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$root\frontend'; npm start"

Write-Host ""
Write-Host "Backend  -> http://localhost:5273" -ForegroundColor Green
Write-Host "Frontend -> http://localhost:4200" -ForegroundColor Green
Write-Host ""
Write-Host "Feche as janelas abertas para encerrar os servidores." -ForegroundColor Yellow
