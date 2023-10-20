$assemblyFile = "D:\Code\MSSQL_DLL\bin\x64\Release\MSSQL_DLL.dll"
$stringBuilder = New-Object -Type System.Text.StringBuilder 

$fileStream = [IO.File]::OpenRead($assemblyFile)
while (($byte = $fileStream.ReadByte()) -gt -1) {
    $stringBuilder.Append($byte.ToString("X2")) | Out-Null
}
$stringBuilder.ToString() -join ""