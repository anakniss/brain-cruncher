# observações;
- Nessa versão faz-se o uso de PostgreSQL com auxilio de Npgsql.
- Frontend WIP.
- Para mudar a String de conexão acessar ../backend/appsettings.json .
- Recomendação: caso esteja usando vscode instalar a extenção Vue-Official.
- Comandos:  npm run build ; npm run dev; 
- Pode-se também apertar o botão debug em ../Frontend/package.json .
# Mudanças Principais:
- Mudança completa do Frontend, implementando agora O framework Vue/vite, e uso de TypeScript.
- Adicionado parcialmente uma lógica para Autenticação de usuário(com hash em server-side).
- Mudança no UserController para poder implementar a autenticação.
- Mudança no Program.cs ("UseNpgsql" e AddScoped<Authenticator>()).



