Clear-Host

Write-Host ""
Write-Host "==============================="
Write-Host "INICIANDO DEPLOY AWS .NET"
Write-Host "==============================="
Write-Host ""

# ---------- CONFIG ----------
$key = "C:\Proyectos\conociendoamistades.cl\keys\ubuntu-2\LightsailDefaultKey-us-east-1.pem"
$server = "ubuntu@98.87.29.132"
$remotePublishDir = "/home/ubuntu/ca_cl_publish"
$localPath = "C:\Proyectos\conociendoamistades.cl\app\v1\CA_CL\Web\bin\Release\net8.0\publish"
$remoteScript = "/home/ubuntu/sh/publicar.sh"

# ---------- VALIDAR PUBLICACION ----------
if (!(Test-Path $localPath)) {
    Write-Host "ERROR: No existe carpeta publish"
    pause
    exit
}

# ---------- SUBIR ARCHIVOS ----------
Write-Host "Subiendo nueva version a $remotePublishDir..."
scp -i "$key" -r "$localPath/." "$server`:$remotePublishDir"

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR subiendo archivos"
    pause
    exit
}

# ---------- EJECUTAR SCRIPT REMOTO ----------
Write-Host "Ejecutando publicar.sh en el servidor..."
ssh -i "$key" "$server" "bash $remoteScript"

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR ejecutando publicar.sh"
    pause
    exit
}

Write-Host ""
Write-Host "DEPLOY TERMINADO"
Write-Host "==============================="

pause