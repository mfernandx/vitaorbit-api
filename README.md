# 🚀 VitaOrbit API

## 📋 Sobre o Projeto

O **VitaOrbit** é uma plataforma de monitoramento de saúde inspirada nas tecnologias utilizadas em missões espaciais, adaptadas para aplicação em ambientes extremos e regiões remotas na Terra.

A proposta do projeto é permitir o acompanhamento contínuo das condições de saúde dos usuários, registrando sinais vitais, sintomas, condições ambientais e situações de emergência. Com base nessas informações, o sistema pode gerar alertas de risco que auxiliam na tomada de decisões e no monitoramento preventivo da saúde.

O projeto foi desenvolvido como parte da disciplina **Advanced Business Development with .NET**, utilizando **ASP.NET Core**, **Entity Framework Core** e **Oracle Database**.

---

# 🎯 Objetivo

Desenvolver uma API RESTful capaz de gerenciar informações relacionadas à saúde de usuários expostos a ambientes extremos, inspirando-se em tecnologias utilizadas em missões espaciais e transformando esse conhecimento em uma solução aplicável para desafios reais na Terra.

---

# 🛠️ Tecnologias Utilizadas

### Backend

* ASP.NET Core 9
* C#
* Entity Framework Core
* Oracle Entity Framework Core Provider
* Oracle Database
* OpenAPI
* Scalar API Reference

### Ferramentas de Desenvolvimento

* Microsoft Visual Studio 2022
* Postman
* Git
* GitHub

---

# 🏗️ Arquitetura da Aplicação

O projeto segue uma arquitetura baseada em:

```text
VitaOrbitApi
│
├── Controllers
│   ├── UserController
│   ├── HealthRecordController
│   ├── SymptomRecordController
│   ├── EnvironmentalConditionController
│   ├── EmergencyController
│   └── AlertController
│
├── Models
│   ├── User
│   ├── HealthRecord
│   ├── SymptomRecord
│   ├── EnvironmentalCondition
│   ├── Emergency
│   └── Alert
│
├── Data
│   └── AppDbContext
│
├── Migrations
│
├── Program.cs
│
└── appsettings.json
```

---

# 📊 Modelo de Dados

## User

Representa um usuário da plataforma.

Principais informações:

* Nome completo
* E-mail
* Senha
* Data de nascimento
* Gênero
* Descrição do usuário
* Localização atual
* Telefone
* Contato de emergência
* Data de registro

Relacionamentos:

* 1:N HealthRecords
* 1:N SymptomRecords
* 1:N Emergencies
* 1:1 EnvironmentalCondition

---

## HealthRecord

Representa registros de sinais vitais do usuário.

Informações armazenadas:

* Frequência cardíaca
* Pressão arterial
* Temperatura corporal
* Saturação de oxigênio
* Humor
* Nível de hidratação
* Horas de sono
* Observações
* Classificação de risco
* Data de registro

---

## SymptomRecord

Representa sintomas informados pelo usuário.

Informações armazenadas:

* Nome do sintoma
* Intensidade
* Frequência
* Data de início
* Descrição
* Classificação de risco
* Data de registro

---

## EnvironmentalCondition

Representa condições ambientais às quais o usuário está exposto.

Informações armazenadas:

* Temperatura externa
* Umidade
* Altitude
* Pressão atmosférica
* Qualidade do ar
* Nível de radiação
* Tipo de ambiente
* Data de registro

---

## Emergency

Representa solicitações de emergência realizadas pelo usuário.

Informações armazenadas:

* Localização
* Mensagem
* Status
* Data da solicitação

---

## Alert

Representa alertas gerados pelo sistema.

Pode estar vinculado a:

* Um registro de saúde (HealthRecord)
* Um registro de sintoma (SymptomRecord)

Informações armazenadas:

* Tipo do alerta
* Mensagem
* Nível de risco
* Data de registro

---

# 🔗 Relacionamentos

```text
User
├── HealthRecords (1:N)
├── SymptomRecords (1:N)
├── Emergencies (1:N)
└── EnvironmentalCondition (1:1)

HealthRecord
└── Alerts (1:N)

SymptomRecord
└── Alerts (1:N)
```

---

# 🤝 Modelo relacional

<img width="1361" height="680" alt="image" src="https://github.com/user-attachments/assets/4357cac5-02ee-40e3-9f5f-f8214a82c375" />


---

# 💡 Diagrama de caso de uso

<img width="8110" height="3891" alt="diagrama-casodeuso-vitaorbit" src="https://github.com/user-attachments/assets/348aa5dd-4d9c-4aad-8555-d57384adbe62" />


---

# ⚙️ Funcionalidades Principais

## Usuários

* Cadastro de usuário
* Login
* Busca por ID
* Listagem de usuários
* Atualização de e-mail
* Atualização de telefone
* Atualização de localização
* Atualização de contato de emergência
* Exclusão de usuário

## Registros de Saúde

* Cadastro de registro de saúde
* Classificação de risco
* Consulta por ID
* Consulta por usuário
* Listagem geral
* Exclusão

## Registros de Sintomas

* Cadastro de sintomas
* Classificação de risco
* Consulta por ID
* Consulta por usuário
* Listagem geral
* Exclusão

## Condições Ambientais

* Cadastro
* Consulta por ID
* Consulta por usuário
* Listagem geral
* Atualização
* Exclusão

## Emergências

* Abertura de solicitação
* Consulta por ID
* Consulta por usuário
* Listagem geral
* Atualização de status
* Exclusão

## Alertas

* Cadastro
* Consulta por ID
* Consulta por usuário
* Listagem geral
* Atualização de status
* Exclusão

---

# 🌐 Documentação OpenAPI

