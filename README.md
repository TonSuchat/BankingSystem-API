# Banking System (.Net Core API solution)
***
# 💻 Technologies
- .Net Core (2.1.1)
- Entity Framework Core
- Microsoft Sql Server
- log4net
- Inmemory Database for UnitTest
***
# ✨ Features
- Create account
- Deposit money
- Transfer money between account
***
# 🔨 Structures
- API - MVC Web API: Provides endpoints that covers all 3 features (Account, Action controllers).
- Entity - Class library: Provides both database models and utility models for using in all projects.
- Logger - Class library: Provides generic logging system using log4net and log output to API/logs directory.
- Service - Class library: Provides all Interfaces, Services method that using in Business logic.
- UnitTest - Xunit: Provides unit test cover all api endpoints both success and error cases.