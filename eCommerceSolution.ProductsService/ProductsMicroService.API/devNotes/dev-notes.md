Project ProductsServivice
lembrete: fazer deploy com Docker ou em produção, lembre-se de não deixar senhas hardcoded no launchSettings.json. Use variáveis de ambiente reais ou secrets management sempre que possível.


ERROS
ERRO EXCEPTION NO POSTMAN, ENDPOINT GET PRODCUTS
"message": "Authentication to host 'localhost' for user 'root' using method 'caching_sha2_password' failed with message: Access denied for user 'root'@'localhost' (using password: NO)",
    "type": "MySql.Data.MySqlClient.MySqlException"

SOLUCAO: crei esta linha de odigo no depenencyInjection Console.WriteLine("Final MySQL Connection String: " + connectionString);
  para no console verificar se os dados estao a ser passados, depois verifiquei as variablesAmbient no appsettings,json, Dockerfile e DependencyInjection.cs.

  Docker compose, na versao 2 os comandos sao 
  

  ## 🛠️ Problema: Banco MySQL sem tabelas após subir o container

Ao subir o container MySQL com `docker-compose`, a aplicação não conseguia conectar corretamente, e mesmo acessando o banco manualmente, a tabela `Products` não existia.

**Erro exibido na aplicação:**  
`Unable to connect to any of the specified MySQL hosts.`

**Erro ao inspecionar o banco:**  
```sql
SHOW TABLES;
-- Resultado: Empty set

O script de inicialização db.sql não estava sendo corretamente lido, porque:

O volume com o caminho para db.sql no docker-compose.yaml não estava corretamente apontando para o arquivo.

Ou o container foi iniciado antes do volume estar corretamente montado (MySQL só executa scripts em /docker-entrypoint-initdb.d na primeira inicialização, e se o volume de dados estiver vazio).

✅ Solução:
Corrigi o volume no docker-compose.yaml para apontar corretamente o diretório com o db.sql:

yaml
Copiar código
volumes:
  - ./mysql-init:/docker-entrypoint-initdb.d
Removi o volume persistente antigo (que já estava com o banco sem dados):

bash
Copiar código
sudo docker volume prune
Removi o container antigo do MySQL:

bash
Copiar código
sudo docker rm -f <id-do-container>
Subi novamente o serviço com o comando:

bash

sudo docker compose up -d
Validei com os seguintes comandos dentro do container MySQL:

sql
Copiar código
SHOW DATABASES;
USE productsdatabase;
SHOW TABLES;
SELECT * FROM Products;
✅ Resultado Final:
O banco productsdatabase foi criado corretamente com a tabela Products, e a aplicação passou a conectar sem erros.

Erro ao actualizar o nome de um producto, vem erro de excpcao do rabbitmq
"message": "Exception of type 'RabbitMQ.Client.Exceptions.BrokerUnreachableException' estou a implementar try catch para apanhar a excepçao e logar o erro.e usar o for para tentar reconectar ao rabbitmq depois de 4 tentaivas.
_o erro estava que o nome da variavel de embiente, estava errado, HostName e no codigo estava Host._

No ACR adicionei os provider para o registry az provider register --namespace Microsoft.Insights --wait;
az provider register --namespace Microsoft.OperationsManagement --wait;
az provider register --namespace Microsoft.ContainerService --wait;
az provider register --namespace Microsoft.ContainerRegistry --wait;
az provider register --namespace Microsoft.Compute --wait;
az provider register --namespace Microsoft.Network --wait;
az provider register --namespace Microsoft.Storage --wait;
az provider register --namespace Microsoft.KeyVault --wait;
az provider register --namespace Microsoft.Monitor --wait;
az provider register --namespace Microsoft.Authorization --wait;