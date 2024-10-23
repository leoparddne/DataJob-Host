set serviceName=DataJob.Server
set serviceFilePath=%~dp0\DataJob.Server.exe
set serviceDescription=DataJob.Server

sc create %serviceName%  BinPath=%serviceFilePath%
sc config %serviceName%    start=auto  
sc description %serviceName%  %serviceDescription%
sc start  %serviceName%
pause

