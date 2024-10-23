set serviceName=DataJob.Server

sc stop   %serviceName% 
sc delete %serviceName% 

pause