# Banking System (.Net Core API solution)
# 💻 Technologies
- .Net Core (2.1.1)
- Entity Framework Core
- Microsoft Sql Server
- log4net
- Inmemory Database for UnitTest
---
# ✨ Features
- Create account
- Deposit money
- Transfer money between account
---
# 🔨 Structures
- API - MVC Web API: Provides endpoints that covers all 3 features (Account, Action controllers).
- Entity - Class library: Provides both database models and utility models for using in all projects.
- Logger - Class library: Provides generic logging system using log4net and log output to API/logs directory.
- Service - Class library: Provides all Interfaces, Services method that using in Business logic.
- UnitTest - Xunit: Provides unit test cover all api endpoints both success and error cases.
***
# 📄 How to run
- Clone the project.
- Change connectionstring in BnakingSystem/API/appsettings.json file.
- Run API project, For the first time when the project run it'll create database automatically.
- You can use postman for testing the API endpoints, I've prepared Postman json file in document/BankingSystem.postman_collection.json
- Also you can test the api with UnitTest project.