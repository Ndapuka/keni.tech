Project ProductsServivice
lembrete: fazer deploy com Docker ou em produção, lembre-se de não deixar senhas hardcoded no launchSettings.json. Use variáveis de ambiente reais ou secrets management sempre que possível.


ERROS
ERRO EXCEPTION NO POSTMAN, ENDPOINT GET PRODCUTS
"message": "Authentication to host 'localhost' for user 'root' using method 'caching_sha2_password' failed with message: Access denied for user 'root'@'localhost' (using password: NO)",
    "type": "MySql.Data.MySqlClient.MySqlException"

SOLUCAO: crei esta linha de odigo no depenencyInjection Console.WriteLine("Final MySQL Connection String: " + connectionString);
  para no console verificar se os dados estao a ser passados, depois verifiquei as variablesAmbient no appsettings,json, Dockerfile e DependencyInjection.cs.
