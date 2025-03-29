# LightInvest

## Descrição do Projeto
O LightInvest é uma plataforma dedicada à educação e otimização de investimentos em energia renovável. O projeto visa fornecer informações detalhadas sobre tarifas energéticas, retorno sobre investimento (ROI) e eficiência energética, auxiliando consumidores e empresas na tomada de decisão.

## Funcionalidades
- **Consulta de Tarifas**: Apresenta informações sobre diferentes tarifas energéticas.
- **Simulação de ROI**: Permite calcular o retorno esperado sobre investimentos em energia renovável.
- **Artigos Educacionais**: Seção dedicada a educação energética com artigos sobre eficiência energética e sustentabilidade.
- **Gerenciamento de Eventos**: Funcionalidade para administrar eventos relacionados à educação energética.

## Tecnologias Utilizadas
- **Back-end**: ASP.NET Core, Entity Framework
- **Front-end**: Razor Pages
- **Banco de Dados**: MySQL
- **Autenticação**: Microsoft Identity

## Estrutura do Projeto
O projeto está organizado em:
- **Controllers**: Gerenciam as requisições HTTP.
- **ViewModels**: Organizam os dados para as Views.
- **Serviços**: Contêm a lógica de negócio.
- **Views**: Responsáveis pela interface do usuário.

## Instalação e Execução
1. Clone o repositório:
   ```sh
github.com/RodrigoElias202100213/LightInvest.git
   ```
2. Configure o banco de dados no `appsettings.json`.
3. Execute as migrações:
   ```sh
   dotnet ef database update
   ```
4. Inicie a aplicação:
   ```sh
   dotnet run
   ```

## Contribuição
Contribuições são bem-vindas! Para contribuir, faça um fork do repositório, crie uma branch para suas alterações e envie um pull request.

## Licença
Este projeto está licenciado sob a MIT License.

