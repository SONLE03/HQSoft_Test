## Ex01
### 1. Overview
#### 1.1 Feature requirements
#### 1.2 Non-Feature requirements
### 2. Approach
#### 2.1 The problem
#### 2.2 The solution
### 3. Implementation
#### 3.1 User activity diagram (CRUD Book)
![ActivityDiagram1](https://github.com/user-attachments/assets/038bbcdf-0137-4696-9d28-7a64f14aef7a)
#### 3.2 Sequence diagram (Create Book)
![SequenceInsertPlan](https://github.com/user-attachments/assets/9df3acb1-d162-459e-b0bd-c45dd92737cd)
#### 3.3 Stored Procedure
##### 3.3.1 Author API (GetAllAuthors and GetAuthorById)
![image](https://github.com/user-attachments/assets/eb9d4fbb-934d-4ce4-8015-49d8ab764b03)
##### 3.3.2 Book API (GetAllBooks and GetBookById)
![image](https://github.com/user-attachments/assets/78087fb4-3437-4fd5-823b-9476c05050ed)
![image](https://github.com/user-attachments/assets/eebffc4e-538e-4b43-ae5a-59dcc21815fc)
##### 3.3.3 Filter
![image](https://github.com/user-attachments/assets/7c28bdec-5119-40b9-ba93-cff9e29ca154)

## Installation
### Docker
  To set up the project on Docker, follow these steps: Make sure Docker is installed and running properly on your computer
1. **Clone the repository**:
     git clone https://github.com/SONLE03/HQSoft_Test
     cd HQSoft_Test
2. **Start the server:**
   docker-compose up -d
### Local
1. **Clone the repository**:
     git clone https://github.com/SONLE03/HQSoft_Test
2. **Create database**: Open the MSSQL application and create a database to store records
3. **Modify the URL connection string**
   - Open the project with visualcode
   - Edit file application.json with template:  "DefaultConnection": "Server=(local);Database={DatabaseName};Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
   - Add database schema to mssql (Migration)
   - Run project.
   