A documentação interativa da API pode ser acessada através do Scalar:

```text
https://localhost:7183/scalar/v1
```

ou

```text
http://localhost:5052/scalar/v1
```

---

# 🚀 Como Executar o Projeto

## 1. Clonar o repositório

```bash
git clone https://github.com/mfernandx/vitaorbit-api.git
```

---

## 2. Configurar a conexão com o banco Oracle

No arquivo:

```text
appsettings.json
```

Configurar:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "SUA_CONNECTION_STRING"
  }
}
```

---

## 3. Aplicar as Migrations

```bash
Add-Migration InitialCreate
```

```bash
Update-Database
```

---

## 4. Executar a aplicação

```bash
F5
```

ou

```bash
dotnet run
```

---

# 🧪 Exemplos de Testes — Endpoints com Body

---

## 👤 User

### Criar usuário

```http
POST /api/User
```

```json
{
  "fullName": "Maria Fernanda",
  "email": "maria@email.com",
  "password": "123456",
  "birthDate": "2000-01-01",
  "gender": "Feminino",
  "userDescription": "Usuária em ambiente remoto",
  "currentLocation": "Amazônia",
  "phoneNumber": "11999999999",
  "emergencyContact": "11888888888"
}
```

### Login

```http
POST /api/User/login
```

```json
{
  "email": "maria@email.com",
  "password": "123456"
}
```

### Alterar e-mail

```http
PUT /api/User/1/email
```

```json
{
  "email": "novoemail@email.com"
}
```

### Alterar telefone

```http
PUT /api/User/1/phone
```

```json
{
  "phoneNumber": "11977777777"
}
```

### Alterar localização

```http
PUT /api/User/1/location
```

```json
{
  "currentLocation": "Base de Pesquisa Antártica"
}
```

### Alterar contato de emergência

```http
PUT /api/User/1/emergency-contact
```

```json
{
  "emergencyContact": "11988888888"
}
```

---

## ❤️ HealthRecord

### Criar registro de saúde

```http
POST /api/HealthRecord
```

```json
{
  "userId": 1,
  "heartRate": 82,
  "bloodPressure": "120/80",
  "bodyTemperature": 36.7,
  "oxygenSaturation": 98,
  "mood": "Estável",
  "hydrationLevel": 75.5,
  "sleepHours": 7.5,
  "notes": "Sem alterações graves."
}
```

---

## 🤒 SymptomRecord

### Criar registro de sintoma

```http
POST /api/SymptomRecord
```

```json
{
  "userId": 1,
  "symptomName": "Dor de cabeça",
  "intensity": 5,
  "frequency": "Intermitente",
  "startedAt": "2026-06-05",
  "description": "Dor moderada após exposição prolongada ao calor."
}
```

---

## 🌎 EnvironmentalCondition

### Criar condição ambiental

```http
POST /api/EnvironmentalCondition
```

```json
{
  "userId": 1,
  "externalTemperature": -5.5,
  "humidity": 35.0,
  "altitude": 3200.0,
  "atmosphericPressure": 690.5,
  "airQuality": "Moderada",
  "radiationLevel": 0.15,
  "environmentType": "Alta montanha"
}
```

### Atualizar condição ambiental

```http
PUT /api/EnvironmentalCondition/1
```

```json
{
  "externalTemperature": -8.2,
  "humidity": 30.0,
  "altitude": 3500.0,
  "atmosphericPressure": 675.3,
  "airQuality": "Baixa",
  "radiationLevel": 0.21,
  "environmentType": "Ambiente extremo de altitude"
}
```

---

## 🚨 Emergency

### Criar emergência

```http
POST /api/Emergency
```

```json
{
  "userId": 1,
  "location": "Coordenadas aproximadas: -23.5505, -46.6333",
  "message": "Sinto tontura intensa e extrema falta de ar."
}
```

### Atualizar status da emergência

```http
PUT /api/Emergency/1/status
```

```json
{
  "status": "Fechada"
}
```

---

## ⚠️ Alert

### Criar alerta vinculado a um registro de saúde

```http
POST /api/Alert
```

```json
{
  "userId": 1,
  "healthRecordId": 1,
  "symptomRecordId": null,
  "typeAlert": "Oxigenação baixa",
  "message": "A saturação de oxigênio registrada está abaixo do nível ideal.",
  "riskLevel": "Alto"
}
```

### Criar alerta vinculado a um registro de sintoma

```http
POST /api/Alert
```

```json
{
  "userId": 1,
  "healthRecordId": null,
  "symptomRecordId": 1,
  "typeAlert": "Sintoma intenso",
  "message": "O sintoma registrado possui intensidade elevada e exige atenção.",
  "riskLevel": "Moderado"
}
```

> Um alerta deve estar vinculado obrigatoriamente a um `HealthRecord` ou a um `SymptomRecord`, mas nunca aos dois ao mesmo tempo.

---

# 📱 Integração com Frontend

A API foi projetada para integração futura com um aplicativo mobile desenvolvido em:

* React Native
* Expo
* TypeScript

As requisições poderão ser realizadas através de bibliotecas como:

* Axios
* Fetch API

Exemplo:

```typescript
const api = axios.create({
  baseURL: "http://192.168.X.X:5052/api"
});
```

---

# 🎥 Link do Pitch

```text
https://youtu.be/igqC9mIPjn0?si=m8pwFMwx-kx_2Iea
```

---

# 👨‍💻 Integrantes da Equipe

* Maria Fernanda Santos Mendes - 2TDSPI
* Beatriz de Sousa Franco – 2TDSPI
* Natan Freitas de Moraes - 2TDSPI
* Giovana Souza Vieira - 2TDSPI 
