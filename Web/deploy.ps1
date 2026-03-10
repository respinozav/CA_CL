Clear-Host

Write-Host ""
Write-Host "==============================="
Write-Host "INICIANDO DEPLOY AWS .NET"
Write-Host "==============================="
Write-Host ""

# ---------- CONFIG ----------
$key = "C:\Users\roder\OneDrive\Documentos\conociendoamistades.cl\keys\ubuntu-2\LightsailDefaultKey-us-east-1.pem"
$server = "ubuntu@98.87.29.132"
$remotePath = "/var/www/ca_cl"
$localPath = "C:\Users\roder\OneDrive\Documentos\conociendoamistades.cl\app\v1\CA_CL\Web\bin\Release\net8.0\publish"
$service = "ca_cl"

# ---------- VALIDAR PUBLICACION ----------
if (!(Test-Path $localPath)) {
    Write-Host "ERROR: No existe carpeta publish"
    pause
    exit
}

# ---------- LIMPIAR SERVIDOR ----------
Write-Host "Eliminando archivos antiguos..."
ssh -i "$key" "$server" "sudo rm -rf $remotePath/*"

# ---------- SUBIR ARCHIVOS ----------
Write-Host "Subiendo nueva version..."

scp -i "$key" -r "$localPath/." "$server`:$remotePath"

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR subiendo archivos"
    pause
    exit
}

# ---------- PERMISOS ----------
Write-Host "Asignando permisos..."
ssh -i "$key" "$server" "sudo chown -R www-data:www-data $remotePath"

# ---------- REINICIAR ----------
Write-Host "Reiniciando servicio..."
ssh -i "$key" "$server" "sudo systemctl restart $service"

# ---------- STATUS ----------
Write-Host ""
Write-Host "Estado servicio:"
ssh -i "$key" "$server" "sudo systemctl status $service --no-pager"

Write-Host ""
Write-Host "DEPLOY TERMINADO"
Write-Host "==============================="

pause